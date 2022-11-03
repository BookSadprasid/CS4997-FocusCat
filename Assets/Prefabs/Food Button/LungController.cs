using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LungController : MonoBehaviour {
  /***** Public Variables *****/
  /*** Controllers ***/
  [Header("Controllers")]
  public ArduinoController arduinoController;
  
  /*** Configuration ***/
  [Header("Configuration")]
  // The number of points given when following the breathing pattern per second
  public int pointsPerBreathPerSecond = 5;

  private bool _isBreathing;
  private int _direction = 1;
  
  private float _timer = 0; // used to keep track of time between point checks

  /***** Unity Methods *****/
  void Update() {
    // If the player is not breathing, reset the scale of the lungs to 1
    if (!_isBreathing) {
      transform.localScale = Vector2.one;
      return;
    }

    // Have the lungs move in and out at a constant rate 
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
    
    /*** Breathing Points Check ***/
    // Update the points timer
    _timer += Time.deltaTime;
    
    // Each second, we will check if the user is following the breathing pattern.
    if (_timer >= 1f) {
      Debug.Log("LungController: Checking if the user is following the breathing pattern");
      // Reset the timer.
      _timer = 0f;
      
      // If the lungs are expanding, and the user is breathing in, add a point
      if (_direction == 1 && arduinoController.GetBreathingState() == ArduinoController.BreathingStates.BreathingIn) {
        Debug.Log("LungController: User is following the breathing in pattern, add " + pointsPerBreathPerSecond + " points");
        Model.AddDollars(pointsPerBreathPerSecond);
      }// If the lungs are shrinking, and the user is breathing out, add a point
      if (_direction == -1 && arduinoController.GetBreathingState() == ArduinoController.BreathingStates.BreathingOut) {
        Debug.Log("LungController: User is following the breathing out pattern, add " + pointsPerBreathPerSecond + " points");
        Model.AddDollars(pointsPerBreathPerSecond);
      }
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