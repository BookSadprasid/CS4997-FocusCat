using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreController : MonoBehaviour {
    /********** Public Variables **********/
    /***** UI Elements *****/
    [Header("UI Elements")]
    public GameObject modal;

    /***** Private Variables *****/
    private IncubaborController[] incubaborControllers;
    
    /***** Unity Methods *****/
    void Start() {
        // Hide the modal
        HideModal();
        
        // Find all the IncubaborControllers in the scene
        incubaborControllers = FindObjectsOfType<IncubaborController>();

        // Each time we render this controller, generate a new set of cats for
        // each IncubaborController
        foreach (IncubaborController incubaborController in incubaborControllers) {
            // Generate a cat for adoption
            Model.Cat generatedCat = Model.Cat.CreateCat();
            // Assign the cat to the incubator
            incubaborController.cat = generatedCat;
            // Update the incubators button to handle adoting a cat. 
            incubaborController.adoptButton.onClick.AddListener(() => AdoptCat(generatedCat, incubaborController));
            
        }
    }
    
    /***** Public Methods *****/
    public void CloseButtonHandler() {  SceneManager.LoadScene("Home"); }
    
    /***** Private Methods *****/
    /** Handler when clicking on the "Adopt" button **/
    private void AdoptCat(Model.Cat cat, IncubaborController incubaborController) {
        // Given a cat
        
        // If the user has enough capacity
        if (!Model.HasEnoughSpace()) {
            ShowModal("Sorry! You don't have enough space to adopt a cat.");
            return;
        }
        
        // And if thee user has enough money
        if (!Model.HasEnoughMoney(cat)) {
            ShowModal("Sorry! You don't have enough money to adopt a cat.");
            return;
        }
        
        // Now that the user has enough money and space, we can adopt the cat
        ShowModal("Congratulations! You have adopt a cat.");
        Model.AdoptCat(cat);
        
        // Generate a new cat for adoption
        Model.Cat generatedCat = Model.Cat.CreateCat();
        // Assign the cat to the incubator
        incubaborController.cat = generatedCat;
        // Update the incubators button to handle adopting a cat.
        incubaborController.adoptButton.onClick.RemoveAllListeners();
        incubaborController.adoptButton.onClick.AddListener(() => AdoptCat(generatedCat, incubaborController));
    }
    
    private void ShowModal(string message) {
        modal.SetActive(true);
        modal.GetComponentInChildren<Text>().text = message;
        
        // Hide the modal after 5 seconds
        Invoke(nameof(HideModal), 2);
    }
    
    private void HideModal() {
        modal.SetActive(false);
    }
}
