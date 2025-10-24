using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Bed;

public class SeasonSwitch : MonoBehaviour
{

    public GameObject springStartFarm;
    public GameObject summerStartFarm;
    public GameObject autumnStartFarm;
    public GameObject winterStartFarm;

    private Season season = Season.Seeds;

    int gameSeason = 0;


    void Start()
    {

        DecideSeason();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            ChangeSeason();
        }
    }


    public void ChangeSeason()
    {
        gameSeason++;
        
        FindCurrentSeason();

        DecideSeason();

        if(gameSeason > 3)
        {
            gameSeason = 0;

            FindCurrentSeason();

            DecideSeason();
        }


    }

    void FindCurrentSeason()
    {
        if(gameSeason == 0)
        {
            season = Season.Seeds;
        }
        if(gameSeason == 1)
        {
            season = Season.Burning;
        }
        if(gameSeason == 2)
        {
            season = Season.Unfeeling;
        }
        if(gameSeason == 3)
        {
            season = Season.Cold;
        }
    }


    void DecideSeason()
    {
        
        if(season == Season.Seeds)
        {
            if(springStartFarm != null) springStartFarm.SetActive(true); if(summerStartFarm != null) summerStartFarm.SetActive(false);   
            if(autumnStartFarm != null) autumnStartFarm.SetActive(false); if(winterStartFarm != null) winterStartFarm.SetActive(false);
            
            Debug.Log("Scene 1 Spring");
        }
        if(season == Season.Burning)
        {
            if(springStartFarm != null) springStartFarm.SetActive(false); if(summerStartFarm != null) summerStartFarm.SetActive(true);   
            if(autumnStartFarm != null) autumnStartFarm.SetActive(false); if(winterStartFarm != null) winterStartFarm.SetActive(false);
            
            Debug.Log("Scene 1 Summer");
        }
        if(season == Season.Unfeeling)
        {
            if(springStartFarm != null) springStartFarm.SetActive(false); if(summerStartFarm != null) summerStartFarm.SetActive(false);   
            if(autumnStartFarm != null) autumnStartFarm.SetActive(true); if(winterStartFarm != null) winterStartFarm.SetActive(false);
            Debug.Log("Scene 1 Autumn");      
        }
        if(season == Season.Cold)
        {
            if(springStartFarm != null) springStartFarm.SetActive(false); if(summerStartFarm != null) summerStartFarm.SetActive(false);   
            if(autumnStartFarm != null) autumnStartFarm.SetActive(false); if(winterStartFarm != null) winterStartFarm.SetActive(true);
            Debug.Log("Scene 1 Winter");
        }
    }

}
