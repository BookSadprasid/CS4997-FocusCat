using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour {
  private void Update() {
    // Show the splash screen for a few seconds
    if(Time.timeSinceLevelLoad > 2) {
      // Then load the menu screen
      SceneManager.LoadScene("Menu");
    }
  }
}
