using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI_Scripts : MonoBehaviour
{
public static UI_Scripts instance;
public GameObject deathScreen;

[SerializeField] int totalKeys = 0;
[SerializeField] TextMeshProUGUI keysText;

[SerializeField] int totalCoins = 0;
[SerializeField] TextMeshProUGUI coinsText;

private void Awake() 
{
    instance = this;
}
private void Start() {
    keysText.text = totalKeys.ToString();  
    coinsText.text = totalCoins.ToString();
}

public void AddToKeys(int keysToAdd)
{
    totalKeys += 1;
    keysText.text = totalKeys.ToString(); 
}
    
    
public void RemoveKeys(int keysToRemove)
{ 
    totalKeys -= 1;
    keysText.text = totalKeys.ToString(); 
}


public void AddToCoins(int coinsToAdd)
{
    totalCoins += coinsToAdd;
    coinsText.text = totalCoins.ToString();
}   
    

public void RemoveCoins(int coinsToRemove)
{
    totalCoins -= coinsToRemove;
    coinsText.text = totalCoins.ToString();
}
  
}
