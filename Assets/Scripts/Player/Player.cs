using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerHealth;
using static HealthSkill;
using static FurnaceFunctions;


#region REQUIRE COMPONENTS
//[RequireComponent(typeof())] (copy paste)
//[RequireComponent(typeof(HealthEvent))]
//[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(PlayerControl))]//if using the movement from this course
[RequireComponent(typeof(MovementByVelocityEvent))]//if using the movement from this course
[RequireComponent(typeof(MovementByVelocity))]//if using the movement from this course
[RequireComponent(typeof(MovementToPositionEvent))]//if using the movement dash from this course
[RequireComponent(typeof(MovementToPosition))]//if using the movement dash from this course
[RequireComponent(typeof(IdleEvent))]//if using the movement from this course
[RequireComponent(typeof(Idle))]//if using the movement from this course
[RequireComponent(typeof(AimWeaponEvent))]//if using the movement from this course
[RequireComponent(typeof(AimWeapon))]//if using the movement from this course
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(AnimatePlayer))]//if using the movement from this course
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
#endregion


//For each of these characters be sure to set the rigid body to Use Auto Mass, set angular drag and gravity scale to 0, and make collision detection continuous
//Also freeze Z position and put Interpolate to Interpolate.
//For the Polygon Collider set as trigger and drag it around the hit zone. The BoxCollider is just to keep player in bounds.
//The Player will need a HandNoWeapon, WeaponAnchorPosition and a child of that called WeaponRotationPoint
//HandNoWeapon will be Layered as PlayerWeapon with a Sprite Renderer, sorting layer is Instances.
//WeaponRotationPoint will have 3 children: 2 are Weapon and Hand. Both are Layer PlayerWeapon. Both will have a sprite renderer component with sorting layer Instances. Order: 5 for Weapon, 10 for Hand
//The weapon will need a PolygonCollider2D
//The third child is the WeaponShootPosition. It'll need to be moves slightly off the player and needs a child: WeaponEffectPosition. (AddParticleEffects)

public class Player : SingletonMonobehaviour<Player>
{
   
    [HideInInspector] public PlayerDetailsSO playerDetails;
    //[HideInInspector] public HealthEvent healthEvent;
    //[HideInInspector] public Health health; // Should not need any health requirements I made my own script
    [HideInInspector] public DestroyedEvent destroyedEvent;
    [HideInInspector] public PlayerControl playerControl;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;//if using the movement from this course
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;//if using the movement dash from this course
    [HideInInspector] public IdleEvent idleEvent;//if using the movement from this course
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;//if using the movement from this course
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    [SerializeField] private SpriteRenderer itemToShow = null;
    [SerializeField] private SpriteRenderer weaponToShow = null;

    private WaitForSeconds afterLiftToolAnimationPause;
    private WaitForSeconds liftToolAnimationPause;
    private WaitForSeconds afterPickAnimationPause;
    private WaitForSeconds pickAnimatonPause;
    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds useToolAnimationPause;
    private GridCursor gridCursor;
    private bool playerToolUseDisabled = false;

    //movement paramters
    private float xInput;
    private float yInput;
    private bool isCarrying = false; 
    private bool isIdle;
    private bool isLiftingToolDown;
    private bool isLiftingToolUp;
    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isRunning;
    private bool isUsingToolDown;
    private bool isUsingToolUp;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isSwingingToolDown;
    private bool isSwingingToolUp;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isWalking;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingLeft;
    private bool isPickingRight;
    private bool isWalkingDown;
    private bool isWalkingRight;
    private bool isWalkingLeft;
    private bool isWalkingUp;
    private bool isIdleRight;
    private bool isIdleLeft;
    private bool isIdleUp;
    private ToolEffect toolEffect = ToolEffect.none;
    private PlayerMovement playerMovement = PlayerMovement.none;

    [HideInInspector]public bool PlayerInputIsDisabled; //{ get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }
    
   // private Direction playerDirection;

    public Transform player;
    
    public static Player instance;

    private Rigidbody2D rigidbody2D;

    private float movementSpeed;

    private Camera mainCamera;

    public List<Weapon> weaponList = new List<Weapon>(); //this is for all the players current weapons

    public PlayerSkills playerSkills;
  
    protected override void Awake()
    {

        //get reference to main cam (farm course)
        mainCamera = Camera.main;

        base.Awake();
        

        rigidbody2D = GetComponent<Rigidbody2D>();

        //load components
        //healthEvent = GetComponent<HealthEvent>();
        //health = GetComponent<Health>();
        destroyedEvent = GetComponent<DestroyedEvent>();
        playerControl = GetComponent<PlayerControl>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();//if using the movement from this course
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();//if using the movement dash from this course
        idleEvent = GetComponent<IdleEvent>();//if using the movement from this course
   //dis for now     aimWeaponEvent = GetComponent<AimWeaponEvent>();//if using the movement from this course
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
   //no active weapon     setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
   //no active weapon     activeWeapon = GetComponent<ActiveWeapon>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerSkills = new PlayerSkills();

    }


    private void Start() 
    {

        gridCursor = FindObjectOfType<GridCursor>();
        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        pickAnimatonPause = new WaitForSeconds(Settings.pickAnimatonPause);
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
        afterPickAnimationPause = new WaitForSeconds(Settings.afterPickAnimationPause);

        //set the player starting health
        if (FindObjectOfType<Health>().healthBar != null)
        {
            SetPlayerHealth();
        }

        instance = this;
    }


    private void Update()
    {

        #region Player Input
        
        if(!PlayerInputIsDisabled)
        {
        ResetAnimationTriggers();

        PlayerWalkInput();

        PlayerTestInput();

        PlayerClickInput();

        GetPlayerDirection();

        GetMultiToolSwingDirection();

    //    PlayerMovementInput(); //disabled bc I like the other move input more

        //send event to any listeners for player movement input
     /*   StaticEventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, isUsingToolDown, isUsingToolLeft, isUsingToolRight, isUsingToolUp,
        isLiftingToolDown, isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isPickingDown, isPickingLeft, isPickingRight, isPickingUp,
        isSwingingToolDown, isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, false, false, false, false);
*/      
        PlayerInventorySlotKeyboardSelection();
     }
        #endregion


        PassiveSkills();

    }


    private void FixedUpdate() 
    {

  //      PlayerMovement();
 

    }


   /* private void PlayerMovement() 
    {

        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

        rigidbody2D.MovePosition(rigidbody2D.position + move);

    }
*/

    //temp test input
    private void PlayerTestInput() 
    {

        //trigger advance time
        //if(Input.GetKey(KeyCode.T))
        //{
            //TimeManager.Instance.TestAdvanceGameMinute();
        //}

        //trigger advance day
        if(Input.GetKeyDown(KeyCode.G))
        {
            Bed.Instance.TestAdvanceGameDay();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            SceneControllerManager.Instance.FadeAndLoadScene(SceneName.SpawnScene.ToString(), transform.position);
        }

    }


    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    private void GetPlayerDirection()
    {   

        RegisterMovementType(); 
        IdlePlayerDirection();
        GetMultiToolSwingDirection();
        //isIdle IS THE DEFUALT SO IT IS DOWNWARDS FACING
        if(playerMovement == PlayerMovement.none)
        {
            playerMovement = PlayerMovement.idleDown;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {   
            playerMovement = PlayerMovement.down;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {   
            playerMovement = PlayerMovement.up;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {   
            playerMovement = PlayerMovement.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {   
            playerMovement = PlayerMovement.right;
        }

    }


    private void RegisterMovementType()
    {   
        if(playerMovement == PlayerMovement.down && playerMovement != PlayerMovement.up && playerMovement != PlayerMovement.right && playerMovement != PlayerMovement.left)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isIdleUp", false);
            animator.SetBool("isIdleLeft", false);
            animator.SetBool("isIdleRight", false);
            animator.SetBool("isWalkingDown", true);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingLeft", false);
            if(playerMovement == PlayerMovement.down)
        {   
            if(Input.GetKeyUp(KeyCode.S))
                {
                    playerMovement = PlayerMovement.idleDown;
                    IdlePlayerDirection();
                }
        }
        }
        else if (playerMovement == PlayerMovement.up && playerMovement != PlayerMovement.left && playerMovement != PlayerMovement.right && playerMovement != PlayerMovement.down)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isIdleUp", false);
            animator.SetBool("isIdleLeft", false);
            animator.SetBool("isIdleRight", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", true);
            if(Input.GetKeyUp(KeyCode.W))
            {
                playerMovement = PlayerMovement.idleUp;
                IdlePlayerDirection();
            }
        }
        else if (playerMovement == PlayerMovement.left && playerMovement != PlayerMovement.up && playerMovement != PlayerMovement.right && playerMovement != PlayerMovement.down)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isIdleUp", false);
            animator.SetBool("isIdleLeft", false);
            animator.SetBool("isIdleRight", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingLeft", true);
            if(Input.GetKeyUp(KeyCode.A))
            {   
                if(playerMovement == PlayerMovement.left)
                {
                    playerMovement = PlayerMovement.idleLeft;
                    IdlePlayerDirection();
                }
            }
        }
        else if (playerMovement == PlayerMovement.right && playerMovement != PlayerMovement.up && playerMovement != PlayerMovement.down && playerMovement != PlayerMovement.left)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isIdleUp", false);
            animator.SetBool("isIdleLeft", false);
            animator.SetBool("isIdleRight", false);
            animator.SetBool("isWalkingRight", true);
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingDown", false);
            Debug.Log("Player is walking right");
            if(Input.GetKeyUp(KeyCode.D))
            {   
                if(playerMovement == PlayerMovement.right)
                {
                    playerMovement = PlayerMovement.idleRight;
                    IdlePlayerDirection();
                }
            }
        }
    }

    private void IdlePlayerDirection()
    {
            if(playerMovement == PlayerMovement.idleDown)
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalkingDown", false);
                animator.SetBool("isIdleLeft", false);
                animator.SetBool("isIdleRight", false);
            }
            else if(playerMovement == PlayerMovement.idleUp)
            {
                animator.SetBool("isIdleUp", true);
                animator.SetBool("isIdleLeft", false);
                animator.SetBool("isIdleRight", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalkingUp", false);
            }
            else if(playerMovement == PlayerMovement.idleLeft)
            {
                animator.SetBool("isIdleLeft", true);
                animator.SetBool("isIdleRight", false);
                animator.SetBool("isIdleUp", false);
                animator.SetBool("isIdle",false);
                animator.SetBool("isWalkingLeft", false);
            }
            else if(playerMovement == PlayerMovement.idleRight)
            {
                animator.SetBool("isIdleRight", true);
                animator.SetBool("isIdleLeft", false);
                animator.SetBool("isIdleUp", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalkingRight", false);
            }
    }

    [HideInInspector] public ItemDetails itemDetails;
    public void GetMultiToolSwingDirection()
    {

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) 
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            return;
        }
        else 
        {
            if (Input.GetMouseButtonDown(0) && itemDetails.MultiTool == true)
            {
                Debug.Log("Swung Multi Tool");
            }
        }

    }

    public void HideCarriedItem()
    {
        itemToShow.sprite = null;
        itemToShow.color = new Color(0f, 0f, 0f, 0f);

        isCarrying = false;
    }


    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        
        if(itemDetails != null)
        {
            itemToShow.sprite = itemDetails.itemSprite;
            itemToShow.color = new Color(1f, 1f, 1f, 1f);
            itemToShow.transform.localScale = new Vector3(-0.3f, 0.3f, 0.4f);
            /*if(itemType == itemType.drink)
            {
                itemToShow.transform.rotation
            }*/

            isCarrying = true;
        }
        else
        if(itemDetails == null)
        {
            HideCarriedItem();
        }
    }

    public void HideHeldWeapon()
    {
        weaponToShow.sprite = null;
        weaponToShow.color = new Color(0f, 0f, 0f, 0f);

        isCarrying = false;
    }



    public void ShowHeldWeapon(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(itemDetails != null)
        {
            weaponToShow.sprite = itemDetails.itemSprite;
            weaponToShow.color = new Color(1f, 1f, 1f, 1f);
            weaponToShow.transform.localScale = new Vector3(0.35f, 0.4f, 0.2f);

            isCarrying = true;
        }
    }

    public void ShowHeldWhip(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(itemDetails != null)
        {
            itemToShow.sprite = itemDetails.itemSprite;
            itemToShow.color = new Color(1f, 1f, 1f, 1f);
            itemToShow.transform.localScale = new Vector3(0.4f, 0.5f, 0.5f);

            isCarrying = true;
        }
    }

        public void ShowHeldMultiTool(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(itemDetails != null)
        {
            itemToShow.sprite = itemDetails.itemSprite;
            itemToShow.color = new Color(1f, 1f, 1f, 1f);
            itemToShow.transform.localScale = new Vector3(0.62f, 0.68f, 0.5f);

            isCarrying = true;
        }
    }
    


    //detect player keyboard input to select inventory slot 
    private void PlayerInventorySlotKeyboardSelection() 
    {

        string numSelected;

        switch (Input.inputString)
        {
            case "1": 
                numSelected = "0";
                break;
            
            case "2": 
                numSelected = "1";
                break;

            case "3": 
                numSelected = "2";
                break;

            case "4": 
                numSelected = "3";
                break;

            case "5": 
                numSelected = "4";
                break;

            case "6": 
                numSelected = "5";
                break;

            case "7": 
                numSelected = "6";
                break;

            case "8": 
                numSelected = "7";
                break;

            case "9": 
                numSelected = "8";
                break;

            case "0": 
                numSelected = "9";
                break;

            case "-": 
                numSelected = "10";
                break;

            case "=": 
                numSelected = "11";
                break;
            
            default:
                numSelected = "";
                break;
        } 

        if(numSelected != "")
        {
            StaticEventHandler.CallInventorySlotSelectedKeyboardEvent(int.Parse(numSelected));
        }

    }


    private void PlayerWalkInput() 
    {

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
        else 
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }

    }


    private void PlayerClickInput() 
    {
        if(!playerToolUseDisabled)
        {
            if(Input.GetMouseButton(0))
            {

                //get cursor grid position
                Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();

                //get player grid position
                Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();

                if(gridCursor.CursorIsEnabled)
                {
                    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
                }
            }
        }

    }


    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition) 
    {

        ResetMovement();

        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        //get grid property details at cursor position (the gridcursor validation routine ensures that grid property details are not null)
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        //get selected item details 
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if(itemDetails != null)
        {
            switch(itemDetails.itemType)
            {
                case ItemType.Seed:
                if(Input.GetMouseButtonDown(0))
                {
                    ProcessPlayerClickInputSeed(gridPropertyDetails, itemDetails);
                }
                break;

                case ItemType.Egg:
                case ItemType.Raw_food:
                case ItemType.Food:
                case ItemType.Ore:
                case ItemType.Craftable:
                case ItemType.Drink:
                case ItemType.Furniture: //NEED TO ADD A ProcessPlayerClickInputFurniture(itemDetails)
                case ItemType.Decor: //NEED TO ADD A ProcessPlayerClickInputDecor(itemDetails)
                case ItemType.Flower:
                case ItemType.Raw_Ore:
                if(Input.GetMouseButtonDown(0))
                {
                    ProcessPlayerClickInputDroppableItem(itemDetails);
                }
                break;

                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Honey_tool:
                
                case ItemType.Collecting_tool:
                ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
                break;

                case ItemType.Multi_Tool:
                break;

                case ItemType.none:
                break;

                case ItemType.count:
                break;

                default:
                break;
            }
        }

    }


    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if(cursorGridPosition.x > playerGridPosition.x)
        {
            return Vector3Int.right;
        }
        else if(cursorGridPosition.x < playerGridPosition.x)
        {
            return Vector3Int.left;
        }
        else if(cursorGridPosition.y > playerGridPosition.y)
        {
            return Vector3Int.up;
        }
        else 
        {
            return Vector3Int.down;
        }
    }


    private void ProcessPlayerClickInputSeed(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {

        if(itemDetails.canBeDropped && gridCursor.CursorPositionIsValid && gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.seedItemCode == -1)
        {
            PlantSeedAtCursor(gridPropertyDetails, itemDetails);
        }
        else if(itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            StaticEventHandler.CallDropSelectedItemEvent();
        }

    }


    private void PlantSeedAtCursor(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {

        //update grid property with seed details
        gridPropertyDetails.seedItemCode = itemDetails.itemCode;
        gridPropertyDetails.growthDays = 0;

        //display planted crop at grid property details
        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);

        //remove item from inventory 
        StaticEventHandler.CallRemoveSelectedItemFromInventoryEvent();

    }


    private void ProcessPlayerClickInputDroppableItem(ItemDetails itemDetails)
    {

        if(itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            StaticEventHandler.CallDropSelectedItemEvent();
        }

    }


    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    {

        //switch on tool
        switch(itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
            if(gridCursor.CursorPositionIsValid)
            {
                HoeGroundAtCursor(gridPropertyDetails, playerDirection);
            }
            break;

            case ItemType.Watering_tool:
            if(gridCursor.CursorPositionIsValid)
            {
                WaterGroundAtCursor(gridPropertyDetails, playerDirection);
            }
            break;

            case ItemType.Collecting_tool:
            if(gridCursor.CursorPositionIsValid)
            {
                CollectInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
            }
            break;

            default:
            break;
        }

    }


    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {

        //trigger animation
        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));

    }


    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {

        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        //set tool animation to hoe in override animation, disabled until i add animations for the character!!!
        /*toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);
*/
        if(playerDirection == Vector3Int.right)
        {
            isUsingToolRight = true;
        }
        if(playerDirection == Vector3Int.left)
        {
            isUsingToolLeft = true; 
        }
        if(playerDirection == Vector3Int.up)
        {
            isUsingToolUp = true;
        }
        if(playerDirection == Vector3Int.down) 
        {
            isUsingToolDown = true;
        }

        yield return useToolAnimationPause;

        //set grid property details for dug ground
        if(gridPropertyDetails.daysSinceDug == -1)
        {
            gridPropertyDetails.daysSinceDug = 0;
        }

        //set grid property to dug
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        //display dug grid tiles
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);

        //after animation pause 
        yield return afterUseToolAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;

    }


    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {

        //trigger animation
        StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));

    }


    private IEnumerator WaterGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {

        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        //set tool animation to watering can in override animation
       /* toolCharacterAttribute.partVariantType = PartVariantType.wateringCan;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);
        Disabled for now until scripts added for the animations*/
        //be sure to add a thing to if the watering can is empty
        toolEffect = ToolEffect.watering;

        if(playerDirection == Vector3Int.right)
        {
            isLiftingToolRight = true;
        }
        if(playerDirection == Vector3Int.left)
        {
            isLiftingToolLeft = true;
        }
        if(playerDirection == Vector3Int.up) 
        {
            isLiftingToolUp = true;
        }
        if(playerDirection == Vector3Int.down) 
        {
            isLiftingToolDown = true;
        }

        yield return liftToolAnimationPause;

        //set grid property details for watered ground 
        if(gridPropertyDetails.daysSinceWatered == -1)
        {
            gridPropertyDetails.daysSinceWatered = 0;
        }
        //set grid property to watered 
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        GridPropertiesManager.Instance.DisplayWateredGround(gridPropertyDetails);

        //after animation pause
        //yield return afterLiftToolAnimationPause;
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;

    }


    //methos process crop with equipped item in player direction
    private void ProcessCropWithEquippedItemInPLayerDirection(Vector3Int playerDirection, ItemDetails equippedItemDetails, GridPropertyDetails gridPropertyDetails)
    {
        switch (equippedItemDetails.itemType)
        {
            case ItemType.Collecting_tool:
            if(playerDirection == Vector3Int.right)
            {
                isPickingRight = true;
            }
            else if (playerDirection == Vector3Int.left)
            {
                isPickingLeft = true;
            }
            else if (playerDirection == Vector3Int.up) 
            {
                isPickingUp = true;
            }
            else if (playerDirection == Vector3Int.down) 
            {
                isPickingDown = true;
            }
            break; 

            case ItemType.none:
            break;
        }
        //get crop at cursor grid location
        Crop crop = GridPropertiesManager.Instance.GetCropObjectAtGridLocation(gridPropertyDetails);

        //execute process tool action for crop
        if(crop != null)
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.Collecting_tool:
                crop.ProcessToolAction(equippedItemDetails);
                break;
            }
        }
    }


    private void CollectInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {

        StartCoroutine(CollectInPlayerDirectionRoutine(gridPropertyDetails, equippedItemDetails, playerDirection));

    }


    private IEnumerator CollectInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {

        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        ProcessCropWithEquippedItemInPLayerDirection(playerDirection, equippedItemDetails, gridPropertyDetails);

        yield return pickAnimatonPause;

        //after anim pause
        yield return afterPickAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;

    }


    private void ResetMovement() 
    {

        //reset movement 
        xInput = 0f;
        yInput = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;

    }


    public void DisablePlayerInputAndResetMovement() 
    {

        DisablePlayerInput();
        ResetMovement();

        //send event to listeners for player movement input
     //   StaticEventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, isUsingToolDown, isUsingToolLeft, isUsingToolRight, isUsingToolUp,
       // isLiftingToolDown, isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isPickingDown, isPickingLeft, isPickingRight, isPickingUp,
       // isSwingingToolDown, isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, false, false, false, false);


    }


    public void DisablePlayerInput() 
    {

        PlayerInputIsDisabled = true;

    }


    public void EnablePlayerInput() 
    {
        
        PlayerInputIsDisabled = false;

    }


    private void ResetAnimationTriggers()
    {

        isPickingDown = false;
        isPickingLeft = false;
        isPickingRight = false; 
        isPickingUp = false;
        isUsingToolDown = false;
        isUsingToolLeft = false;
        isUsingToolRight = false;
        isUsingToolUp = false;
        isLiftingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolUp = false;
        isLiftingToolLeft = false;
        isSwingingToolDown = false;
        isSwingingToolLeft = false; 
        isSwingingToolRight = false;
        isSwingingToolUp = false; 
        toolEffect = ToolEffect.none;
        
    }


    public Vector3 GetPlayerViewportPosition() 
    {

        //vector3 viewport position for player 
        return mainCamera.WorldToViewportPoint(transform.position);

    }

    //disabled for full movement controls
    //initialize the player
    /*public void Initialize(PlayerDetailsSO playerDetails)
    {

        this.playerDetails = playerDetails;

        //create the players starting weapons
        CreatePlayerStartingWeapons();

        //set the player starting health
        SetPlayerHealth();

    }
    */
    

    /*private void OnEnable() 
    {

        //sub to the player health event
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;

    }


    private void OnDisable() 
    {

        //unsub to the player health event
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;

    }*/


    //handle health changed event
    /*private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {

        //if the player has died
        if(healthEventArgs.healthAmount <= 0f)
        {
            destroyedEvent.CallDestroyedEvent(true, 0);
        }

    }*/

    //disabled bc player now has starting items and not starting weapon
    //set the players starting weapons
    /*private void CreatePlayerStartingWeapons()
    {

        //clear list
        weaponList.Clear();

        //loop through all weapon details and populate the weapon list to the player
        foreach (WeaponDetailsSO weaponDetails in playerDetails.startingWeaponList)
        {
            //add the weapon to the player
            AddWeaponToPlayer(weaponDetails);
        }

    }
    */

    //set player health from playerDetails SO
    private void SetPlayerHealth()
    {
        playerHealth.getStartingHealth();
    }


    //return the player position
    public Vector3 GetPlayerPosition()
    {

        return transform.position;

    }


    //add a weapon to the player weapon dictionary
    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        
        //create new weapon class
        Weapon weapon = new Weapon() { weaponDetails = weaponDetails, weaponReloadTimer = 0f, weaponClipRemainingAmmo = 
        weaponDetails.weaponClipAmmoCapacity, weaponRemainingAmmo = weaponDetails.weaponAmmoCapacity, isWeaponReloading = false };

        //add the weapon to the list
        weaponList.Add(weapon);

        //set weapon position in list
        weapon.weaponListPosition = weaponList.Count;

        //set the added weapon as active
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);

        return weapon;

    }


    //returns true if the weapon is held by the player, but false if not
    public bool IsWeaponHeldByPlayer(WeaponDetailsSO weaponDetails)
    {
        foreach (Weapon weapon in weaponList)
        {
            if(weapon.weaponDetails == weaponDetails) return true;
        }

        return false;
    }


    public void GetPlayer()
    {
       this.player = GameObject.FindWithTag("Player").transform;
    }

    public void PassiveSkills()
    {
        
        if(healthSkill != null)
        {
        healthSkill.SelfHeal();
        //healthSkill.IncreasedSpeed();
        }
    }






    //COMBAT ABILITIES



    //FARMING ABILITIES


    
    //INTERACTION ABILITIES




}
