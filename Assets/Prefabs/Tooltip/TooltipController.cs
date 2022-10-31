using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour {
  /***** Unity Methods *****/
  void Start() {
    // Default the Tooltip to be invisible
    gameObject.SetActive(false);
  }
  
  /***** Public Methods *****/
  public void Show() {
    // Show the tooltip
    gameObject.SetActive(true);
  }
  
  public void Hide() {
    // Hide the tooltip
    gameObject.SetActive(false);
  }
}
