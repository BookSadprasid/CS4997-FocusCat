using System;
using UnityEngine;

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

  /***** Unity Methods *****/
  void Start() {
    // Get the width of the container minus the right border width;
    _containerWidth = container.rect.width - 5;

    // Move both the know and fill
    MoveKnob();
    MoveFill();
  }
  
  void Update() {
    // Move both the know and fill
    MoveKnob();
    MoveFill();
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
