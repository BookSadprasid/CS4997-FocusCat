using System;
using UnityEngine;
using UnityEngine.UI;

/** Controller which handles showing the users' dollars */
public class DollarsPerSecondDisplayController : MonoBehaviour {
  /***** Private Variables *****/
  private Text _text;
  
  /***** Unity Methods *****/
  void Start() {
    _text = gameObject.GetComponentInChildren<Text>();
  }
  
  // Each frame
  void Update() {
    int dollarsPerSecond = Model.DollarsPerSecond();
    _text.text = formatDollarsPerSecond(dollarsPerSecond);
  }
  
  /***** Private Methods *****/
  /** Formats the dollar per second text to only show 3 digits*/
  private string formatDollarsPerSecond(int dollars) {
    if (dollars < 1_000) {
      return dollars + " / sec";
    } 
    if (dollars < 1_000_000) {
      int thousands = dollars / 1_000;
      if(thousands < 10) {
        return (dollars / 1_000.00).ToString("F2") + "K / sec";
      } 
      if (thousands < 100){
        return (dollars / 1_000.00).ToString("F1") + "K / sec";
      }
      return dollars / 1_000 + "K / sec";
    }
    if (dollars < 1_000_000_000) {
      int millions = dollars / 1_000_000;
      if(millions < 10) {
        return (dollars / 1_000_000.00).ToString("F2") + "M / sec";
      } 
      if (millions < 100){
        return (dollars / 1_000_000.00).ToString("F1") + "M / sec";
      }
      return dollars / 1_000_000 + "M / sec";
    }

    throw new Exception("Dollars too large");
  }
}
