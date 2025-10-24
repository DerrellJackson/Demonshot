using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GridCursor gridCursor;
    public GameObject draggedItem;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private InventoryBarUI inventoryBar = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public ItemDetails itemDetails;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public int itemQuantity;
    [SerializeField] private int slotNumber = 0;


    private void Awake() 
    {

        parentCanvas = GetComponentInParent<Canvas>();

    }


    private void Start() 
    {

        mainCamera = Camera.main; 
        gridCursor = FindObjectOfType<GridCursor>();

    }


    private void ClearCursors() 
    {

        //disable cursor
        gridCursor.DisableCursor();

        //set item type to none
        gridCursor.SelectedItemType = ItemType.none;

    }


    private void OnEnable() 
    {

        //sub to keyboard event
        StaticEventHandler.InventorySlotSelectedKeyboardEvent += InventorySlotSelectedKeyboardEvent;

        //sub to remove item event
        StaticEventHandler.RemoveSelectedItemFromInventoryEvent += RemoveSelectedItemFromInventory;

        //sub to click event 
        StaticEventHandler.DropSelectedItemEvent += DropSelectedItemAtMousePosition;

        StaticEventHandler.AfterSceneLoadEvent += SceneLoaded;

    }

    private void OnDisable() 
    {

        //unsub to keyboard event
        StaticEventHandler.InventorySlotSelectedKeyboardEvent -= InventorySlotSelectedKeyboardEvent;

        //unsub to remove item event
        StaticEventHandler.RemoveSelectedItemFromInventoryEvent -= RemoveSelectedItemFromInventory;

        //unsub to click event 
        StaticEventHandler.DropSelectedItemEvent -= DropSelectedItemAtMousePosition;

        StaticEventHandler.AfterSceneLoadEvent -= SceneLoaded;

    }


    private void InventorySlotSelectedKeyboardEvent(int slotSelected)
    {

        //if slot selected is this slot
        if (slotSelected == slotNumber) 
        {
            //if inventory slot currently selected then deselect 
            if(isSelected == true) 
            {
                ClearSelectedItem();
            }
            else 
            {
                if(itemQuantity > 0) 
                {
                    SetSelectedItem();
                }
            }
        }

    }


    //sets the inventory slot item to be selected 
    private void SetSelectedItem()
    {

        //clear currently highlighted item
        inventoryBar.ClearHighlightOnInventorySlots();

        //highlight items on the inventory bar
        isSelected = true;

        //set highlighted inventory slots 
        inventoryBar.SetHighlightedInventorySlots();

        //set use radius for cursors
        gridCursor.ItemUseGridRadius = itemDetails.itemUseGridRadius;

        //if item requires a grid cursor then enable cursor 
        if(itemDetails.itemUseGridRadius > 0)
        {
            gridCursor.EnableCursor();
        }
        else 
        {
            gridCursor.DisableCursor();
        }

        //set item type
        gridCursor.SelectedItemType = itemDetails.itemType;

        //set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);


        if(itemDetails.canBeCarried == true)
        {
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else if (itemDetails.canBeCarried == false)
        {
            Player.Instance.HideCarriedItem();
        }

        if(itemDetails.MeleeWeapon == true)
        {
            Player.Instance.ShowHeldWeapon(itemDetails.itemCode);
        }
        else if (itemDetails.MeleeWeapon == false)
        {
            Player.Instance.HideHeldWeapon();
        }
        
        if(itemDetails.OffHandWeapon == true)
        {
            Player.Instance.ShowHeldWhip(itemDetails.itemCode);
        }

        if(itemDetails.MultiTool == true)
        {
            Player.Instance.ShowHeldMultiTool(itemDetails.itemCode);
        }
        


    }


    public void ClearSelectedItem() 
    {

        ClearCursors();

        Player.Instance.HideCarriedItem();
        Player.Instance.HideHeldWeapon();

        //clear currently highlighted item
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        //set no item selected in inventory
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

    }


    //drops the item if it is selected at the current mouse position, called by the drop item event
    private void DropSelectedItemAtMousePosition() 
    {
        
        if(itemDetails != null && isSelected)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //if you can drop item here, not needed anymore
            //Vector3Int gridPosition = GridPropertiesManager.Instance.grid.WorldToCell(worldPosition);
            //GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(gridPosition.x, gridPosition.y);

            //if(gridPropertyDetails != null && gridPropertyDetails.canDropItem)
            if(gridCursor.CursorPositionIsValid)
            {
            //create item from prefab at mouse position
            GameObject itemGameObject = Instantiate(itemPrefab, new Vector3 (worldPosition.x, worldPosition.y - Settings.gridCellSize/2f, worldPosition.z), Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //remove item from players inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

            //if no more of item then clear selected 
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }
        
            }

        }

    }

    private void RemoveSelectedItemFromInventory() 
    {

        if(itemDetails != null && isSelected)
        {
            int itemCode = itemDetails.itemCode;

            //remove item from players inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, itemCode);

            //if no more of item then clear selected
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, itemCode) == -1)
            {
                ClearSelectedItem();
            }
        }

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if(itemDetails != null)
        {
        if(eventData.button != PointerEventData.InputButton.Right)
        {
        //disable player input
        Player.Instance.DisablePlayerInputAndResetMovement();

        //Instantiate gameobject as dragged item
        draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);
        draggedItem.transform.localScale = new Vector3(1f, 1f, 1f);

        //get image for dragged item
        Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
        draggedItemImage.sprite = inventorySlotImage.sprite;

        SetSelectedItem();
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

            //if drag ends over inventory bar, get item drag is over and swap them
            if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotUI>() != null) 
            {
                //get slot number where the drag ended
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotUI>().slotNumber;

                //swap inventory items in inventory list
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                //destroy inventory text box
                DestroyInventoryTextBox();

                //clear selected item 
                ClearSelectedItem();
            }
            //else attempt to drop the item if it can be dropped 
            else
            {
                if(itemDetails.canBeDropped)
                {
                    //drop a full stack of items
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //store a temporary stacksize variable
                        int stackSize = itemQuantity;
 
                        for(int i = 0; i < stackSize; i++)
                        {
                            DropSelectedItemAtMousePosition();
                        }
                    }
                    else //drop one item only
                    {
                        DropSelectedItemAtMousePosition();
                    }
                }
            }

            //enable player input 
            Player.Instance.EnablePlayerInput();
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {

        //if left click 
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(eventData.button != PointerEventData.InputButton.Right)
            {
            //if inventory slot is currently selected then deselect 
            if(isSelected == true)
            {
                ClearSelectedItem();
            }
            else 
            {
                if(itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
            }
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        //populate with item details 
        if(itemQuantity != 0)
        {
            //instantiate inv text box
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            InventoryTextBoxUI inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<InventoryTextBoxUI>();

            //set the item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            //populate the text box
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            //set text box position according to inventory bar position 
            if(inventoryBar.IsInventoryBarPositionBottom)
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else 
            {
                 inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f); 
                 inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }

        }

    }


    public void OnPointerExit(PointerEventData eventData)
    {

        DestroyInventoryTextBox();

    }


    public void DestroyInventoryTextBox() 
    {

        if(inventoryBar.inventoryTextBoxGameobject != null) 
        {
            Destroy(inventoryBar.inventoryTextBoxGameobject);
        }

    }


    public void SceneLoaded() 
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }



}
