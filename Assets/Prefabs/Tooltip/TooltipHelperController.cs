using System;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Attach this to any UI Game Object with a Tooltip reference and it the
 * tooltip will appear when the pointer hovers over the Game Object.
 */
public class TooltipHelperController : MonoBehaviour {
  /***** Public Variables *****/
  public TooltipController tooltipController;
  
  /***** Private Variables *****/
  private EventTrigger eventTrigger;
  
  /***** Unity Methods *****/
  void Start() {
    // Create an event trigger
    eventTrigger = gameObject.AddComponent<EventTrigger>();
    
    // Create a Pointer Enter event
    EventTrigger.Entry entry = new EventTrigger.Entry();
    entry.eventID = EventTriggerType.PointerEnter;
    entry.callback.AddListener((data) => {
      tooltipController.Show();
    });
    eventTrigger.triggers.Add(entry);
    
    // Create a Pointer Exit event
    EventTrigger.Entry exit = new EventTrigger.Entry();
    exit.eventID = EventTriggerType.PointerExit;
    exit.callback.AddListener((data) => {
      tooltipController.Hide();
    });
    eventTrigger.triggers.Add(exit);
  }
}
