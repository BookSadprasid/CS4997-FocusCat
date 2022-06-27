using UnityEngine;

public class ButtonController : MonoBehaviour {
  /***** Private Variables *****/
  // Represents when the button is pressed
  private bool _isPressed;
  
  /***** Unity Events *****/
  void OnMouseDown() {
    _isPressed = true;
  }
  
  void OnMouseUp() {
    _isPressed = false;
  }
  
  /***** Public Methods *****/
  /** Called by FoodButtonController to check if the button is pressed */
  public bool IsPressed() {
    return _isPressed;
  }
}