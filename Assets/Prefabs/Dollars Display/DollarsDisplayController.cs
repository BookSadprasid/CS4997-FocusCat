using System;
using UnityEngine;
using UnityEngine.UI;

/** Controller which handles showing the users' dollars */
public class DollarsDisplayController : MonoBehaviour {
  /***** Private Variables *****/
  private Text _text;
  
  /***** Unity Methods *****/
  void Start() {
    _text = gameObject.GetComponentInChildren<Text>();
  }
  
  // Each frame
  void Update() {
    int dollars = Model.Dollars();
    _text.text = formatDollars(dollars);
  }
  
  /***** Private Methods *****/
  /** Formats the dollar amount by showing up to three numbers and a unit */
  private string formatDollars(int dollars) {
    if (dollars < 1_000) {
      return dollars + "";
    } 
    if (dollars < 1_000_000) {
      int thousands = dollars / 1_000;
      if(thousands < 10) {
        return (dollars / 1_000.00).ToString("F2") + "K";
      } 
      if (thousands < 100){
        return (dollars / 1_000.00).ToString("F1") + "K";
      }
      return dollars / 1_000 + "K";
    }
    if (dollars < 1_000_000_000) {
      int millions = dollars / 1_000_000;
      if(millions < 10) {
        return (dollars / 1_000_000.00).ToString("F2") + "M";
      } 
      if (millions < 100){
        return (dollars / 1_000_000.00).ToString("F1") + "M";
      }
      return dollars / 1_000_000 + "M";
    }

    throw new Exception("Dollars too large");
  }
}
