using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PokdexController : MonoBehaviour {
    /********** Public Variables **********/
    /***** UI Elements *****/
    public PokdexCardController[] pokdexCardControllers;
    public GameObject leftButton, rightButton;
    
    /***** Private Variables *****/
    private List<Model.Cat> cats;
    private int _page = 0;

    private AudioSource _audioSource;
    
    /***** Unity Methods *****/
    void Start() {
        // Get all the cats the user has adopted
        cats = Model.Cats();
        
        UpdatePokdex();
        
        UpdateButtons();
        
        _audioSource = GetComponent<AudioSource>();
    }
    
    /***** Public Methods *****/
    public void HandleOnClose() {
        StartCoroutine(PlayClickSoundAnd(() => SceneManager.LoadScene("Home")));
    }

    public void GoLeft() {
        StartCoroutine(PlayClickSoundAnd(
            () => {
                _page -= 1;
                UpdatePokdex();
                UpdateButtons();
            }
        ));
    }
    
    public void GoRight() {
        StartCoroutine(PlayClickSoundAnd(
            () => {
                _page += 1;
                UpdatePokdex();
                UpdateButtons();
            }
        ));
    }
    
    // Helper
    IEnumerator PlayClickSoundAnd(Action action) {
        // Play click sound
        _audioSource.Play();
        // Wait until the audio is done playing before loading the scene
        yield return new WaitWhile(() => _audioSource.isPlaying);
        // Run the action
        action.Invoke();
    }
    
    /***** Private Methods *****/
    private void UpdatePokdex() {
        // For each pokdex card
        for (int i = 0; i < pokdexCardControllers.Length; i++) {
            int catIndex = _page * 9 + i;

            if (catIndex < cats.Count) {
                pokdexCardControllers[i].Enable(cats[catIndex]);
            }
            else {
                pokdexCardControllers[i].Disable();
            }
        }
        
        // Show/Hide the left/right buttons
        leftButton.SetActive(CanGoLeft());
        rightButton.SetActive(CanGoRight());
    }

    private void UpdateButtons() {
        // Show/Hide the left/right buttons
        leftButton.SetActive(CanGoLeft());
        rightButton.SetActive(CanGoRight());
    }

    private bool CanGoLeft() {
        // We cannot go to the left if we are on the first page
        if  (_page == 0) return false;
        return true;
    }
    
    private bool CanGoRight() {
        // We cannot go right if there are no remaining cats unseen
        int totalCats  = cats.Count;
        int seenCats   = (_page + 1) * 9; // This represents the cats on this page and to the left
        int unseenCats = totalCats - seenCats; 
        
        if(unseenCats > 0) return true;
        return false;
    }
}
