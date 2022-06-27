using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCondoController : MonoBehaviour
{
    // ******************** UI Elements *************************
    // Cat Towers
    public GameObject catTower1, catTower2, catTower3, catTower4, catTower5;
    private GameObject[] _catCondo;
    // Sleeping spots on Cat Tower 1
    public SpriteRenderer sleepingSpot1, sleepingSpot2, sleepingSpot3, sleepingSpot4;
    // Sleeping spots on Cat Tower 2
    public SpriteRenderer sleepingSpot5, sleepingSpot6, sleepingSpot7, sleepingSpot8;
    // Sleeping spots on Cat Tower 3
    public SpriteRenderer sleepingSpot9, sleepingSpot10, sleepingSpot11, sleepingSpot12;
    // Sleeping spots on Cat Tower 4
    public SpriteRenderer sleepingSpot13, sleepingSpot14, sleepingSpot15, sleepingSpot16;
    // Sleeping spots on Cat Tower 5
    public SpriteRenderer sleepingSpot17, sleepingSpot18, sleepingSpot19, sleepingSpot20;
    private SpriteRenderer[] _sleepingSpots;


    public GameObject cat1, cat2, cat3, cat4, cat5, cat6;
    private GameObject[] _cats;

    // Start is called before the first frame update
    void Start()
    {
        _catCondo = new[] {catTower1, catTower2, catTower3, catTower4, catTower5};
        _sleepingSpots = new[]
        {
            sleepingSpot1, sleepingSpot2, sleepingSpot3, sleepingSpot4, sleepingSpot5, sleepingSpot6, sleepingSpot7,
            sleepingSpot8, sleepingSpot9, sleepingSpot10, sleepingSpot11, sleepingSpot12, sleepingSpot13,
            sleepingSpot14, sleepingSpot15, sleepingSpot16, sleepingSpot17, sleepingSpot18, sleepingSpot19,
            sleepingSpot20
        };

        _cats = new[] {cat1, cat2, cat3, cat4, cat5, cat6};

        // Start with setting all sleeping spot to be inactive
        // SetAllSleepingSpotInactive();

        // SetSleepingSpotsActiveBasedOnLevels();
        
        // Populate the cats
        setCatsInActive();
        setCatsActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Set sleeping spot with this number active
    public void SetThisSleepingSpotActive(int num)
    {
        _sleepingSpots[num].gameObject.SetActive(true);
    }
    
    // Set sleeping up to this number active
    public void SetSleepingSpotsActiveBasedOnLevels()
    {
        for (int i = 0; i < Model.Level(); i++)
        {
            SetThisSleepingSpotActive(i);
        }
    }

    public void setCatsActive() {
        List<Model.Cat> catsInGame = Model.Cats();

        for(int i = 0; i < Math.Min(Model.Cats().Count, _cats.Length); i++) {
            _cats[i].SetActive(true);
            _cats[i].GetComponent<CatController>().cat = catsInGame[i];
            _cats[i].GetComponent<CatController>().showDollars = true;
        }    
    }

    public void setCatsInActive()
    {
        foreach (GameObject cat in _cats)
        {
            cat.SetActive(false);
        }
    }

    // Set this sleeping spot inactive
    public void SetThisSleepingSpotInActive(int num)
    {
        _sleepingSpots[num-1].gameObject.SetActive(false);
    }
    
    // Set all sleeping spot inactive
    public void SetAllSleepingSpotInactive()
    {
        foreach (SpriteRenderer s in _sleepingSpots) 
        {
            s.gameObject.SetActive(false);
        }
    }
}
