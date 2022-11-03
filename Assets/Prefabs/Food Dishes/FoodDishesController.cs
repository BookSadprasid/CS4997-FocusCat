using UnityEngine;

public class FoodDishesController : MonoBehaviour {
  /***** Public Variables *****/
  [Header("UI Elements")]
  public GameObject dollarsPerSecondContainer;
  public NumberController dollarsPerSecondNumberController;
  
  /***** Private Variables *****/
  private FoodDishController[] _foodDishControllers;
  private AudioSource _audioSource;
  
  // Dollar animation
  private float _animationLengthSeconds = 2; // How long the scale animation should take
  private float _animationTime; // Running time since the last animation
  private Vector2 startingPosition;
  private bool _hasAnimated;
  private int dollarsPerBreath;

  /***** Unity Methods *****/
  void Start() {
    // Find all children food dishes
    _foodDishControllers = GetComponentsInChildren<FoodDishController>();
    
    // Save the audio source
    _audioSource = GetComponent<AudioSource>();
    
    // Set the dollars per second which is number of cats * 10
    dollarsPerBreath = Model.Cats().Count * 10;;
    dollarsPerSecondNumberController.number = dollarsPerBreath;
    
    // Move the dollars per second container to starting position
    startingPosition = dollarsPerSecondContainer.GetComponent<RectTransform>().anchoredPosition;
    // And scale down the dollars per second container
    dollarsPerSecondContainer.transform.localScale = new Vector3(0, 0, 0);
  }

  void Update() {
    // Find the food dish fullness percentage.
    float foodDishPercentage = Model.Instance.gameData.foodDishPercentage;

    // Given that the foodDishPercentage is a value between 0 and 1,
    // we can use it to find the index of the food dish that is full.
    int maxFoodDishSpriteIndex = _foodDishControllers[0].foodDishSprites.Length - 1;
    int foodDishSpriteIndex = (int) Mathf.Floor(foodDishPercentage * (maxFoodDishSpriteIndex));
    
    // Iterate over each child food dish and update it's sprite image
    foreach (FoodDishController foodDishController in _foodDishControllers) {
      foodDishController.foodDishSpriteIndex = foodDishSpriteIndex;
    }
    
    // When breathing, animate the dollars per second container
    if (Model.Instance.gameState == Model.GameState.Breathing) {
      // When breathing in, we want to show the animation
      if (Model.Instance.isBreathingIn) {
        
        // We also don't want to animate two times so we check if we have already animated
        if (!_hasAnimated) {
          // Play the sound when the _animationTime is 0
          if (_animationTime == 0) {
            _audioSource.Play();
            
            // Add dollars to the Model
            Model.AddDollars(dollarsPerBreath);
          }
          
          // If we have no animated, we show the dollars per second
          dollarsPerSecondContainer.SetActive(true);
          
          // Start the track the animation time
          _animationTime += Time.deltaTime;

          if (_animationTime <= _animationLengthSeconds) {
            // Scale and move for half the animation time
            if (_animationTime / 2 <= _animationLengthSeconds) {
              // Move the container up
              dollarsPerSecondContainer.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 20f * Time.deltaTime / _animationTime / 5);
              // Scale up the container
              dollarsPerSecondContainer.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime / _animationTime / 7;   
            }
          }
          else {
            // Once the animation is complete, we stop animating
            _hasAnimated = true;
            
            // Rese the animation time
            _animationTime = 0;
            
            // Reset the number
            // Move the dollars per second container to starting position
            dollarsPerSecondContainer.GetComponent<RectTransform>().anchoredPosition = startingPosition;
            // And scale down the dollars per second container
            dollarsPerSecondContainer.transform.localScale = new Vector3(0, 0, 0);
          }
        }
      }
      // Once we are breathing out, we update the animation to false so that
      // we can animate again at the next breath.
      else {
        _hasAnimated = false;
      }
    }
    else {
      dollarsPerSecondContainer.SetActive(false);
    }
  }

}
