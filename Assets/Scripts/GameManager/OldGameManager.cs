using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class OldGameManager : SingletonMonobehaviour<OldGameManager>
{
    #region Header GAMEOBJECT REFRENCES
    [Space(10)]
    [Header("GAMEOBJECT REFRENCES")]
    #endregion Header GAMEOBJECT REFRENCES

    #region Tooltip
    [Tooltip("Populate with the pause menu gameobject")]
    #endregion Tooltip
    [SerializeField] private GameObject pauseMenu;

    #region Tooltip
    [Tooltip("Populate with the MessageText textmeshpro component inside the FadeScreenUI")]
    #endregion Tooltip 
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    #region Tooltip
    [Tooltip("Populate this with the FadeImage canvas group component in the FadeScreenUI")]
    #endregion Tooltip 
    [SerializeField] private CanvasGroup canvasGroup;


    #region Header DUNGEON LEVELS

    [Space(10)]
    [Header("DUNGEON LEVELS")]

    #endregion Header DUNGEON LEVELS
    #region Tooltip

    [Tooltip("Populate with the dungeon level scriptable objects")]

    #endregion Tooltip

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]

    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;

    private bool isFading = false; //for the overview map

    protected override void Awake() 
    {

        //call base class
        base.Awake();

        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);

        //set player details value - saved in current player scriptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        //instantiate player
        InstantiatePlayer();

    }


    //create the player in scene at position
    private void InstantiatePlayer()
    {
        
        //instantiate the player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        //initialize player
        player = playerGameObject.GetComponent<Player>();

        //disabled for now 
      //  player.Initialize(playerDetails);

    }


    private void OnEnable() 
    {

        //subscribe to room changed event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        //subscribe to the enemies defeated in rooms event
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

        //subscribe to the points scored event
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;
        
        //subscribe to the score multiplier event 
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;

        //subscribe to player being destroyed event
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;

    }


    private void OnDisable() 
    {

        //unsubscribe to the room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        //unsubscribe to the enemies defeated in rooms event
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;

        //unsubscribe to the points scored event
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;

        //unsubscribe to the score multiplier event 
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

        //unsubscribe to player being destroyed event
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;

    }


    //handle room changed event
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {

        SetCurrentRoom(roomChangedEventArgs.room);

    }


    //handle room enemies defeated event
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {

        RoomEnemiesDefeated();

    }


    //handle points scored event
    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {

        //increase score
        gameScore += pointsScoredArgs.points * scoreMultiplier;

        //call score changed event
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);

    }


    //handle the score multiplier event
    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {

        if(multiplierArgs.multiplier)
        {
            scoreMultiplier++;
        }
        else
        {
            scoreMultiplier--;
            scoreMultiplier--;
        }

        //clamp between 1 and 20
        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30); //(value I want to clamp, lowest number, highest number)

        //call score changed event
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);

    }


    //handle the player being destroyed event
    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {

        previousGameState = gameState;
        gameState = GameState.gameLost;

    }


    // Start is called before the first frame update
    void Start()
    {

        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        //set score to zero 
        gameScore = 0;

        //set the multiplier to 1
        scoreMultiplier = 1;

        //set screen to black
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));

    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
       
        //THIS IS FOR TESTING ONLY SO SLASH IT OUT AS THIS IS JUST TO SEE THE VARIANTS OF LEVELS
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //   gameState = GameState.gameStarted; //This is also testing
        //}
    }

    //Handle game state
    private void HandleGameState()
    {
        //Handle game state
        switch (gameState)
        {
            case GameState.gameStarted:
                //Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);

                gameState = GameState.playingLevel;

                //trigger room enemies defeated since we start in the entrance where there are no enemies
                RoomEnemiesDefeated();

                break;

            //while playing the level handle the TAB key to display the map
            case GameState.playingLevel:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    DisplayDungeonOverViewMap();
                }

            break;

            //while engagin enemies handle the escape key for the pause menu
            case GameState.engagingEnemies:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                break;            

            //if in the dungeon overview map handle the release of the tab key to clear the map
            case GameState.dungeonOverviewMap:

                //key released
                if(Input.GetKeyUp(KeyCode.Tab))
                {
                    //clear dungeon map
                    DungeonMap.Instance.ClearDungeonOverViewMap();
                }

                break;

            //before the boss is engaged
            case GameState.bossStage:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    DisplayDungeonOverViewMap();
                }

                break;

            //while engaging the boss handle the escape key for the pause menu
            case GameState.engagingBoss:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                break;

            //handle the level being completed 
            case GameState.levelCompleted: 

                //display level completed text
                StartCoroutine(LevelCompleted());

                break;

            //handle the game being won, only trigger this once
            case GameState.gameWon: 

                if(previousGameState != GameState.gameWon)
                    StartCoroutine(GameWon());

                    break;

                //handle the game being lost gamestate, only trigger it once
                case GameState.gameLost: 

                    if(previousGameState != GameState.gameLost)
                    {
                        StopAllCoroutines(); //prevent msgs if you clear the level just as getting killed
                        StartCoroutine(GameLost());
                    }

                    break;

                //restart the game gamestate
                case GameState.restartGame:

                    RestartGame();

                    break;

                //if the gamestate is paused then pressing it again will unpause the game
                case GameState.gamePaused:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
        }
    }


    //set the current room the player is in
    public void SetCurrentRoom(Room room)
    {

        previousRoom = currentRoom;
        currentRoom = room;

        //debug
        //Debug.Log(room.prefab.name.ToString());

    }


    //room enemies defeated - test if all the dungeon rooms have been cleared of enemies and if so load the nect dungeon level
    private void RoomEnemiesDefeated()
    {
   
        bossRoom = null;

        //loop through all dungeon rooms to see if cleared of enemies

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            //skip boss room for now
            if(keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

        }

        //set game state, if the boss room is cleared
        if(bossRoom.room.isClearedOfEnemies) 
        {
            //are there more dungeon levels then 
            if(currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else 
            {
                gameState = GameState.gameWon;
            }
        }

    }


    //pause game menu 
    public void PauseGameMenu() 
    {

        if (gameState != GameState.gamePaused)
        {
            pauseMenu.SetActive(true);
            GetPlayer().playerControl.DisablePlayer();

            //set game state
            previousGameState = gameState;
            gameState = GameState.gamePaused;
        }
        else if (gameState == GameState.gamePaused)
        {
            pauseMenu.SetActive(false);
            GetPlayer().playerControl.EnablePlayer();

            //set game state 
            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
        }

    }


    //dungeon map screen display
    private void DisplayDungeonOverViewMap() 
    {

        //return if fading
        if(isFading)
            return;

        //display dungeon map
        DungeonMap.Instance.DisplayDungeonOverViewMap();

    }


    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {

        //build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if(!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Could not build dungeon from specified rooms and node graphs..");
        }

        //call a static event that room has changed
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        //set player roughly mid-room
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        //get nearest spawn position point in room closest to player
        //player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

        //display dungeon level text
        StartCoroutine(DisplayDungeonLevelText());

    }


    //display the dungeon level text
    private IEnumerator DisplayDungeonLevelText()
    {

        //set the screen to black
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));

        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL" + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList [currentDungeonLevelListIndex].levelName.ToUpper();

        yield return StartCoroutine(DisplayMessageRoutine(messageText, Color.white, 2f));

        GetPlayer().playerControl.EnablePlayer();

        //fade in 
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));

    }


    //display the message text for displaySeconds, if the displaySeconds it = 0 then the message is displayed until the return key is pressed
    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {

        //set text 
        messageTextTMP.SetText(text);
        messageTextTMP.color = textColor;

        //display the message for the given time
        if(displaySeconds > 0f)
        {
            float timer = displaySeconds;

            while (timer > 0f && !Input.GetKeyUp(KeyCode.Space) || timer > 0f && !Input.GetMouseButtonUp(0)) //Input.GetKey(KeyCode.Mouse0) //just if it does not work how I want
            {
                timer -= Time.deltaTime; 
                yield return null;
            }
        }
        else 
        //display the message until the button is pressed
        {
            while(!Input.GetKeyUp(KeyCode.Space) || !Input.GetMouseButtonUp(0))
            {
                yield return null;
            }
        }
        yield return null;

        //clear text
        messageTextTMP.SetText("");

    }


    //enter boss stage
    private IEnumerator BossStage()
    {

        //activate boss room
        bossRoom.gameObject.SetActive(true);

        //wait 2 seconds
        yield return new WaitForSeconds(2f);

        //fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color (0f, 0f, 0f, 0.4f)));

        //display boss message
        yield return StartCoroutine(DisplayMessageRoutine("Nice going " + GameResources.Instance.currentPlayer.playerName + ". \nAll enemies are cleared here. \nFinish the level tryhard.", Color.white, 5f));

        //fade out canvas
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color (0f, 0f, 0f, 0.4f)));

    }


    //show level as being completed and load the next level
    private IEnumerator LevelCompleted()
    {

        //play next level
        gameState = GameState.playingLevel;

        yield return new WaitForSeconds(2f);
        
        //fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        //display level completed
        yield return StartCoroutine(DisplayMessageRoutine("Another level completed." + "\nAre you a tryhard " + GameResources.Instance.currentPlayer.playerName + "?", Color.white, 5f ));

        //fade out canvas 
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        //when player presses space or left mouse go to next level
        while (!Input.GetKeyUp(KeyCode.Space) || !Input.GetMouseButtonUp(0))
        {
            yield return null;
        }

        yield return null;

        //increase index to next level
        currentDungeonLevelListIndex++;

        PlayDungeonLevel(currentDungeonLevelListIndex); //will prob remove this code and put it as ReturnToSafeZone(currentSafeZone) or something like that for trades and stuff

    }


    //fade canvas group
    public IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {

        isFading = true;

        Image image = canvasGroup.GetComponent<Image>();
        image.color = backgroundColor;

        float time = 0;

        while(time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }

        isFading = false;

    }


    //game won, call it after I make some credits or something. maybe a button to restart it. also increase difficulty new game plus OR just make a new game plus through dungeons by increasing levels
    private IEnumerator GameWon()
    {
        
        previousGameState = GameState.gameWon; //to avoid it being called more than once

        //disable player
        GetPlayer().playerControl.DisablePlayer();

        //fade out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        //display game was beaten 
        yield return StartCoroutine(DisplayMessageRoutine("So you actually were a tryhard " + GameResources.Instance.currentPlayer.playerName + ".", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("You scored " + gameScore.ToString("###,###0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("Thank you so much for playing through my game. - Luck", Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("Press SPACE or LEFT CLICK to get back to the main screen.", Color.white, 0f));

        gameState = GameState.restartGame; 

    }


    //game lost, get good noob. Will prob make it where it just restarts level until they quit to the main menu.
    private IEnumerator GameLost()
    {

        previousGameState = GameState.gameLost;

        //disable player 
        GetPlayer().playerControl.DisablePlayer();

        //wait for 1 second
        yield return new WaitForSeconds(1f);

        //fade out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        //disable enemies, this FindObjectsOfType is resource heavy which is why it is only used and end senarios
        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        //display game lost message
        yield return StartCoroutine(DisplayMessageRoutine("Improve your tactics " + GameResources.Instance.currentPlayer.playerName + ".", Color.white, 2f));

        yield return StartCoroutine(DisplayMessageRoutine("You scored " + gameScore.ToString("###,###0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("Press SPACE or LEFTCLICK to go back to restart again.", Color.white, 0f));

        gameState = GameState.restartGame; //THIS WILL PROB BE REPLACED WITH A RESTART LEVEL / QUIT OPTION

    }


    //restart game
    private void RestartGame()
    {

        SceneManager.LoadScene("SampleScene"); //remember when I make or rename this scene to change this part

    }


    //get the player
    public Player GetPlayer()
    {

        return player;

    }


    //get the player minimap icon
    public Sprite GetPlayerMinimapIcon()
    {

        return playerDetails.playerMinimapIcon;

    }


    //get current room the player is in
    public Room GetCurrentRoom()
    {

        return currentRoom;

    }


    //get current dungeon level 
    public DungeonLevelSO GetCurrentDungeonLevel() 
    {

        return dungeonLevelList[currentDungeonLevelListIndex];

    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof (pauseMenu), pauseMenu);
        HelperUtilities.ValidateCheckNullValue(this, nameof (messageTextTMP), messageTextTMP);
        HelperUtilities.ValidateCheckNullValue(this, nameof (canvasGroup), canvasGroup);

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif
    #endregion Validation 


}
