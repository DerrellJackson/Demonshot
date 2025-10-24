using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushItemDrop : MonoBehaviour
{

    bool isEnabled = false; // change in future for it to only be used once per in game day
    float dropChance = 0; // decides the rarity of each drop
    private BushDrops dropState = BushDrops.unused;
    private GameObject bushItemGameObject;
    private BushItem bushItem; 


    public void UseDropItem()
    {

        if(!isEnabled) return;
        
        switch(dropState)
        {
            case BushDrops.unused:
                ActivateDrop();
                break;

            case BushDrops.herbItem:
                CollectHerbItem();
                break; 

            case BushDrops.leafItem: 
                CollectLeafItem();
                break;

            case BushDrops.stickItem:
                CollectStickItem();
                break; 

            case BushDrops.rareItem:
                CollectRareItem();
                break;
            
            case BushDrops.enemyPopOut:
                SpawnEnemyPopOut();
                break;

            case BushDrops.empty:
                return;
            

            default: 
                return;
        }


    }


    public void EnableBushDrop()
    {

        isEnabled = true;

    }


    private void ActivateDrop()
    {

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.bushDrop);
        UpdateDropState();

    }

    void UpdateDropState()
    {  
        
        if(dropChance <= 15f || dropChance <= 27f && dropChance > 23f)
        {
            dropState = BushDrops.stickItem;
            dropChance += Random.Range(1f, 4f);
            InstantiateStickItem();
        }

        else if (dropChance <= 20f && dropChance > 15f)
        {
            dropState = BushDrops.leafItem;
            dropChance += Random.Range(1f, 6f);
            InstantiateLeafItem();
        }
        
        else if (dropChance <= 23f && dropChance > 21f || dropChance <= 30f && dropChance > 27f)
        {
            dropState = BushDrops.herbItem;
            dropChance -= Random.Range(1f, 4f);
            InstantiateHerbItem();
        }
        
        else if (dropChance < 19f && dropChance > 21f || dropChance <= 101f && dropChance > 31f)
        {
            dropState = BushDrops.rareItem;
            dropChance = 0f;
            InstantiateRareItem();
        }
        
        else if (dropChance > 30f && dropChance < 32f || dropChance > 20f && dropChance < 22f || dropChance > 40f && dropChance < 42f)
        {
            dropState = BushDrops.enemyPopOut;
            dropChance = 100f;
            InstantiateEnemyPopOut();
        }

        else
        {
            dropState = BushDrops.empty;
        }
        
    }

    private void InstantiateItem() 
    {

        bushItemGameObject = Instantiate(GameResources.Instance.bushItemPrefab, this.transform);

        bushItem = bushItemGameObject.GetComponent<BushItem>();
    
    }

    private void InstantiateStickItem()
    {

    }

    private void InstantiateLeafItem()
    {

    }

    private void InstantiateHerbItem()
    {

    }

    private void InstantiateRareItem()
    {

    }

    private void InstantiateEnemyPopOut()
    {

    }


    private void CollectStickItem()
    {

    }

    private void CollectLeafItem()
    {

    }

    private void CollectHerbItem()
    {

    }

    private void CollectRareItem()
    {

    }

    private void SpawnEnemyPopOut()
    {

    }
    

}
