using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana_UI : MonoBehaviour
{
    public Mana mana;

    public Image ManaCircle1;
    public Image ManaCircle2;
    public Image ManaCircle3;

    public GameObject ManaContainer1;
    public GameObject ManaContainer2;
    public GameObject ManaContainer3;

    bool canActivate3 = false;
    
    private float mana_r = 0f;
    public float mana_rt = 2f;

    public float mana_gain_amount = 10f;
    public float mana_drain_test = 20f;



    void Start()
    {   
        //make only the single working mana container work at start
        ManaContainer1.SetActive(true);
        ManaContainer2.SetActive(false);
        ManaContainer3.SetActive(false);

        mana.current_mana = 50f;
        mana.mana_max = 100f;

        mana_r = 0f;
        mana_rt = 2f;

    }

    public void Update()
    {   
        mana_r += Time.deltaTime;
        AddManaConatinerTest();
        DrainLog(mana_drain_test);

        if(mana_r >= mana_rt)
        {
            RegenMana(mana_gain_amount);

            Debug.Log("Current Mana is at: " + mana.current_mana);
            mana_r = 0f;
        }
        DisplayMana();
        
        if(mana.current_mana < 0)
        {
            mana.current_mana = 0;
            ManaCircle1.fillAmount = 0f;
            Debug.Log("Out Of Mana");
        }
       
    }


    void AddManaConatinerTest()
    {
        if(Input.GetKeyDown(KeyCode.X) && canActivate3 == true)
        {
            ManaContainer3.SetActive(true);
            ManaCircle3.fillAmount = 1f;
            mana.mana_max = 300f;
            mana.current_mana = 300f;
            ManaCircle1.fillAmount = 1f;
            ManaCircle2.fillAmount = 1f;
        }
        else if(Input.GetKeyDown(KeyCode.X) && canActivate3 == false)
        {
            ManaContainer2.SetActive(true);
            ManaCircle2.fillAmount = 1f;
            mana.mana_max = 200f;
            mana.current_mana = 200f;
            ManaCircle1.fillAmount = 1f;
            
            canActivate3 = true;  
        }

    }

    public void CheckManaContainer()
    {
        if(mana.mana_max > 200f)
        {
            ManaContainer3.SetActive(true);
            ManaCircle1.fillAmount = 1f;
            ManaCircle2.fillAmount = 1f;
        }
        else if (mana.mana_max > 100f && mana.mana_max <= 200f)
        {
            ManaContainer2.SetActive(true);
            ManaCircle1.fillAmount = 1f;
        }

    }

    void DrainLog(float manaAmount)
    {
        
        if(Input.GetMouseButtonDown(1))
        {
            DrainMana(manaAmount);
            Debug.Log("Current Mana: " + (mana.current_mana - manaAmount));
        }
           
        //CheckManaContainer();

    }

    void DisplayMana()
    {
        if(mana.mana_max <= 100f)
        {
            ManaCircle1.fillAmount = (mana.current_mana * 0.01f);
        }
        if(mana.mana_max <= 200f && mana.mana_max > 100f)
        {
            if(mana.current_mana > 100f)
            {
            ManaCircle2.fillAmount = (mana.current_mana * 0.01f - 1f);
            }
            else
            {
            ManaCircle1.fillAmount = (mana.current_mana * 0.01f);
            }
        }
        if(mana.mana_max <= 300f && mana.mana_max > 200f)
        {        
            if(mana.current_mana >= 200f)
            {
            ManaCircle3.fillAmount = (mana.current_mana * 0.01f - 2f);
            }    
            if(mana.current_mana < 200f && mana.current_mana >= 100f)
            {
            ManaCircle2.fillAmount = (mana.current_mana * 0.01f - 1f);
            }
            else
            {
            ManaCircle1.fillAmount = (mana.current_mana * 0.01f);
            }
        }
    }

    void DrainMana(float manaAmount)
    {

        canDrain(manaAmount);
        
    }

    void RegenMana(float manaAmount)
    {
        
        canRegen(manaAmount);
    

    }

    bool canDrain(float manaAmount)
    {
        if(mana.current_mana != 0f)
        {
            mana.current_mana -= manaAmount;
           // ManaCircle1.fillAmount = (mana.current_mana * 0.01f);
            return true;
        }
        else 
        return false;
    }


    bool canRegen(float manaAmount)
    {
        
            if(mana.current_mana < mana.mana_max)
            {
            mana.current_mana += manaAmount;
            
            return true;
            }
            else
            return false;

    }

}
