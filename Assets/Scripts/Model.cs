using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Handles retrieving, updating and saving persistent data for the game.
 *
 * IMPORTANT: There should be a GameObject with this Model script available in
 * each scene since we are using the Unity Update methods for our point
 * increment interval.
 *
 * NOTE: This model saves to disk each second on platform, not on web.
 */
public class Model : MonoBehaviour {
  /***** Public Variables *****/
  // Easy way to get access to the Model instance using a static reference.
  public static Model Instance { get; private set; }
  // Getter for the gameData. THIS IS NOT EDITABLE.
  public GameData gameData => _gameData;
  
  /*** Game State ***/ 
  public enum GameState {
    // When the game is running in the background
    Idle,
    // When the player is performing paced breathing
    Breathing
  };
  // Public accessible getter and setter for the game state since other components can control this.
  public GameState gameState = GameState.Idle;
  
  // Public state for if the player is breating in or out
  public bool isBreathingIn = false;

  /***** Private Variables *****/
  private float _secondsSincePointUpdate;
  private static GameData _gameData;
  
  /***** Unity Methods *****/
  /** Make sure there is only one instance of this model. Using a Singleton pattern */
  void Awake() {
    // Keep this object around for the lifetime of the game.
    DontDestroyOnLoad(gameObject);
    
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
    
    // Load the game data from disk (this is done on awake so that we can access it quickly)
    // On platform get the date from the disk 
    _gameData = GetGameData();
  }

  void Start() {
    DateTime todayDate = DateTime.Now;
    
    // Streak calculation
    DateTime lastLoginDate = _gameData.lastLoginDate != "" ? DateTime.Parse(_gameData.lastLoginDate) : todayDate;
    if (!todayDate.Equals(lastLoginDate)) {
      // Then reset the session data
      _gameData.deepBreathingSession = 0;
      _gameData.pacedBreathingSession = 0;
      
      // And if it has been one day since the last login
      if (todayDate.Subtract(lastLoginDate).Days == 1) {
        // Increase the streak
        _gameData.dayStreak++;
      } 
      // Or if it has been more than one day since the last login
      else if (todayDate.Subtract(lastLoginDate).Days > 1) {
        // Reset the streak
        _gameData.dayStreak = 1;
      }
      
      // Finally update the last login date and save the game data
      _gameData.lastLoginDate = todayDate.ToString("yyyy MMMM dd");
    }

    // Points calculation (since last update)
    DateTime pointsUpdatedAt = _gameData.pointsUpdatedAt != "" ? DateTime.Parse(_gameData.pointsUpdatedAt) : todayDate;
    int      secondsSinceLastUpdate = (int)todayDate.Subtract(pointsUpdatedAt).TotalSeconds;
    
    // LIMIT to only allow for 1h of offline point accumulation
    secondsSinceLastUpdate = Math.Min(secondsSinceLastUpdate,  3600);

    int      pointsSinceLastUpdate = secondsSinceLastUpdate * UpdatePoints();
    _gameData.points += pointsSinceLastUpdate;
    _gameData.pointsUpdatedAt = DateTime.Now.ToString("yyyy MMMM dd HH:mm:ss");
    
    int      dollarsSinceLastUpdate = secondsSinceLastUpdate * UpdateDollars();
    _gameData.dollars += dollarsSinceLastUpdate;
    
    // Add a flag to show how many points and dollars the user has earned since last update
    if(secondsSinceLastUpdate > 0) {
      // If there was anytime away from the game since the last update
      // store the amount of points and dollars earned since last update
      _gameData.dollarsSinceLastUpdate = dollarsSinceLastUpdate;
    }
    
    SaveGameData(_gameData);
  }

  void Update() {
    // Update time
    _secondsSincePointUpdate += Time.deltaTime;

    // Every second, perform the following actions
    if (_secondsSincePointUpdate >= 1) {
      _secondsSincePointUpdate -= 1;
      UpdateDollarsAndPoints();
      UpdateFood();
      UpdateLevel();
    }
  }
  
  /***** Public Methods *****/
  /** Resets the game date for a new user */
  public static void ResetGameData() {
    // Instantiate a new GameDate object
    _gameData = new GameData();
    
    // Set last login date to today
    _gameData.lastLoginDate = DateTime.Now.ToString("yyyy MMMM dd");
    
    // Set points updated at to today
    _gameData.pointsUpdatedAt = DateTime.Now.ToString("yyyy MMMM dd HH:mm:ss");
    
    // Set the food percentage to 100
    _gameData.foodBarPercentage = 1;

    // Make sure the user starts at level 1 
    _gameData.level = 1;
    // And the user should have one cat.
    _gameData.cats = new List<Cat>();
    Cat newCat = Cat.CreateCat();
    newCat.Adopt();
    _gameData.cats.Add(newCat);
    
    // Save the game data
    SaveGameData(_gameData);
  }
  
  
  /** Given a cat, checks if the user can adopt a cat and adopts it. */
  public static void AdoptCat(Cat cat) {
    // Given a cat
    // And the user has enough space
    if (!HasEnoughSpace()) throw new Exception("Not enough space to adopt a cat");
    // And the user has enough money
    if (!HasEnoughMoney(cat)) throw new Exception("Not enough money to adopt a cat");
    
    // Then adopt the cat
    // By deducting the dollar
    _gameData.dollars -= cat.price;
    // And by updating the cat adoption date
    cat.Adopt();
    // And by adding the cat to the list of cats
    _gameData.cats.Add(cat);
    
    // Save the game data
    SaveGameData(_gameData);
  }
  
  /** Helper to determine if the user has enough space to adopt a cat. */
  public static bool HasEnoughSpace() {
    int remainingSpaceOnCatTower = Level() - Cats().Count;
    return remainingSpaceOnCatTower > 0;
  }
  
  /** Helper to determine if the user has enough money to adopt a cat. */
  public static bool HasEnoughMoney(Cat cat) {  return Dollars() >= cat.price; }
  
  public static void AddPoints(int points) {
    _gameData.points += points;
    SaveGameData(_gameData);
  }
  
  public static void AddDollars(int dollars) {
    _gameData.dollars += dollars;
    SaveGameData(_gameData);
  }

  /***** Getters *****/
  public static List<Cat> Cats() {  return _gameData.cats; }
  public static int Level() { return _gameData.level; }
  public static int Dollars() {  return _gameData.dollars; }
  public static int DollarsPerSecond() {
    int dollarsPerSecond = 0;
    Cats().ForEach(cat => dollarsPerSecond += cat.dollarsPerSecond);
    return dollarsPerSecond;
  }
  public static int Points() { return _gameData.points; }

  public static int PointsToNextLevel() {
    if (Points() < Math.Pow(2, 05)) return (int)Math.Pow(2, 5);
    if (Points() < Math.Pow(2, 06)) return (int)Math.Pow(2, 6);
    if (Points() < Math.Pow(2, 07)) return (int)Math.Pow(2, 7);
    if (Points() < Math.Pow(2, 08)) return (int)Math.Pow(2, 8);
    if (Points() < Math.Pow(2, 09)) return (int)Math.Pow(2, 9);
    if (Points() < Math.Pow(2, 10)) return (int)Math.Pow(2, 10);
    if (Points() < Math.Pow(2, 11)) return (int)Math.Pow(2, 11);
    if (Points() < Math.Pow(2, 12)) return (int)Math.Pow(2, 12);
    if (Points() < Math.Pow(2, 13)) return (int)Math.Pow(2, 13);
    if (Points() < Math.Pow(2, 14)) return (int)Math.Pow(2, 14);
    if (Points() < Math.Pow(2, 15)) return (int)Math.Pow(2, 15);
    return (int) Math.Pow(2, 15); // Represents the max level
  }

  /***** Private Methods *****/
  /** Update points. This is used at each interval (1 second). */
  private int UpdatePoints() {
    int points = 0;
    
    // For each cat
    Cats().ForEach(cat => {
      // Add the cat's points per second
      points += cat.pointsPerSecond;
    });

    return points;
  }
  
  /** Update dollars. This is used at each interval (1 second). */
  private int UpdateDollars() {
    int dollars = 0;
    
    // For each cat
    Cats().ForEach(cat => {
      // Add the cat's dollars per second
      dollars += cat.dollarsPerSecond;
    });

    return dollars;
  }
  
  /** Updates the user's points and dollars */
  private void UpdateDollarsAndPoints() {
    // Update points
    _gameData.points += UpdatePoints();
    _gameData.pointsUpdatedAt = DateTime.Now.ToString("yyyy MMMM dd HH:mm:ss");
    
    // Update dollars
    _gameData.dollars += UpdateDollars();

    // Save the game data
    SaveGameData(_gameData);
  }
  
  /** Increase the food bar when the player is not breathing */
  private void UpdateFood() {
    // When the game is in a idle state
    if (gameState == GameState.Idle) {
      // Each second empty the food dish
      UpdateFoodDish(false);
      
      // And increase the food bar only if it's not full.
      if (_gameData.foodBarPercentage >= 1) {
        // Make sure it's not exceeding 100%
        _gameData.foodBarPercentage = 1;
      }
      // If the food bar is not full 
      else {
        // Increase the food bar at a speed which will take 30 minutes to fill
        _gameData.foodBarPercentage += 1f / (60f * 30f);
      }
    }
    // When the player is performing paced breathing, decrease the food amount
    else if (gameState == GameState.Breathing) {
      // If the food bar is empty
      if(_gameData.foodBarPercentage <= 0) {
        // Make sure it's not below 0%
        _gameData.foodBarPercentage = 0;
      }
      // If the food bar is not empty
      else {
        // Decrease the food bar at a speed which will take 5 minutes to empty
        _gameData.foodBarPercentage -= 1f / (60f * 5f);
        
        // Fill the food dish
        UpdateFoodDish(true);
      }
    }
  }

  private void UpdateFoodDish(bool isFilling) {
    if (isFilling) {
      // Handle overfill
      if (_gameData.foodDishPercentage >= 1) {
        _gameData.foodDishPercentage = 1;
      }
      else {
        // Increase the food dishes at a speed which will take 10 minutes to fill.
        _gameData.foodDishPercentage += 1f / (60f * 10f);
      }
    }
    else {
      // Handle underfill
      if (_gameData.foodDishPercentage <= 0) {
        _gameData.foodDishPercentage = 0;
      }
      else {
        // Decrease the food dishes at a speed which will take 60 minutes to empty.
        _gameData.foodDishPercentage -= 1f / (60f * 60f);
      }
      
    }
  }
  
  /** Updates the user's level */
  private void UpdateLevel() {
    int currentLevel   = Level();
    int potentialLevel = 11; // Represents the max level
    
    // Determine what level the user should be
    if (Points() < Math.Pow(2, 5)) potentialLevel = 1;
    else if (Points() < Math.Pow(2, 6)) potentialLevel = 2;
    else if (Points() < Math.Pow(2, 7)) potentialLevel = 3;
    else if (Points() < Math.Pow(2, 8)) potentialLevel = 4;
    else if (Points() < Math.Pow(2, 9)) potentialLevel = 5;
    else if (Points() < Math.Pow(2, 10)) potentialLevel = 6;
    else if (Points() < Math.Pow(2, 11)) potentialLevel = 7;
    else if (Points() < Math.Pow(2, 12)) potentialLevel = 8;
    else if (Points() < Math.Pow(2, 13)) potentialLevel = 9;
    else if (Points() < Math.Pow(2, 14)) potentialLevel = 10;
    else if (Points() < Math.Pow(2, 15)) potentialLevel = 11;
    
    // Determine if the user has leveled up
    if (currentLevel != potentialLevel) {
      // Increase the user's level
      _gameData.level = potentialLevel;
      // Save the game data
      SaveGameData(_gameData);
    }
  }
  
  private static GameData GetGameData() {
    #if UNITY_STANDALONE
      // Get the game date from dist when on a platform (not web)
      string serializedData = File.ReadAllText(Application.streamingAssetsPath + "/gameData.json");
      return JsonUtility.FromJson<GameData>(serializedData);
    #else
      // On web, get the game date from IndexDB
      string serializedData = PlayerPrefs.GetString("gameData");
      
      // Handle the case where the game data is not yet initialized
      if(serializedData == "") {
        // Reset the game date
        ResetGameData();
        // Return this new date data
        return _gameData;
      }
      return JsonUtility.FromJson<GameData>(serializedData);
#endif
  }

  public static void SaveGameData(GameData gameData) {
    #if UNITY_STANDALONE
      // Only save to disk when on a platform and not the web
      string serializedData = JsonUtility.ToJson(gameData, true);
      File.WriteAllText(Application.streamingAssetsPath + "/gameData.json", serializedData);
    #else 
      // Save to IndexDB when on the web
      string serializedData = JsonUtility.ToJson(gameData, true);
      PlayerPrefs.SetString("gameData", serializedData);
    #endif
  }

  /***** Private Classes *****/
  /**
   * Represents a cat.
   * All public fields are serializable. Add more fields as needed.
   */
  [Serializable]
  public class Cat {
    /***** Fields *****/
    public string specie;    // Name of the cat species
    public int    rarity;     // Rarity of the cat (between 1 and 10)
    public int    variant;    // Variant of the cat (between 0 and 2)
    public string adoptedOn;  // String date when the cat was adopted
    public int    price;      // Price of the cat
    public int    pointsPerSecond; // The amount of paw points the cat gives per second
    public int    dollarsPerSecond; // The amount of dollars the cat gives per second
    
    /***** Static Public Methods *****/
    public static Cat CreateCat() {
      // Instantiate a new cat
      Cat cat = new Cat();
      
      // Based on the user's level, we will generate a rarity between 1 and Min(userLevel, 10)
      cat.rarity = Random.Range(1, Mathf.Min(Level(), 11));

      // Generate all other fields for the cat
      cat.specie            = GenerateSpecies(cat.rarity);
      cat.variant           = Random.Range(0, 3);
      cat.price             = GeneratePrice(cat.rarity);
      cat.pointsPerSecond   = cat.rarity; // A cat generates points per second based on its rarity value
      cat.dollarsPerSecond  = cat.price / 100; // A cat generates 100th of it's price per second in dollars;

      return cat;
    } 
    
    public static String GenerateSpecies(int rarity) {
      switch (rarity) {
        case 1: return "Felis Alfredo";
        case 2: return "Felis Bucatini";
        case 3: return "Felis Rigatoni";
        case 4: return "Felis Muzzelune";
        case 5: return "Felis Fetuccine";
        case 6: return "Felis Cannelloni";
        case 7: return "Felis Noodadles";
        case 8: return "Felis Sushi";
        case 9: return "Felis P. lotor";
        case 10: return "Felis David";
        default: return "Felis";
      }
    }
    
    public static int GeneratePrice(int rarity) {
      switch (rarity) {
        case 1: return 1000;
        case 2: return 2500;
        case 3: return 5000;
        case 4: return 8500;
        case 5: return 12000;
        case 6: return 17000;
        case 7: return 23000;
        case 8: return 27000;
        case 9: return 32000;
        case 10: return 37000;
        default: return 0;
      }
    }
    
    /***** Public Methods *****/
    public void Adopt() {
      // Set the date when the cat was adopted
      adoptedOn = DateTime.Today.ToString("yyyy MMMM dd");
    }
  }
  
  /**
   * Represents the game data which is saved on disk. Any public fields are
   * serializable. Add more fields as needed.
   */
  [Serializable]
  public class GameData {
    /***** Fields *****/
    // Level
    public int level = 1; // The user's level.
    // Points
    public int points;    // The players points. This is used to calculate the level.
    public string pointsUpdatedAt; // The datetime when the points were last updated.
    public int dollars;   // The players dollars. This is used to purchase cats.
    
    // Cats
    public List<Cat> cats; // The list of adopted cats that the player has purchased.
    
    // Food Information
    public float foodBarPercentage; // The percentage of the food bar that is filled
    public float foodDishPercentage; // The percentage of the food dish that is filled

    // Analytics
    public string lastLoginDate; // Used to increment or break the streak
    public int dayStreak; // Streak for the amount of days the player has opened the game.
    public int deepBreathingSession; // Number of deep breathing session today. Will reset when last login date is changed
    public int pacedBreathingSession; // Number of paced breathing session today. Will reset when last login date is changed
    
    // Offline Data
    public int dollarsSinceLastUpdate; // The amount of dollars since the last update.
  }
}
