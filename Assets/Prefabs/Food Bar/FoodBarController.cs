using UnityEngine;

public class FoodBarController : MonoBehaviour {
  /***** Public Variables *****/
  [Header("Do Not Modify")] 
  public RectTransform mask;
  
  /***** Public Variables *****/
  private RectTransform _rectTransform;
  private float _maxHeight; // Represents the max height of the mask

  /***** Unity Methods *****/
  void Start() {
    // Get the height of the parent container
    _rectTransform = GetComponent<RectTransform>();
    _maxHeight = _rectTransform.rect.height;
    
    // Give the value from 0 to 1, mask the appropriate amount of the food bar
    MoveMask();
  }
  
  void Update() {
    // Update the Mask size in case anything has changed in value 
    MoveMask();
  }
  
  /***** Private Methods *****/
  /** Communicates with the Model to determine the size of the mask */
  void MoveMask() {
    // Get the food bar percentage from the game data.
    float foodBarPercentage = Model.Instance.gameData.foodBarPercentage;
    // Calculate the height required to mask the percentage of the food bar
    float maskHeight = (1 - foodBarPercentage) * _maxHeight;
    // Update the height for the mask
    mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maskHeight);
  }

}