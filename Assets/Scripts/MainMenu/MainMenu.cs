using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI birthMonth;
    [SerializeField] TextMeshProUGUI monthChanger;
    [SerializeField] TextMeshProUGUI dateChanger;
    [SerializeField] TextMeshProUGUI gender;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] GameObject playerNameInputField;

    bool isMale;
    bool isFemale;
    bool isOther;

    bool isBornSpring;
    bool isBornSummer;
    bool isBornAutumn;
    bool isBornWinter;

    void Start()
    {
        gender.text = "himself.";
        birthMonth.text = "Spring,";
        monthChanger.text = "the overgrowth of";
        dateChanger.text = "on the first.";
        isMale = true;
        isBornSpring = true;
        GiveGenericName();

    }


    public void HandlePlayerNameChange()
    {

        playerName.text = playerNameInputField.GetComponent<TMP_InputField>().text;
        
        GiveGenericName();
        

    }

    public void GiveGenericName()
    {
        playerName.text = playerNameInputField.GetComponent<TMP_InputField>().text;

        if(playerName.text == "" || playerName.text == null || playerName.text == " ")
        {
            
            if(isMale && isBornSpring)
            {
                playerName.text = "Jimmy";
            }
            if(isFemale && isBornSpring)
            {
                playerName.text = "Julie";
            }
            if(isOther && isBornSpring)
            {
                playerName.text = "Jes";
            }

            if(isMale && isBornSummer)
            {
                playerName.text = "Ethan";
            }
            if(isFemale && isBornSummer)
            {
                playerName.text = "Emma";
            }
            if(isOther && isBornSummer)
            {
                playerName.text = "Eli";
            }

            if(isMale && isBornAutumn)
            {
                playerName.text = "Sunny";
            }
            if(isFemale && isBornAutumn)
            {
                playerName.text = "Sandra";
            }
            if(isOther && isBornAutumn)
            {
                playerName.text = "Salone";
            }

            if(isMale && isBornWinter)
            {
                playerName.text = "William";
            }
            if(isFemale && isBornWinter)
            {
                playerName.text = "Wanda";
            }
            if(isOther && isBornWinter)
            {
                playerName.text = "Wint";
            }

        }

    }


    public void HandleInputDataForBirthSeason(int val)
    {   
        HandlePlayerNameChange();
        
        if(val == 0)
        {
            birthMonth.text = "Spring,";
            monthChanger.text = "overgrowth of";
            isBornAutumn = false;
            isBornSpring = true;
            isBornSummer = false;
            isBornWinter = false;
            GiveGenericName();
        }
        if(val == 1)
        {
            birthMonth.text = "Summer,";
            monthChanger.text = "a heat exhausting";
            isBornAutumn = false;
            isBornSpring = false;
            isBornSummer = true;
            isBornWinter = false;
            GiveGenericName();
        }
        if(val == 2)
        {
            birthMonth.text = "Autumn,";
            monthChanger.text = "the worst times of";
            isBornAutumn = true;
            isBornSpring = false;
            isBornSummer = false;
            isBornWinter = false;
            GiveGenericName();
        }
        if(val == 3)
        {
            birthMonth.text = "Winter,";
            monthChanger.text = "a freezing hell of";
            isBornAutumn = false;
            isBornSpring = false;
            isBornSummer = false;
            isBornWinter = true;
            GiveGenericName();
        }

    }

    public void HandleInputDataForGender(int val)
    {
        HandlePlayerNameChange();
        if(val == 0)
        {
            gender.text = "himself.";
            isMale = true;
            isFemale = false;
            isOther = false;
            GiveGenericName();
        }
        if(val == 1)
        {
            gender.text = "herself.";
            isMale = false;
            isFemale = true;
            isOther = false;
            GiveGenericName();
        }
        if(val == 2)
        {
            gender.text = "themself.";
            isMale = false;
            isFemale = false;
            isOther = true;
            GiveGenericName();
        }
    }

    public void HandleInputDataForBirthDay(int val)
    {
        GiveGenericName();

        if(val == 0)
        {
            dateChanger.text = "on the first.";
        }
        if(val == 1)
        {
            dateChanger.text = "on the second";
        }
        if(val == 2)
        {
            dateChanger.text = "on the third.";
        }
        if(val == 3)
        {
            dateChanger.text = "on the fourth.";
        }
        if(val == 4)
        {
            dateChanger.text = "on the fifth.";
        }
        if(val == 5)
        {
            dateChanger.text = "on the sixth.";
        }
        if(val == 6)
        {
            dateChanger.text = "on the seventh.";
        }
        if(val == 7)
        {
            dateChanger.text = "on the eighth.";
        }
        if(val == 8)
        {
            dateChanger.text = "on the ninth.";
        }
        if(val == 9)
        {
            dateChanger.text = "on the tenth.";
        }
        if(val == 10)
        {
            dateChanger.text = "on the eleventh.";
        }
        if(val == 11)
        {
            dateChanger.text = "on the twelveth.";
        }
        if(val == 12)
        {
            dateChanger.text = "on the thirteenth.";
        }
        if(val == 13)
        {
            dateChanger.text = "on the fourteenth.";
        }
        if(val == 14)
        {
            dateChanger.text = "on the fifteenth.";
        }
        if(val == 15)
        {
            dateChanger.text = "on the sixteenth.";
        }
        if(val == 16)
        {
            dateChanger.text = "on the seventeenth.";
        }
        if(val == 17)
        {
            dateChanger.text = "on the eighteenth.";
        }
        if(val == 18)
        {
            dateChanger.text = "on the nineteenth.";
        }
        if(val == 19)
        {
            dateChanger.text = "on the twentieth.";
        }
        if(val == 20)
        {
            dateChanger.text = "on the twenty first.";
        }
        if(val == 21)
        {
            dateChanger.text = "on the twenty second.";
        }
        if(val == 22)
        {
            dateChanger.text = "on the twenty third.";
        }
        if(val == 23)
        {
            dateChanger.text = "on the twenty fourth.";
        }
        if(val == 24)
        {
            dateChanger.text = "on the twenty fifth.";
        }
        if(val == 25)
        {
            dateChanger.text = "on the twenty sixth.";
        }
        if(val == 26)
        {
            dateChanger.text = "on the twenty seventh.";
        }
        if(val == 27)
        {
            dateChanger.text = "on the twenty eighth.";
        }
        if(val == 28)
        {
            dateChanger.text = "on the twenty ninth.";
        }
        if(val == 29)
        {
            dateChanger.text = "on the thirtieth.";
        }

    }



    TextMeshProUGUI GetGender()
    {
        return gender;
    }

    TextMeshProUGUI GetBirthMonth()
    {
        return birthMonth;
    }

    TextMeshProUGUI GetPlayerName()
    {
        return playerName;
    }



}
