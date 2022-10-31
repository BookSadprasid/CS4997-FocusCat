using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/** This component handles controlling the home screen */
public class HomeController : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  [Header("UI Elements")]
  public RectTransform centerDisplay;
  public RectTransform leftDisplay;
  public GameObject modal; // This is used to display the amount of points earned while offline 
  
  /***** Private Variables *****/
  private Vector2 _centerDisplayPosition; // Stores the original position of the left display.
  private Vector2 _leftDisplayPosition; // Stores the original position of the left display.
  
  private AudioSource _audioSource;

  /***** Unity Methods *****/
  void Start() {
    // Save the original position of canvas elements
    _centerDisplayPosition = centerDisplay.position;
    _leftDisplayPosition = leftDisplay.position;
    
    // Get the audio source
    _audioSource = GetComponent<AudioSource>();

    MaybeShowOfflinePointsAndDollarsModal();
  }
  
  void Update() {
    /*** Move CatCondo and Left Buttons when breathing ***/
    if (Model.Instance.gameState == Model.GameState.Breathing) {
      Vector2 newPosition = new Vector2(-3, 0);
      
      centerDisplay.transform.position = _centerDisplayPosition + newPosition;
      leftDisplay.transform.position = _leftDisplayPosition + newPosition;
    } else {
      centerDisplay.transform.position = _centerDisplayPosition;
      leftDisplay.transform.position = _leftDisplayPosition;
    }
  }
  
  /****** Public Methods *****/
  public void GoToPetShop() {
    StartCoroutine(PlayClickSoundAndGoToPetShop());
  }
  public void GoToPokdex() {
    StartCoroutine(PlayClickSoundAndGoToMeowdex());
  }
  
  // Helper 
  IEnumerator PlayClickSoundAndGoToPetShop() {
    // Play click sound
    _audioSource.Play();
    // Wait until the audio is done playing before loading the scene
    yield return new WaitWhile(() => _audioSource.isPlaying);
    // Load the scene
    SceneManager.LoadScene("Pet Shop");
  }
  
  IEnumerator PlayClickSoundAndGoToMeowdex() {
    // Play click sound
    _audioSource.Play();
    // Wait until the audio is done playing before loading the scene
    yield return new WaitWhile(() => _audioSource.isPlaying);
    // Load the scene
    SceneManager.LoadScene("Meowdex");
  }
  
  /****** Private Methods *****/
  private void MaybeShowOfflinePointsAndDollarsModal() {
    // If there are values in the `pointsSinceLastUpdate` or `dollarsSinceLastUpdate` fields, show the modal.
    if(Model.Instance.gameData.dollarsSinceLastUpdate != 0) {
      int dollars = Model.Instance.gameData.dollarsSinceLastUpdate;

      modal.SetActive(true);
      modal.GetComponentInChildren<Text>().text =
        "Congratulations! The cats helped you earn " + dollars.ToString("C0") + " while you were away.";
      
      // Close the modal in 5 seconds.
      Invoke(nameof(CloseModal), 5);
      
      // Update the game date so that we don't show this anymore
      Model.Instance.gameData.dollarsSinceLastUpdate = 0;
      Model.SaveGameData(Model.Instance.gameData);
    }
  }

  private void CloseModal() {
    modal.SetActive(false);
  }
}
