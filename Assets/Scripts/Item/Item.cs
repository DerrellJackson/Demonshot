using UnityEngine;

public class Item : MonoBehaviour
{

    [ItemCodeDescription] [SerializeField] private int _itemCode;
    [SerializeField] private float _itemPrice;

    private SpriteRenderer spriteRenderer;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    public float ItemPrice { get { return _itemPrice; } set { _itemPrice = value; } }

    private void Awake() 
    {

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }


    private void Start() 
    {

        if(ItemCode != 0)
        {
            Init(ItemCode);
        }

    }


    public void Init (int itemCodeParam)
    {
        if(itemCodeParam != 0)
        {

            ItemCode = itemCodeParam;

            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);

            spriteRenderer.sprite = itemDetails.itemSprite;

            //if item type is reapable then add the nudge component
            if(itemDetails.itemType == ItemType.Reapable_scenary)
            {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }

        public float GetPrice ()
    {
        return ItemPrice;
    }

}
