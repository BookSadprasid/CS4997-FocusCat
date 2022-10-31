#if UNITY_STANDALONE

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaceBreathingSceneController : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  [Header("UI Elements")]
  public Text timerText;
  
  /***** Controllers *****/
  [Header("Controllers")]
  public ArduinoController arduinoController;
  public PaceBreathingBackgroundController paceBreathingBackgroundController;
  
  /***** Configuration *****/
  [Header("Configuration")]
  // This is used by the paceBreathingBackgroundController to determine the speed of the background color change.
  public float breathingInterval = 10f;
  public int pointsPerBreath = 10; // The number of points given when following the breathing pattern. This is given per second.
  
  /***** Private Variables *****/
  private float breathingTimer; // Used to keep track of the time between breaths.
  private int breathingPoints; //  Points earned for following the breathing pattern.

  /***** Unity Methods *****/
  void Update() {
    float timeSinceLevelLoad = Time.timeSinceLevelLoad;
    timerText.text = formatTime(timeSinceLevelLoad);
    
    /*** Breathing Check ***/
    breathingTimer += Time.deltaTime;
    
    // Each second, we will check if the user is following the breathing pattern.
    if (breathingTimer >= 1f) {
      // Reset the timer.
      breathingTimer = 0f;
      
      // If the user is breathing in at this point, the direction should be -1
      if (arduinoController.GetBreathingState() == ArduinoController.BreathingStates.BreathingIn && paceBreathingBackgroundController.direction == -1) {
        // Then give the user a point
        breathingPoints += pointsPerBreath;
      } 
      // If the user is breathing out at this point, the direction should be 1
      else if(arduinoController.GetBreathingState() == ArduinoController.BreathingStates.BreathingOut && paceBreathingBackgroundController.direction == 1) {
        // Then give the user a point
        breathingPoints += pointsPerBreath;
      }
      else {
        // Otherwise no points 
      }
    }
  }
  
  /***** Public Methods *****/
  public void GoHome() {
    // FIXME: Remove
    Debug.Log("Received: " + breathingPoints + "points for correctly breathing");
    // Update the user points for correctly breathing
    Model.AddPoints(breathingPoints);
    // Go to the home scene
    SceneManager.LoadScene("Home");
  }
  
  /***** Private Methods *****/
  private string formatTime(float seconds) {
    string secondString = ((int) (seconds % 60)).ToString("D2");
    string minuteString = ((int) (seconds / 60)).ToString("D2");
    return minuteString + ":" + secondString;
  } 
}

#endif
