using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  /***** Public Variables *****/
  public Text titleText;
  public Button leftButton, rightButton;
  
  /***** Private Variables *****/
  private AudioSource _audioSource;

  void Start() {
    _audioSource = GetComponent<AudioSource>();   
    
    bool _isNewGame;
    
    // Check if this is a new game by either the user having no points or no cats
    if (Model.Points() == 0 || Model.Cats().Count == 0)  _isNewGame = true;
    else  _isNewGame = false;

    // Update title text
    if (_isNewGame) titleText.text = "Welcome to Focus Cat!";
    else titleText.text = "Welcome back to Focus Cat!";

    // Update the text of each button
    if (_isNewGame) {
      leftButton.GetComponentInChildren<Text>().text = "Start New Game";
      rightButton.GetComponentInChildren<Text>().text = "About Focus Cat";  
    }
    else {
      leftButton.GetComponentInChildren<Text>().text = "Continue Game";
      rightButton.GetComponentInChildren<Text>().text = "Start New Game";
    }
    
    // Update the onClick behaviours of each button
    // Left Button
    if (_isNewGame) leftButton.onClick.AddListener(StartNewGame);
    else leftButton.onClick.AddListener(() => GoTo("Home"));
    // Right Button
    // FIXME: Create About scene
    if (_isNewGame) rightButton.onClick.AddListener(() => GoTo("About"));
    else rightButton.onClick.AddListener(StartNewGame);
  }

  /***** Private Functions *****/
  private void GoTo(string sceneName) {
    StartCoroutine(PlayClickSoundAnd(
      () => {
        SceneManager.LoadScene(sceneName);
      }
    ));
  }
  
  private void StartNewGame() {
    // Reset the game data
    Model.ResetGameData();
    
    // Go to the home screen
    GoTo("Home");
  }
  
  // Helper
  IEnumerator PlayClickSoundAnd(Action action) {
    // Play click sound
    _audioSource.Play();
    // Wait until the audio is done playing before loading the scene
    yield return new WaitWhile(() => _audioSource.isPlaying);
    // Run the action
    action.Invoke();
  }
}
