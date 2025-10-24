using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInventoryManagementSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
 
    public Image inventoryManagementSlotImage;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject greyedOutImageGO;

    [SerializeField] private MenuInventoryManagement inventoryManagement = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;
    [SerializeField] private int slotNumber = 0;

//    private Vector3 startingPosition;
    public GameObject draggedItem;
    private Canvas parentCanvas;


    private void Awake() 
    {

        parentCanvas = GetComponentInParent<Canvas>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right)
        {
        if(itemQuantity != 0)
        {
            //instantiate gameobject as dragged item
            draggedItem = Instantiate(inventoryManagement.inventoryManagementDraggedItemPrefab, inventoryManagement.transform);
            
            //get image for dragged item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventoryManagementSlotImage.sprite;
            draggedItem.transform.localScale = new Vector3(2f, 3f, 1f);
        }
        }

    }


    public void OnDrag(PointerEventData eventData)
    {

        if(eventData.button != PointerEventData.InputButton.Right)
        {
        //move game object as dragged item
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
        }

    }


    public void OnEndDrag(PointerEventData eventData)
    {

        //destroy game object as dragged item
        if(draggedItem != null)
        {
            Destroy(draggedItem);

            //object drag is over
            if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<MenuInventoryManagementSlot>() != null)
            {
                //get the slot number where the drag ended
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<MenuInventoryManagementSlot>().slotNumber;

                //swap inventory items in inventory list 
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                //destroy inventory text box
                inventoryManagement.DestroyInventoryTextBoxGameobject();
            }
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        //populate text box with item details
        if(itemQuantity != 0)
        {
            //instantiate inventory text box
            inventoryManagement.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryManagement.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            InventoryTextBoxUI inventoryTextBox = inventoryManagement.inventoryTextBoxGameobject.GetComponent<InventoryTextBoxUI>();

            //set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            //populate text box 
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            //set text box position
            if(slotNumber > 23)
            {
                inventoryManagement.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryManagement.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else 
            {            
                inventoryManagement.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryManagement.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }

    }


    public void OnPointerExit(PointerEventData eventData)
    {

        inventoryManagement.DestroyInventoryTextBoxGameobject();

    }


}
