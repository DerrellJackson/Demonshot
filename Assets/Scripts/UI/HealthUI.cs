using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
//SCRIPT DISABLED AS NOT NEEDED. USE "PLAYER HEALTH" AS IT CONTAINS UI STUFF TOO
public class HealthUI : MonoBehaviour
{
    
    private List<GameObject> healthHeartsList = new List<GameObject>();


    private void OnEnable() 
    {
        
       // GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;

    }


    private void OnDisable() 
    {

       // GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;

    }


    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {

        SetHealthBar(healthEventArgs);
        
    }


    private void ClearHealthBar() 
    {

        foreach (GameObject heartIcon in healthHeartsList)
        {
            Destroy(heartIcon);
        }
        healthHeartsList.Clear();

    }


    private void SetHealthBar(HealthEventArgs healthEventArgs)
    {

        ClearHealthBar();

        //instantiate heart image prefabs
        int healthHearts = Mathf.CeilToInt(healthEventArgs.healthPercent * 100f / 20f); //this displays five hearts but I will probably make it 10 max so the 100 / 20 may be 100 / 10  or
        //a new method may need to be created to add hearts / shields
        //after messing around with this I will most likely add a shield thing below this that goes up like the ammo UI script

        for (int i = 0; i < healthHearts; i++)
        {
            //instantiate heart prefabs
            GameObject heart = Instantiate(GameResources.Instance.heartPrefab, transform);

            //position
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.uiHeartSpacing * i, 0f);

            healthHeartsList.Add(heart);
        }

    }

}
