using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/** This component handles controlling the home screen */
public class HomeController : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  [Header("UI Elements")]
  public GameObject catCondo;
  public GameObject catCondoPlatform;
  public RectTransform centerDisplay;
  public RectTransform leftDisplay;

  public Text catCount;
  
  /***** Private Variables *****/
  private Vector2 _catCondoPosition; // Stores the original position of the cat condo.
  private GameObject[] _cats; // Stores all the cats in the scene. 
  private List<Vector2> _catCondoCatPositions = new List<Vector2>(); // Stores the orignal position of the cats in the cat condo.
  private Vector2 _catCondoPlatformPosition; // Stores the original position of the cat condo platform.
  private Vector2 _centerDisplayPosition; // Stores the original position of the left display.
  private Vector2 _leftDisplayPosition; // Stores the original position of the left display.
  
  // Petshop button
  // Nap room button
  // Pokedex button
  // Help button
  // CatTower
  

  // Start is called before the first frame update
  void Start() {
    // Save the original positions of the catCondo and left display
    _catCondoPosition = catCondo.transform.position;
    // Save the original position of the cats in the cat condo
    _cats = GameObject.FindGameObjectsWithTag("Cat");
    foreach (GameObject cat in _cats) {
      _catCondoCatPositions.Add(cat.transform.position);
    }
    // Save the original position of the other elements
    _catCondoPlatformPosition = catCondoPlatform.transform.position;
    _centerDisplayPosition = centerDisplay.position;
    _leftDisplayPosition = leftDisplay.position;
  }

  // Update is called once per frame
  void Update() {
    /*** Update Display ***/
    // UpdateCatDisplay(); TODO: Do this
    // FIXME: Create another controller for this.
    // UpdateLevelProgressDisplay(); TODO: Do this
    
    /*** Move CatCondo and Left Buttons when breathing ***/
    if (Model.Instance.gameState == Model.GameState.Breathing) {
      Vector2 newPosition = new Vector2(-3, 0);
      catCondo.transform.position = _catCondoPosition + newPosition;
      for(int i= 0; i < _cats.Length; i++) {
        _cats[i].transform.position = _catCondoCatPositions[i] + newPosition * 39;
      }
      catCondoPlatform.transform.position = _catCondoPlatformPosition + newPosition;
      centerDisplay.transform.position = _centerDisplayPosition + newPosition;
      leftDisplay.transform.position = _leftDisplayPosition + newPosition;
    } else {
      catCondo.transform.position = _catCondoPosition;
      for(int i= 0; i < _cats.Length; i++) {
        _cats[i].transform.position = _catCondoCatPositions[i];
      }
      catCondoPlatform.transform.position = _catCondoPlatformPosition;
      centerDisplay.transform.position = _centerDisplayPosition;
      leftDisplay.transform.position = _leftDisplayPosition;
    }
    
    // TODO: Update the food amount
    // TODO: Money animation
    // TODO: PawPoints animation
    // TODO: Update PawPoints
  }
  
  /****** Private Methods *****/
  public void GoToNappingRoom() {
    SceneManager.LoadScene("Pace Breathing");
  }
  public void GoToPetShop() {
    SceneManager.LoadScene("Pet Shop");
  }
}
