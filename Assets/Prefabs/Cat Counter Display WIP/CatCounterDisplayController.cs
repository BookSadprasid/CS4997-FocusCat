using UnityEngine;
using UnityEngine.UI;

public class CatCounterDisplayController : MonoBehaviour {
  /***** Private Variables *****/
  private Text _text;
  
  /***** Unity Methods *****/
  void Start() {
    // Get the child text component
    _text = gameObject.GetComponentInChildren<Text>();
  }
  
  // Each frame
  void Update() {
    // Update the cat counter and total cat count
    int adoptedCats        = Model.Cats().Count;
    int totalAdoptableCats = Model.Level();
    _text.text = adoptedCats + "/" + totalAdoptableCats;
  }
}
