using UnityEngine;

public class PaceBreathingBackgroundController : MonoBehaviour {
  /***** Public Variables *****/
  [Header("Configuration")]
  public float intervalSeconds = 5f;
  
  /***** Private Variables *****/
  private SpriteRenderer _spriteRenderer;
  private float _timer; // Time since the last interval
  private int _direction = 1; // Either 1 or -1 which will make the color lighter or darker
  
  void Start() {
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
    // Update timer
    _timer += Time.deltaTime;
    
    // Once we hit the interval time change direction
    if (_timer > intervalSeconds) {
      _timer -= intervalSeconds;
      _direction *= -1;
    }
    
    // Update the background color
    Color fromColor = _direction == 1 ? Color.white : Color.black;
    Color toColor   = _direction == 1 ? Color.black : Color.white;
    _spriteRenderer.color = 
      Color.Lerp(
        fromColor, 
        toColor,
        // We are getting to 100% color interpolation 1 second before the
        // interval time to give the user time to pause before transitioning.
        // Min handles when _timer (5) / intervalSeconds (5) - 1 < 1 which should not happen.
        Mathf.Min(_timer, (intervalSeconds - 1)) / (intervalSeconds - 1));
  }
}
