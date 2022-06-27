using UnityEngine;
using UnityEngine.UI;

public class NumberController : MonoBehaviour {
  /***** Public Variables *****/
  public int number; // This is the value of the number to be represented.
  public Sprite[] numberSprites; // Represents the sprites for number 0-9
  public GameObject unitDigit, tenthDigit, hundredDigit; // Represents the UI Image components for the digits
  
  /***** Unity Methods *****/
  void Update() {
    // Given a number between 0 and 999
    // Set the image for the unit digit
    unitDigit.GetComponent<Image>().sprite = numberSprites[number % 10];
    
    if(number >= 10) {
      // Set the image for the tenth digit
      tenthDigit.GetComponent<Image>().sprite = numberSprites[(number / 10) % 10];
      tenthDigit.SetActive(true);
    } else {
      tenthDigit.SetActive(false);
    }

    if (number >= 100) {
      // Set the image for the hundred digit
      hundredDigit.GetComponent<Image>().sprite = numberSprites[(number / 100) % 10];
      hundredDigit.SetActive(true);
    }
    else {
      hundredDigit.SetActive(false);
    }
    
  }
}
