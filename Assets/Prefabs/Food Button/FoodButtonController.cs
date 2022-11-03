using UnityEngine;

public class FoodButtonController : MonoBehaviour {
  /***** Public Variables *****/
  [Header("Game Objects")]
  public GameObject circle;
  public GameObject lung;
  public GameObject button;
  public GameObject breathInText;
  public GameObject breathOutText;

  /***** Private Variables *****/
  private AudioSource audioSource;
  
  /***** Unity Methods *****/
  void Start() {
    // Scale down the LungCircle component and hide it
    circle.SetActive(false);
    circle.transform.localScale = Vector3.zero;
    
    // Hide the text
    breathInText.SetActive(false);
    breathOutText.SetActive(false);
    
    // Get the AudioSource component
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {
    // While the Button is pressed
    if (button.GetComponent<ButtonController>().IsPressed()) {
      // When the Button is pressed, play the sound
      if (!audioSource.isPlaying) {  audioSource.Play(); }

      // Tell the Model that we are in a breathing state (this will deplete the food bar)
      Model.Instance.SetGameState(Model.GameState.Breathing);
      
      // And show the LungCircle component
      circle.SetActive(true);

      // If the circle is not fully scaled
      if (circle.transform.localScale.x < 1) {
        // Scale the circle up
        circle.transform.localScale += Vector3.one * Time.deltaTime;
      } 
      // If the circle is fully scaled
      else if (circle.transform.localScale.x >= 1) {
        // Set the circle scale to 1 to handle over-scaling due to timing.
        circle.transform.localScale = Vector3.one;
        // And trigger the lung to grow (the LungController is controlling the growth, this is independent from the circle animation)
        lung.GetComponent<LungController>().IsBreathing(true);
      }
    }
    // While the button is not pressed
    else {
      // Stop the sound
      audioSource.Stop();
      
      // Tell the Model that we are in a idle state (this will increase the food bar)
      Model.Instance.SetGameState(Model.GameState.Idle);
      
      // And hide the LungCircle component
      circle.SetActive(false);

      // Stop the lung from growing
      lung.GetComponent<LungController>().IsBreathing(false);

      // If the circle is still showing
      if(circle.transform.localScale.x > 0) {
        // Scale down the circle
        circle.transform.localScale -= Vector3.one * Time.deltaTime;
      }
      // Otherwise
      else {
        // Set the circle scale to 0 since under-scaling to the negatives will
        // show the circle.
        circle.transform.localScale = Vector3.zero;
      }
    }
    
    // Show the breath in text when the lung are breathing and the direction is 1
    // Only show text when breathing
    if (lung.GetComponent<LungController>().GetBreathing()) {
      // Show breath in text when the direction is 1
      if (lung.GetComponent<LungController>().GetDirection() == 1) {
        breathInText.SetActive(true);
        breathOutText.SetActive(false);
      }
      // Otherwise show breath out text
      else {
        breathInText.SetActive(false);
        breathOutText.SetActive(true);
      }
    }
    else {
      // Hide the text
      breathInText.SetActive(false);
      breathOutText.SetActive(false);
    }
  }
}
