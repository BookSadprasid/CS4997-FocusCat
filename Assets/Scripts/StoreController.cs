using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreController : MonoBehaviour {
    /********** Public Variables **********/
    /***** UI Elements *****/
    [Header("UI Elements")]
    public GameObject modal;
    
    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip adoptSound;

    /***** Private Variables *****/
    private IncubaborController[] incubaborControllers;
    private AudioSource _audioSource;
    
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
            incubaborController.adoptButton.onClick.AddListener(() => {
                StartCoroutine(
                    PlaySoundAnd(
                        () => AdoptCat(generatedCat, incubaborController)
                    )
                );
            });
            
        }
        
        // Find the audio source
        _audioSource = GetComponent<AudioSource>();
    }
    
    /***** Public Methods *****/
    public void CloseButtonHandler() {
        _audioSource.clip = clickSound;
        StartCoroutine(PlaySoundAnd(
            () => SceneManager.LoadScene("Home")
        )); 
    }
    
    // Helper
    IEnumerator PlaySoundAnd(Action action) {
        // Play click sound
        _audioSource.Play();
        // Wait until the audio is done playing before loading the scene
        yield return new WaitWhile(() => _audioSource.isPlaying);
        // Run the action
        action.Invoke();
    }

    /***** Private Methods *****/
    /** Handler when clicking on the "Adopt" button **/
    private void AdoptCat(Model.Cat cat, IncubaborController incubaborController) {
        // Update the click sound
        _audioSource.clip = adoptSound;
        // Play the click sound
        _audioSource.Play();
        
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
