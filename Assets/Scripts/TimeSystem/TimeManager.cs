using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonobehaviour<TimeManager>
{
    [HideInInspector]public bool isInBed = false;
    private int gameYear = 1;
    private Season gameSeason = Season.Seeds; 
    private int gameDay = 1; 
    //private int gameHour = 6;
    //private int gameMinute = 30; 
    //private int gameSecond = 0; 
    //private bool gameClockPaused = false;
    //private float gameTick = 0f;
    private string gameDayOfWeek = "Day of Sloth"; 
    //Day of Sloth is Monday, Day of Envy is tuesday, Day of Lust is Wend, Day of Wrath is Thursday, Day of Greed is Friday, Day of Gluttony is Sat, Day of Pride is Sunday 
    //Lucifer = Pride, Belphegor = Sloth, Mammon = Greed, Beelzebub = Gluttony, Statan = Wrath, Leviathan = Envy, Asmodeus  = Lust


    private void Start() 
    {
    //    StaticEventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }


    //private void Update() 
    //{

    //    UpdateGameDay();

    /*    if(!gameClockPaused)
        {
            GameTick();
        }
    */
    //}


   /* private void GameTick() 
    {

        gameTick += Time.deltaTime; 
        if(gameTick >= Settings.secondsPerGameSecond)
        {
            gameTick -= Settings.secondsPerGameSecond;

            UpdateGameSecond();
        }

    } 
    
    private void UpdateGameSecond() 
    {

     gameSecond++; 

     if(gameSecond > 59)
     {
        gameSecond = 0;
        gameMinute++;

            if(gameMinute > 59)
            {
                gameMinute = 0;
                gameHour++;
                if(gameHour > 23)
                {
                    gameHour = 0;
                    gameDay++;
                    if(gameDay > 30)
                    {
                        gameDay = 1;
                        int gs = (int)gameSeason;
                        gf++;
                        
                        gameSeason = (Season)gs;
                        if(gs > 3)
                        {
                            gs = 0;
                            gameSeason = (Season)gs;
                            
                            gameYear++; 

                            StaticEventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, //gameHour, //gameMinute, //gameSecond);
                        }
                        StaticEventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, //gameHour, //gameMinute, //gameSecond);
                    }
                    gameDayOfWeek = GetDayOfWeek();
                    StaticEventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, //gameHour, //gameMinute, //gameSecond);
                }
                //StaticEventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, //gameHour, //gameMinute, //gameSecond);
            }
            //StaticEventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason gameDay, gameDayOfWeek, //gameHour, //gameMinute, //gameSecond);
            //Debug.Log("Game Year: " + gameYear + " Game Season: " + gameSeason + " Game Day: " + gameDay + "  Game Hour: " + gameHour + "  Game Minute: " + gameMinute);
     }

     //call to advance game second event would go here if required

    }*/

    
    private void AddGameDay()
    {

    if(isInBed = true)
    {
    gameDay++;
                    if(gameDay > 30)
                    {
                        gameDay = 1;
                        int gs = (int)gameSeason;
                        gs++;
                        
                        gameSeason = (Season)gs;
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
                    isInBed = false;
    }
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
                return "";

        }

    }       

    //advance game minute
    //public void TestAdvanceGameMinute() 
    //{

        //for(int i = 0; i < 60; i++)
        //{
            //UpdateGameSecond();
        //}   

    //}

    //advance game day test
    public void TestAdvanceGameDay() 
    {
        
        //GameClock.UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek);
        AddGameDay();
        //for (int i = 0; i < 86400; i++)
        //UpdateGameSecond;

    }


}
