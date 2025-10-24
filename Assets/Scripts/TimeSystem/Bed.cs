using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : SingletonMonobehaviour<Bed>
{
    private bool canSleep = true;
    //private bool isSleeping;
    private float sleepingTime = 0.5f;
    private float sleepingCooldown = 4f;

    private int gameYear = 1;
    private Season gameSeason = Season.Seeds; 
    private int gameDay = 1; 
    [HideInInspector]public GameObject Player;

    private string gameDayOfWeek = "Day of Sloth";

    private GameObject seasonSwitcher;

    // private GameObject springFarm1;
    // private GameObject summerFarm1;
    // private GameObject autumnFarm1;
    // private GameObject winterFarm1;
    


    private void Start() 
    {

        Player = GameObject.FindWithTag("Player");

        

        //THIS IS STILL NULL FOR SOME REASON
        // springFarm1 = TileMapPrefabs.Instance.springFarmPrefab; summerFarm1 = TileMapPrefabs.Instance.summerFarmPrefab;  
        // autumnFarm1 = TileMapPrefabs.Instance.autumnFarmPrefab;  winterFarm1 = TileMapPrefabs.Instance.winterFarmPrefab;  


    }
 

    private void AddGameDay()
    {
        
        seasonSwitcher = GameObject.FindWithTag("SeasonSwitcher");
        gameDay++;
        
                    if(gameDay > 30)
                    {
                        gameDay = 1;
                        int gs = (int)gameSeason;
                        gs++;

                        PickCurrentSeason();
                    
                        gameSeason = (Season)gs;
                        //PickCurrentSeason();
                        if(gs > 3)
                        {
                            gs = 0;
                            gameSeason = (Season)gs;
                            
                            gameYear++;
                            
                            if(gameYear > 99999999)
                            gameYear = 1;

                            StaticEventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek); //gameHour, //gameMinute, //gameSecond);
                        }
                        StaticEventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek); //gameHour, //gameMinute, //gameSecond);
                    } 
                    gameDayOfWeek = GetDayOfWeek();
                    StaticEventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek); //gameHour, //gameMinute, //gameSecond);
                   
    
    }  


    private string GetDayOfWeek() 
    {
        
        int totalDays = (((int)gameSeason) * 30) + gameDay;
        int dayOfWeek = totalDays % 7;

        switch(dayOfWeek)
        {

            case 1:
                return "Day of Sloth";

            case 2: 
                return "Day of Envy";

            case 3:
                return "Day of Lust";

            case 4:
                return "Day of Wrath";

            case 5:
                return "Day of Greed";
            
            case 6:
                return "Day of Gluttony";

            case 7:
                return "Day of Pride";
            
            default: 
                return "Day of Pride";

        }

    }      


    //advance game day test
    public void TestAdvanceGameDay() 
    {
        
        //GameClock.UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek);
        AddGameDay();
        //for (int i = 0; i < 86400; i++)
        //UpdateGameSecond;

    }


    private void OnTriggerStay2D(Collider2D other) 
    {

      if(other.tag == "Player") 
      { 
        if(canSleep == true)
        {
        StartCoroutine(Sleep());
        }
      }
      

    }


    private void OnTriggerExit2D(Collider2D other)
    {
      
       if(other.tag == "Player")
      {
     //   isSleeping = false; 
      }
  
    }


    private IEnumerator Sleep() 
    {

    canSleep = false;
    //isSleeping = true;
    yield return new WaitForSeconds(sleepingTime);
    //isSleeping = false;
    AddGameDay();
    yield return new WaitForSeconds(sleepingCooldown);
    canSleep = true;

    }

     void PickCurrentSeason()
     {
            seasonSwitcher = GameObject.FindWithTag("SeasonSwitcher");
            seasonSwitcher.GetComponent<SeasonSwitch>().ChangeSeason();
        
    //         if(gameSeason == Season.Seeds)
    //         {
    //             //Spring

    //             springFarm1.SetActive(true); summerFarm1.SetActive(false); autumnFarm1.SetActive(false); winterFarm1.SetActive(false);                             
    //         }

    //         if(gameSeason == Season.Burning)
    //         {
    //             //Summer
                            
    //             springFarm1.SetActive(false); summerFarm1.SetActive(true); autumnFarm1.SetActive(false); winterFarm1.SetActive(false); 
                            
    //         }

    //         if(gameSeason == Season.Unfeeling)
    //         {
    //             //Autumn
                            
    //             springFarm1.SetActive(false); summerFarm1.SetActive(false); autumnFarm1.SetActive(true); winterFarm1.SetActive(false); 
                            
    //         }
                        
    //         if(gameSeason == Season.Cold)
    //         {
    //             //Winter
                            
    //             springFarm1.SetActive(false); summerFarm1.SetActive(false); autumnFarm1.SetActive(false); winterFarm1.SetActive(true); 
                            
    //         }
     }


}
