using UnityEngine;
using UnityEngine.UI;

public class FoodBagController : MonoBehaviour {
  /***** Public Variables *****/
  public Sprite[] foodBagSprites;
  public float animationSpeedSeconds = 0.5f;
  
  /***** Private Variables *****/
  private Image _image;
  private int _foodBagSpriteIndex;
  private float _animationTimer;

  /***** Unity Methods *****/
  void Start() {
    // Get teh image component
    _image = GetComponent<Image>();
    
    // Set the initial food bag sprite
    _foodBagSpriteIndex = 0;
    _image.sprite = foodBagSprites[_foodBagSpriteIndex];
    
    // Set the animation timer
    _animationTimer = 0.0f;
  }
  
  void Update() {
    // Only animation when the GameState is Breathing and there is food in the food bar
    if (
      Model.Instance.gameState != Model.GameState.Breathing
      || Model.Instance.gameData.foodBarPercentage <= 0.0f
    ) {
      return;
    } 
    
    // Update animation time
    _animationTimer += Time.deltaTime;
    
    // Check if the animation timer is greater than the animation speed
    if (_animationTimer > animationSpeedSeconds) {
      // Reset the animation timer
      _animationTimer -= animationSpeedSeconds;
      
      // Increment the food bag sprite index
      _foodBagSpriteIndex++;
      
      // Check if the food bag sprite index is greater than the number of food bag sprites
      if (_foodBagSpriteIndex >= foodBagSprites.Length) {
        // Reset the food bag sprite index
        _foodBagSpriteIndex = 0;
      }
      
      // Set the food bag sprite
      _image.sprite = foodBagSprites[_foodBagSpriteIndex];
    }
  }

}
