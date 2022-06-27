using UnityEngine;
using UnityEngine.UI;

public class IncubaborController : MonoBehaviour {
    /********** Public Variables **********/
    /***** UI Elements *****/
    [Header("UI Elements")]
    public CatController catPrefab;
    public Text speciesText, rarityText, priceText;
    public Button adoptButton;
    
    /***** Configuration *****/
    [Header("Configuration")]
    public Model.Cat cat; // Cat information for this prefab
    
    // Each frame
    void Update() {
        // Pass the cat information to the UI elements
        catPrefab.cat       = cat;
        speciesText.text    = "Species: " + cat.specie;
        rarityText.text     = "Rarity: " + cat.rarity;
        priceText.text      = "Price: $" + cat.price;
    }
}
