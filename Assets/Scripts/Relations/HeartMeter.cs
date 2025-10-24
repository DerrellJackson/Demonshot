using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeartMeter : MonoBehaviour 
{
    public GameObject CharacterDetailsHolder;

    void start()
    {
        
    }


    [SerializeField] HeartMeterDetails[] characterDetails;

    [System.Serializable]  public class HeartMeterDetails 
    {

    public string characterName;
    public GameObject characterSprite;
    public int numberOfHearts; //if some characters are easy to like you
    public Sprite[] likesSprites; //the sprites that show the likes
    public Sprite[] dislikeSprites; //the sprites that show the dislikes
    public Sprite[] lovesSprites; //the sprites that show the loves
    public Sprite[] hatesSprites; //the sprites that show the hates
    public string descriptionText; //the detailed description of the character
    public int birthday;    
    public int birthMonth;
    public bool playerKnowsThemAtStart; //shows if they are known or not to the player at the beginning of the game or have to be met by them
    public bool isNotHuman;
    public bool isNotAdult;
    public bool isFemale; //if is female then can get pregnant at some point UNLESS IT IS NOT A HUMAN OR OF AGE!!!!
    }

}
