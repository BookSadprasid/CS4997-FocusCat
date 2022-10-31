#if UNITY_STANDALONE

using UnityEngine;

public class PaceBreathingBackgroundController : MonoBehaviour {
  /***** Private Variables *****/
  private PaceBreathingSceneController _paceBreathingSceneController;
  private SpriteRenderer _spriteRenderer;
  private float _colorTimer; // Time since the last interval

  // Either 1 or -1 which will make the color lighter or darker
  // 1 = Breathing Out
  // -1 = Breathing In
  public int direction = 1; 
  
  void Start() {
    // Get the scene controller.
    _paceBreathingSceneController = GameObject.Find("PaceBreathingSceneController").GetComponent<PaceBreathingSceneController>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    
    // Find the camera height and width
    float height = Camera.main.orthographicSize * 2;
    float width  = height * Camera.main.aspect;

    // To make the background fit the camera, we need to scale it
    Sprite s          = _spriteRenderer.sprite;
    float  unitWidth  = s.textureRect.width / s.pixelsPerUnit;
    float  unitHeight = s.textureRect.height / s.pixelsPerUnit;
 
    transform.localScale = new Vector3(width / unitWidth, height / unitHeight);
  }

  void Update() {
    // Update timers
    _colorTimer += Time.deltaTime;

    float intervalSeconds = _paceBreathingSceneController.breathingInterval;

    /*** Color Change ***/
    // Once we hit the interval time change direction
    if (_colorTimer > intervalSeconds) {
      _colorTimer -= intervalSeconds;
      direction *= -1;
    }
    
    // Update the background color
    Color fromColor = direction == 1 ? Color.white : Color.black;
    Color toColor   = direction == 1 ? Color.black : Color.white;
    _spriteRenderer.color = 
      Color.Lerp(
        fromColor, 
        toColor,
        // We are getting to 100% color interpolation 1 second before the
        // interval time to give the user time to pause before transitioning.
        // Min handles when _timer (5) / intervalSeconds (5) - 1 < 1 which should not happen.
        Mathf.Min(_colorTimer, (intervalSeconds - 1)) / (intervalSeconds - 1));
  }
}

#endif