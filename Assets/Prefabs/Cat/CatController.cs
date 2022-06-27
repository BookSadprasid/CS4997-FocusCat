using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  [Header("UI Elements")]
  public GameObject dollarsPerSecondContainer;
  public NumberController dollarsPerSecondNumberController;
  
  /***** Sprites *****/
  [Header("Sprites")]
  public Sprite[] 
    catRarity0Sprites, 
    catRarity1Sprites,
    catRarity2Sprites,
    catRarity3Sprites,
    catRarity4Sprites,
    catRarity5Sprites,
    catRarity6Sprites,
    catRarity7Sprites,
    catRarity8Sprites,
    catRarity9Sprites;
  
  /***** Configuration *****/
  [Header("Configuration")]
  public Model.Cat cat; // Cat information to be displayed
  public bool showDollars; // Whether to show the dollars per second
  
  /***** Private Variables *****/
  private Image _image;
  private float _animationLengthSeconds = 2; // How long the scale animation should take
  private float _animationTime; // Running time since the last animation
  
  /***** Unity Methods *****/
  void Start() {
    _image = GetComponent<Image>();
    
    // Set the cat's dollars per second
    dollarsPerSecondNumberController.number = cat.dollarsPerSecond;
    
    // Move the dollars per second container to starting position
    dollarsPerSecondContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
    // And scale down the dollars per second container
    dollarsPerSecondContainer.transform.localScale = new Vector3(0, 0, 0);
  }
  
  // Each frame
  void Update() {
    // Update the cats sprite based on the cat's rarity and variant
    _image.sprite = GetCatSprite(cat.rarity, cat.variant);

    // Don't perform points animations after this points if we don't want it.
    if (!showDollars) {
      dollarsPerSecondContainer.SetActive(false);
      return;
    }
    dollarsPerSecondContainer.SetActive(true);

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
      _animationTime = 0;
      
      // Move the dollars per second container to starting position
      dollarsPerSecondContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
      // And scale down the dollars per second container
      dollarsPerSecondContainer.transform.localScale = new Vector3(0, 0, 0);
    }
    
  }
  
  /***** Private Methods *****/
  private Sprite GetCatSprite(int rarity, int variation) {
    switch (rarity) {
      case 1: return catRarity0Sprites[variation];
      case 2: return catRarity1Sprites[variation];
      case 3: return catRarity2Sprites[variation];
      case 4: return catRarity3Sprites[variation];
      case 5: return catRarity4Sprites[variation];
      case 6: return catRarity5Sprites[variation];
      case 7: return catRarity6Sprites[variation];
      case 8: return catRarity7Sprites[variation];
      case 9: return catRarity8Sprites[variation];
      case 10: return catRarity9Sprites[variation];
      default: return null;
    }
  }
}
