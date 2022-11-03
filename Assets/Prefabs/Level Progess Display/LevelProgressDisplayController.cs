using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayController : MonoBehaviour {
  /********** Public Variables **********/
  /***** UI Elements *****/
  [Header("UI Elements")] 
  public RectTransform container;
  public RectTransform knob;
  public RectTransform fill;
  
  /***** Private Variables *****/
  // The width of the container to fill.
  private float _containerWidth;
  
  private Image _fillImage;
  private Color _activeColor = new Color(140 / 255f, 199 / 255f, 191 / 255f);
  private Color _inactiveColor = new Color(193 / 255f, 67/255f, 46/255f);

  /***** Unity Methods *****/
  void Start() {
    // Get the width of the container minus the right border width;
    _containerWidth = container.rect.width - 5;
    
    // Get the fill image.
    _fillImage = fill.GetComponent<Image>();

    // Move both the know and fill
    MoveKnob();
    MoveFill();
  }
  
  void Update() {
    // Move both the know and fill
    MoveKnob();
    MoveFill();
    
    // Check if it has been over 60m since the last deep breathing session.
    DateTime todayDate                            = DateTime.Now;
    DateTime lastDeepBreathingDate                = !String.IsNullOrEmpty(Model.Instance.gameData.lastDeepBreathingAt) ? DateTime.Parse(Model.Instance.gameData.lastDeepBreathingAt) : todayDate;
    int      secondsSinceLastDeepBreathingSession = (int)todayDate.Subtract(lastDeepBreathingDate).TotalSeconds;
    
    // If it has been more than 1h since the last deep breathing session,
    // change the color of the fill to red.
    if (secondsSinceLastDeepBreathingSession > 60 * 60) {
      _fillImage.color = _inactiveColor;
    }
    else {
      _fillImage.color = _activeColor;
    }
  }
  
  /***** Private Methods *****/
  void MoveKnob() {
    // Calculate the xOffset given the borderWidth and the containerWidth.
    float x = _containerWidth * Math.Min((float) Model.Points() / Model.PointsToNextLevel(), 1);
    // Move the knob
    knob.anchoredPosition = new Vector2(x, 0);
  }
  
  void MoveFill() {
    // Since the fill is offset to include the the left border, we don't need to
    // account for it.
    float width = _containerWidth * Math.Min((float) Model.Points() / Model.PointsToNextLevel(), 1);
    
    // Set the width of the fill
    fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
  }

}
