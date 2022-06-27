using UnityEngine;
using UnityEngine.UI;

public class FoodDishController : MonoBehaviour {
  /***** Public Variables *****/
  // Represents the different sprites of the food dish
  public Sprite[] foodDishSprites;
  // Represents which food dish sprite to render
  [Range(0, 8)]
  public int foodDishSpriteIndex;
  
  /***** Private Variables *****/
  private Image _image;
  
  /***** Unity Methods *****/
  void Start() {
    // Get the image component
    _image = GetComponent<Image>();
  }

  void Update() {
    // Set the food dish sprite
    _image.sprite = foodDishSprites[foodDishSpriteIndex];
  }
}
