using UnityEngine;

[System.Serializable]
public class ItemDetails 
{
 
    public int itemCode; //code used to identify item
    public ItemType itemType;
    public string itemDescription; 
    public Sprite itemSprite;
    public string itemLongDescription; //for inventory bar
    public short itemUseGridRadius; //the radius away that the item can hit from
    public float itemUserRadius; //radius away a swing can be for stuff like scythe
    public bool isStartingItem;
    public bool canBePickedUp; //used for stuff I do not want picked up
    public bool canBeDropped; //cannot drop starting gear for example
    public bool canBeEaten;
    public bool canBeCarried;
    public bool hasOwnAnimation;
    public bool isAlsoForCrafting;
    public bool MeleeWeapon;
    public bool OffHandWeapon;
    public bool MultiTool;
    public bool RawOre;
    public float HealthIncreaseAmount;
    public float HealingAmount;
    public float FoodFillAmount;
    public float EnergyFillAmount;
    public float JoyFillAmount;

    // public float StatusEffectTime;

    // //good status effects
    // public bool SpeedIncreaseSE;
    // public bool ControlWorldSpeedSE;
    // public bool HealthRegenSE;
    // public bool HalfDamageRecievedSE;
    // public bool MiningSpeedIncreaseSE;
    // public bool FishingCatchIncreaseSE;
    // public bool FlamingBulletsSE;
    // public bool DashSpeedIncreaseSE;
    // public bool DashDistanceIncreaseSE;
    // public bool InvisibleSE;
    // public bool InvinsibleSE;
    // public bool DoubleDamageOnEnemySE;
    // public bool DamageHurtsAllSE;
    // public bool ThornsSE;
    // public bool LovedSE;
    // public bool FullBellySE;
    // public bool CoffeeBeanSE;

    // //bad status effects
    // public bool SlownessSE;
    // public bool UncontrollableWorldSpeedSE;
    // public bool DarknessSE; //make visibility severely decreased
    // public bool AttackSpeedSlowSE;
    // public bool WeakDamageSE;
    // public bool BarelyAliveSE; //lower all status like joy and everything down to 20, even health
    // public bool DoubleDamageOnPlayerSE;
    // public bool InsanitySE;
    // public bool HatedSE; //villagers will not speak to you (no trades)
    //public bool EmptyFeelingSE; //hungry
    //public bool DecafSE; //tired


    ItemDetails item;

    int availability;
    [SerializeField] private float _itemPrice;
    float price { get { return _itemPrice; } set { _itemPrice = value; } }
    int quantityInTransaction;

    public ItemDetails(ItemDetails item, int availability, float price, int quantityInTransaction)
                {
                    this.item = item;
                    this.availability = availability;
                    this.price = price;
                    this.quantityInTransaction = quantityInTransaction;
                }
            public string GetName()
            {
                return item.itemDescription;
            }

            public Sprite GetSprite()
            {
                return item.itemSprite;
            }

            public int GetQuantity()
            {
                return availability;
            }

            public float GetPrice()
            {
                return price;
            }

            public ItemDetails AddItem()
            {   
                return item;
            }
    
            public int GetQuantityInTransaction()
            {
                return quantityInTransaction;
            }

            public int GetItemCode()
            {
                return itemCode;
            }

}
