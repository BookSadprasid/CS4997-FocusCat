using UnityEngine;
using UnityEngine.UI;

public class HelpButtonController : MonoBehaviour {
  /********** Public Variables **********/
  [Header("Sprites")]
  public Sprite clickedSprite;
  public Sprite unclickedSprite;

  /********** Private Variables **********/
  private bool selected;
  private Image image;
  private TooltipController[] tooltips;
  
  private AudioSource audioSource;
  
  /********** Unity Methods **********/
  void Start() {
    // Find all the tooltip in the current scene
    tooltips = Resources.FindObjectsOfTypeAll<TooltipController>();

    // Get the image component
    image = GetComponent<Image>();
    
    // Get the audio source
    audioSource = GetComponent<AudioSource>();
  }
  
  /********** Public Methods **********/
  public void OnClick() {
    // Play a click sound
    audioSource.Play();

    // Toggle the selected state
    selected = !selected;
    
    // Change the image of the button
    image.sprite = selected ? clickedSprite : unclickedSprite;
    
    // If selected, show all the tooltips
    if (selected) {
      // For each tooltip, show it
      foreach (TooltipController tooltip in tooltips) {
        tooltip.Show();
      }
    }
    else {
      // For each tooltip, hide it
      foreach (TooltipController tooltip in tooltips) {
        tooltip.Hide();
      }
    }
  }
}
