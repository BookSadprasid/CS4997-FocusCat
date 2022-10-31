using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatCondo2Controller : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  public GameObject[] catSpots; // Possible places cats can sit.
  public Button leftButton; // Button to move left.
  public Button rightButton; // Button to move left.
  
  /***** Private Variables *****/
  private int _page = 0; // Current page of cats.
  
  /***** Unity Methods *****/
  void Start() {
    /*** Populate Cats ***/
    UpdateCats();
    
    /*** Button Handlers ***/
    leftButton.onClick.AddListener(() => {  _page--; UpdateCats(); });
    rightButton.onClick.AddListener(() => { _page++; UpdateCats(); });
  }

  void Update() {
    leftButton.gameObject.SetActive(ShouldShowLeftButton());
    rightButton.gameObject.SetActive(ShouldShowRightButton());
  }
  
  /***** Private Methods *****/
  private bool ShouldShowLeftButton() {
    return _page != 0;
  }
  
  private bool ShouldShowRightButton() {
    // Show the right button if there are more cats than cat spots
    int numberOfCats = Model.Cats().Count;
    int numberOfCatSpots = catSpots.Length;
    
    // Do not show if there is more cat spots than cats
    if(numberOfCats < numberOfCatSpots) {
      return false;
    }
    
    // In the case that there are more cats than cat spots, determine if there
    // are anymore cats to show given the page we are on.
    int remainingCats = numberOfCats - (_page * numberOfCatSpots);
    return remainingCats > 0;
  }

  private void UpdateCats() {
    // Generate a set of numbers representing the cat spot indexes
    int[] randomNumbers = Enumerable.Range(0, catSpots.Length).OrderBy(x => Random.value).Take(Model.Cats().Count).ToArray();
    
    // For each cat spot, show a cat based on the page
    int fromIndex = _page * catSpots.Length;
    int toIndex   = Math.Min(Model.Cats().Count, fromIndex + catSpots.Length);
    for (int i = fromIndex; i < toIndex; i++) {
      catSpots[randomNumbers[i % catSpots.Length]].SetActive(true);
      catSpots[randomNumbers[i % catSpots.Length]].GetComponent<CatController>().cat = Model.Cats()[i];
      catSpots[randomNumbers[i % catSpots.Length]].GetComponent<CatController>().showDollars = true;
    }
  }
}
