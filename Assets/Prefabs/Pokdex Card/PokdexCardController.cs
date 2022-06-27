using UnityEngine;
using UnityEngine.UI;

public class PokdexCardController : MonoBehaviour {
    /********** Public Variables **********/
    /***** UI Elements *****/
    [Header("UI Elements")]
    public CatController catController;
    public Text specieText, rarityText, adoptedOnText;

    /***** Public Methods *****/
    /** Given a cat, show the cat details */
    public void Enable(Model.Cat cat) {
        catController.cat = cat;
        catController.gameObject.SetActive(true);
        
        specieText.text     = "Specie: " + cat.specie;
        rarityText.text     = "Rarity: " + cat.rarity;
        adoptedOnText.text  = "Adopted: " + cat.adoptedOn;
    }
    
    /** Disables the card component visually */
    public void Disable() {
        catController.gameObject.SetActive(false);
        
        specieText.text = "";
        rarityText.text = "";
        adoptedOnText.text = "";
    }
}
