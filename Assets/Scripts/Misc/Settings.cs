using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Settings
{

    //obscuring item fading 
    public const float itemFadeInSeconds = 0.25f;
    public const float itemFadeOutSeconds = 0.35f;
    public const float itemTargetAlpha = 0.45f;

    //inventory 
    public static int playerInitialInventoryCapacity = 48;
    public static int playerMaximumInventoryCapacity = 48;

    //tilemap 
    public const float gridCellSize = 1f; //grid cell size in unity units
    public static Vector2 cursorSize = Vector2.one;

    #region UNITS
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion


    #region ROOM SETTINGS
    
    public const float fadeInTime = 0.4f; //time to fade in the room( CHANGE TO ALTER SPEED IT POPS IN AT )
    public const int maxChildCorridors = 3;//max number of child corridors leading from a room. 
    //- maximum should be 3 but is not recommended cause the dungeon can cause the building to fail since they are less likely to fit together;
    public const float doorUnlockDelay = 1f;

    #endregion


    //MAY NEED TO "//" OUT IF I WANT 360 SHOOTING
    #region ANIMATOR PARAMETERS
    //Animator parameters - player
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int dashUp = Animator.StringToHash("dashUp");
    public static int dashDown = Animator.StringToHash("dashDown");
    public static int dashRight = Animator.StringToHash("dashRight");
    public static int dashLeft =Animator.StringToHash("dashLeft");

    //farm course movement 
    public const float runningSpeed = 6f; // has no effect rn
    public const float walkingSpeed = 3f; // has no effect rn
    public static float useToolAnimationPause = 0.25f; //take out player movement script + requirement for these move scripts to work
    public static float liftToolAnimationPause = 0.4f; //take out player movement script + requirement for these move scripts to work
    public static float pickAnimatonPause = 1f; //take out player movement script + requirement for these move scripts to work
    public static float afterUseToolAnimationPause = 0.2f;
    public static float afterLiftToolAnimationPause = 0.4f;
    public static float afterPickAnimationPause = 0.2f; //take out player movement script + requirement for these move scripts to work
    
    //farm course player anims: // has no effect rn
    public static int xInput;
    public static int yInput;
    public static int isWalking;
    public static int isRunning;
    public static int toolEffect;
    public static int isUsingToolUp;
    public static int isUsingToolRight;
    public static int isUsingToolLeft;
    public static int isUsingToolDown;
    public static int isLiftingToolUp;
    public static int isLiftingToolRight;
    public static int isLiftingToolLeft;
    public static int isLiftingToolDown;
    public static int isSwingingToolUp;
    public static int isSwingingToolRight;
    public static int isSwingingToolLeft;
    public static int isSwingingToolDown;
    public static int isPickingRight;
    public static int isPickingLeft;
    public static int isPickingUp;
    public static int isPickingDown;

    public static int isWalkingDown;

    //shared anim parameters from farm course
    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;

    //items
    public const string Seed = "Seed";
    public const string Egg = "Egg";
    public const string Ore = "Ore";
    public const string RawOre = "Raw Ore";
    public const string Potion = "Potion";
    public const string Food = "Food";
    public const string Rawfood = "Raw Food";
    public const string Craftable = "Crafting Item";
    public const string Drink = "Drink";
    public const string Flower = "Flower";

    //furniture 
    public const string Furniture = "Furniture";
    public const string Decor = "Decorations";

    //tools 
    public const string HoeingTool = "Hoe";
    public const string ChoppingTool = "Axe";
    public const string BreakingTool = "Pickaxe";
    public const string ReapingTool = "Scythe";
    public const string WateringTool = "Watering Can";
    public const string CollectingTool = "Backpack";
    public const string MeleeWeapon = "Melee Weapon";
    public const string ProjectileWeapon = "Projectile Weapon";
    public const string ExplosiveWeapon = "Explosive";
    public const string HoneyTool = "Honey Tool";
    public const string FishingTool = "Fishing Tool";
    public const string MultiTool = "Multi Tool";
    

    //time system , disabled as the game will not be timed most likely
    //public const float secondsPerGameSecond = 0.012f;

    //static constuctor from farm course: 
    static Settings() 
    {
        xInput = Animator.StringToHash("xInput");
        yInput = Animator.StringToHash("yInput");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        toolEffect = Animator.StringToHash("toolEffect");
        isUsingToolDown = Animator.StringToHash("isUsingToolDown");
        isUsingToolLeft = Animator.StringToHash("isUsingToolLeft");
        isUsingToolRight = Animator.StringToHash("isUsingToolRight");
        isUsingToolUp = Animator.StringToHash("isUsingToolUp");
        isLiftingToolDown = Animator.StringToHash("isLiftingToolDown"); 
        isLiftingToolLeft = Animator.StringToHash("isLiftingToolLeft");
        isLiftingToolRight = Animator.StringToHash("isLiftingToolRight");
        isLiftingToolUp = Animator.StringToHash("isLiftingToolUp");
        isSwingingToolDown = Animator.StringToHash("isSwingingToolDown");
        isSwingingToolLeft = Animator.StringToHash("isSwingingToolLeft");
        isSwingingToolUp = Animator.StringToHash("isSwingingToolUp");
        isSwingingToolRight = Animator.StringToHash("isSwingingToolRight");
        isPickingDown = Animator.StringToHash("isPickingDown");
        isPickingLeft = Animator.StringToHash("isPickingLeft");
        isPickingRight = Animator.StringToHash("isPickingRight");
        isPickingUp = Animator.StringToHash("isPickingUp");

        //shared anim parameters from farm course
        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");

        isWalkingDown = Animator.StringToHash("isWalkingDown");

    }

    //flip tables / objects
    public static int flipUp = Animator.StringToHash("flipUp");
    public static int flipRight = Animator.StringToHash("flipRight");
    public static int flipLeft = Animator.StringToHash("flipLeft");
    public static int flipDown = Animator.StringToHash("flipDown");

    //chest open
    public static int use = Animator.StringToHash("use");
    //chest vanish
    public static int vanish = Animator.StringToHash("itemTaken");

    //Animator parameters - door
    public static int open = Animator.StringToHash("open");  

    //Animator parameters - damageable decor
    public static int destroy = Animator.StringToHash("destroy");
    public static String stateDestroyed = "Destroyed";      

    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f; //default music fade out transition (may remove)
    public const float musicFadeInTime = 0.5f; // default music fade in transition (may remove)
    #endregion

    #region FIRING CONTROL
    public const float useAimAngleDistance = 3.5f; // if the target distance is less than this then the aim angle will be used (calculated by the player), else the weapon aim angle will be used
    #endregion 

    #region ASTAR PATHFINDING PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region UI PARAMETERS
    public const float uiHeartSpacing = 20f; //teacher has it at 16, I think 20 looks better 
    public const float uiAmmoIconSpacing = 4f;
    #endregion

    #region CONTACT DAMAGE PARAMETERS
    public const float contactDamageCollisionResetDelay = 0.5f;
    #endregion

}