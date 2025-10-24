using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isDashing, bool isIdle, bool isCarrying, ToolEffect toolEffect, 
bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown, 
bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolDown, bool isSwingingToolUp,
bool idleUp, bool idleDown, bool idleLeft, bool idleRight); 

public static class StaticEventHandler
{
    
    //drop selected item event 
    public static event Action DropSelectedItemEvent;

    public static void CallDropSelectedItemEvent() 
    {

        if(DropSelectedItemEvent != null)
        DropSelectedItemEvent();

    }


    //remove selected item from the inventory 
    public static event Action RemoveSelectedItemFromInventoryEvent;

    public static void CallRemoveSelectedItemFromInventoryEvent()
    {

        if(RemoveSelectedItemFromInventoryEvent != null)
        RemoveSelectedItemFromInventoryEvent();

    }


    //inventory updated event 
    public static event Action<InventoryLocation, Dictionary<int, InventoryItem>> InventoryUpdatedEvent;

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, Dictionary<int, InventoryItem> inventoryDict)
    {
        if(InventoryUpdatedEvent != null)
            InventoryUpdatedEvent(inventoryLocation, inventoryDict);
    }

    //ui slot selection keyboard event
    public static event Action<int> InventorySlotSelectedKeyboardEvent;

    public static void CallInventorySlotSelectedKeyboardEvent(int slotNum)
    {

        if(InventorySlotSelectedKeyboardEvent != null)
        InventorySlotSelectedKeyboardEvent(slotNum);

    }

    //movement event
    public static event  MovementDelegate MovementEvent;

    //movement event call for publishers
    public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isDashing, bool isIdle, bool isCarrying, ToolEffect toolEffect, 
bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown, 
bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolDown, bool isSwingingToolUp,
bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
{
    if(MovementEvent != null)
    MovementEvent(inputX, inputY, isWalking, isRunning, isDashing, isIdle, isCarrying, toolEffect, isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
    isLiftingToolDown, isLiftingToolUp, isLiftingToolRight, isLiftingToolLeft, isPickingDown, isPickingLeft, isPickingRight, isPickingUp,
    isSwingingToolDown, isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, idleDown, idleUp, idleLeft, idleRight);
}


    //time events but disabled as I do not plan on using a timer, if you want it back be sure to add int, int, int in the none blocked out "<int, Season, int, string, int, int, int>"
    //advance game minute
    //public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    //public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    //{
        
        //if(AdvanceGameMinuteEvent != null)
        //{
                //AdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
        //}
    
    //}


     //time events but disabled as I do not plan on using a timer
    //advance game hour
    //public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;

    //public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    //{
        
        //if(AdvanceGameHourEvent != null)
        //{
                //AdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
        //}
    
    //}

    public static event Action<int, Season, int, string> AdvanceGameDayEvent;

    //advance game day
    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek)
    {

        if (AdvanceGameDayEvent != null)
        {
            AdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek);
        }

    }


    //advance game season
    public static event Action<int, Season, int, string> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek)
    {

        if (AdvanceGameSeasonEvent != null)
        {
            AdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek);
        }

    }


    //advance game year
    public static event Action<int, Season, int, string> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek)
    {

        if (AdvanceGameYearEvent != null)
        {
            AdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek);
        }

    }


    //scene load events in the order that they happen
    //before scene unload fade out event
    public static event Action BeforeSceneUnloadFadeOutEvent;

    public static void CallBeforeSceneUnloadFadeOutEvent() 
    {

        if(BeforeSceneUnloadFadeOutEvent != null)
        {
            BeforeSceneUnloadFadeOutEvent();
        }

    }


    //before scene unload
    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {

        if(BeforeSceneUnloadEvent != null)
        {
            BeforeSceneUnloadEvent();
        }

    }


    //after scene loaded event
    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent()
    {

        if (AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
        
    }


    //after scene load fade in event 
    public static event Action AfterSceneLoadFadeInEvent;

    public static void CallAfterSceneLoadFadeInEvent() 
    {

        if (AfterSceneLoadFadeInEvent != null)
        {
            AfterSceneLoadFadeInEvent();
        }

    }


    //no longer part of farm course downwards.
    //room changed event
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs() { room = room });
    }

    //room enemies defeated event
    public static event Action<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated;

    public static void CallRoomEnemiesDefeatedEvent(Room room)
    {
        OnRoomEnemiesDefeated?.Invoke(new RoomEnemiesDefeatedArgs() { room = room });
    }

    //points scored event
    public static event Action<PointsScoredArgs> OnPointsScored;

    public static void CallPointsScoredEvent(int points)
    {

        OnPointsScored?.Invoke(new PointsScoredArgs() { points = points });

    }

    //score chabged event 
    public static event Action<ScoreChangedArgs> OnScoreChanged; 

    public static void CallScoreChangedEvent(long score, int multiplier) 
    {

        OnScoreChanged?.Invoke (new ScoreChangedArgs() { score = score, multiplier = multiplier });

    }

    //multiplier event 
    public static event Action<MultiplierArgs> OnMultiplier;

    public static void CallMultiplierEvent(bool multiplier)
    {

        OnMultiplier?.Invoke(new MultiplierArgs() { multiplier = multiplier });

    }

}


public class RoomChangedEventArgs : EventArgs 
{

    public Room room;

}


public class RoomEnemiesDefeatedArgs : EventArgs 
{

    public Room room;

}


public class PointsScoredArgs : EventArgs 
{

    public int points;

}


public class ScoreChangedArgs : EventArgs 
{

    public long score;
    public int multiplier;

}


public class MultiplierArgs : EventArgs 
{

    public bool multiplier;

}
