using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LungController : MonoBehaviour {
  /***** Private Variables *****/
  private bool _isBreathing;
  private int _direction = 1;

  /***** Unity Methods *****/
  void Update() {
    // While the player is breathing
    if (_isBreathing) {
      // Update the direction of the lung
      Model.Instance.isBreathingIn = _direction == 1;

      // If we are expanding the lung and we have hit the max scale
      if (_direction == 1 && transform.localScale.x > 2f) {
        // Change direction of the scale
        _direction = -1;
      } 
      // If we are contracting the lung and we have hit the min scale
      else if (_direction == -1 && transform.localScale.x < 1f) {
        // Change the direction of the scale
        _direction = 1;
      }
      
      // Change the scale of the lung
      transform.localScale += 
        (Vector3.one / 4) // This represents the speed
        * Time.deltaTime * _direction;
    }
    // Otherwise reset the scale of the lungs
    else {
      transform.localScale = Vector3.one;
    }
  }
  
  
  /***** Public Methods *****/
  /** Called by the FoodButtonController when the player should start breathing */
  public void IsBreathing(bool isBreathing) {
    _isBreathing = isBreathing;
  }
  
  public int GetDirection() {
    return _direction;
  }
  
  public bool GetBreathing() {
    return _isBreathing;
  }
}