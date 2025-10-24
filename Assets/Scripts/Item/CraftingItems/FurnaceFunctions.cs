using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnaceFunctions : MonoBehaviour
{
    public InventoryManager inventoryManager;
   // private Player player;
    public static FurnaceFunctions furnaceFunctions;
    private void Awake() => furnaceFunctions = this;
    [HideInInspector] public Animator animator;
    private GameObject itemToSmelt;
    private bool playerIsHoldingRawOre = false;
    private bool isSmelting = false;
    private bool isCollecting = false;

    private float timer = 0f;
    private int itemCode;
    [HideInInspector] public ItemDetails itemDetails;


    private void Start()
    {
        animator = GetComponent<Animator>();
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
    }

    private void Update()
    {
        DetermineFurnaceState();
    }

    private void DetermineFurnaceState()
    {
        if(isCollecting == false && isSmelting == false)
        {
            PlayIdleFurnace();
        }
        if(isSmelting == true && isCollecting == false)
        {
            PlaySmeltFurnace();
        }
        if(isSmelting == false && isCollecting == true)
        {
            PlayCollectFurnace();
        }
    }


    private void PlayIdleFurnace()
    {
        animator.SetBool("f_collect", false);
        animator.SetBool("f_smelt", false);
        isCollecting = false;
        isSmelting = false;
    }

    public void PlaySmeltFurnace()
    {

        timer += Time.deltaTime;

        animator.SetBool("f_smelt", true);
        animator.SetBool("f_collect", false);
        isSmelting = true;
        if(timer >= 4f)
        {     
            isSmelting = false;
            isCollecting = true;
        
            PlayCollectFurnace();
            
        }
        
    }

    private void PlayCollectFurnace()
    {
        animator.SetBool("f_collect", true);
        animator.SetBool("f_smelt", false);
        timer = 0f;

    }

    private void GiveXpForBurning(float XpToGain)
    {
        

        
    }

    private void ReceiveOreType()
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(
        itemDetails != null && itemCode == 13000 ||
        itemDetails != null && itemCode == 13001 ||
        itemDetails != null && itemCode == 13002 ||
        itemDetails != null && itemCode == 13003 ||
        itemDetails != null && itemCode == 13004 ||
        itemDetails != null && itemCode == 13005 ||
        itemDetails != null && itemCode == 13006 ||
        itemDetails != null && itemCode == 13007 ||
        itemDetails != null && itemCode == 13008 ||
        itemDetails != null && itemCode == 13009 ||
        itemDetails != null && itemCode == 13010 )
        {
        if(itemCode == 13000)
        {
            SpawnApatite();
        }
        if(itemCode == 13001)
        {
            SpawnAqualium();
        }
        if(itemCode == 13005)
        {
            SpawnIron();
        }
        }

    }


    private void SpawnApatite()
    {
        Debug.Log("Apatite Spawned.");
    }
    private void SpawnAqualium()
    {   
        Debug.Log("Aqualium Spawned.");
    }
    private void SpawnIron()
    {
        itemCode = 10112;
    }
    private void SpawnCopper()
    {
        itemCode = 10112;
    }

    private void CheckIfPlayerIsHoldingRawOre()
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(
        itemDetails != null && itemCode == 13000 ||
        itemDetails != null && itemCode == 13001 ||
        itemDetails != null && itemCode == 13002 ||
        itemDetails != null && itemCode == 13003 ||
        itemDetails != null && itemCode == 13004 ||
        itemDetails != null && itemCode == 13005 ||
        itemDetails != null && itemCode == 13006 ||
        itemDetails != null && itemCode == 13007 ||
        itemDetails != null && itemCode == 13008 ||
        itemDetails != null && itemCode == 13009 ||
        itemDetails != null && itemCode == 13010 )
        {
            playerIsHoldingRawOre = true;
        }
        else
        {
            playerIsHoldingRawOre = false;
        }
    }


    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")   
        {
            if(Input.GetKeyDown(KeyCode.E) && isSmelting == false && isCollecting == false)
            {   
                if(itemDetails.RawOre == true)
                {
                CheckIfPlayerIsHoldingRawOre();
                GiveXpForBurning(10f);
                PlaySmeltFurnace();
                ReceiveOreType();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && isCollecting == true && isSmelting == false)
            {
                PlayIdleFurnace();
                ReceiveOreType();
                isSmelting = false;
                isCollecting = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && isCollecting == false && isSmelting == true)
            {
                return;
            }
        }

    }

}
