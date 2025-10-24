using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Tooltip
    [Tooltip("Populate this with the FadeImage canvas group component in the FadeScreenUI")]
    #endregion Tooltip 
    [SerializeField] private CanvasGroup canvasGroup;

    #region Tooltip
    [Tooltip("Populate with the MessageText textmeshpro component inside the FadeScreenUI")]
    #endregion Tooltip 
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    public static GameManager Instance;
    private Player player;
    private Player playerToFind;
    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;

    private bool isFading = false; //for the overview map

    private bool _gamePaused = false;
    [SerializeField] private InventoryBarUI inventoryBarUI = null;
    [SerializeField] private MenuInventoryManagement menuInventoryManagement = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private Button[] menuButtons = null;
    public bool gamePaused { get =>  _gamePaused; set => _gamePaused = value; }



     protected override void Awake() 
    {

        base.Awake();

        pauseMenu.SetActive(false);

        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);

        Instance = this;

    } 

    void start() 
    {
        DefinePlayer();
    }


    private void Update() 
    {

        PauseGameMenu();

    }

    public void DefinePlayer() 
    {
    
    GameObject playerGameObject = GameObject.FindWithTag("Player");
    player = playerGameObject.GetComponent<Player>();
    if(playerGameObject == null)
    {
    Debug.Log("Cannot Define Player");        
    }

    }


    private void OnEnable() 
    {

        //subscribe to player being destroyed event
      //  player.destroyedEvent.OnDestroyed += Player_OnDestroyed;

        //subscribe to the points scored event
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;
        
        //subscribe to the score multiplier event 
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;

    }


    private void OnDisable() 
    {

        //unsubscribe to player being destroyed event
       // player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
        
        //unsubscribe to the points scored event
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;

        //unsubscribe to the score multiplier event 
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

    }


    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {

        previousGameState = gameState;
        gameState = GameState.gameLost;

    }


     //handle points scored event
    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {

        //increase score
        gameScore += pointsScoredArgs.points * scoreMultiplier;

        //call score changed event
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);

    }

    //Handle game state
    private void HandleGameState()
    {
        //Handle game state
        switch (gameState)
        {
            case GameState.gameStarted:
                //Play first level
                //This was if I went binding of isaac approach but went more stardew lol
                //PlayDungeonLevel(currentDungeonLevelListIndex);

                gameState = GameState.playingLevel;

                //trigger room enemies defeated since we start in the entrance where there are no enemies
                //RoomEnemiesDefeated();

                break;

            //while playing the level handle the TAB key to display the map
            case GameState.playingLevel:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    //DisplayDungeonOverViewMap();
                    //Add in other menu and add a map menu with the M key
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
            /*case GameState.dungeonOverviewMap:

                //key released
                if(Input.GetKeyUp(KeyCode.Tab))
                {
                    //clear dungeon map
                    DungeonMap.Instance.ClearDungeonOverViewMap();
                }

                break;*/ //May re add this as I do want a map but will prob rename it something else like world map

            //before the boss is engaged
            case GameState.bossStage:

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    //DisplayDungeonOverViewMap();
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
            /*case GameState.levelCompleted: 

                //display level completed text
                StartCoroutine(LevelCompleted());

                break;*/

            //handle the game being won, only trigger this once
            case GameState.gameWon: 

                if(previousGameState != GameState.gameWon)
                    StartCoroutine(GameWon()); //I will need to add a winning state

                    break;

                //handle the game being lost gamestate, only trigger it once
                case GameState.gameLost: 

                    if(previousGameState != GameState.gameLost)
                    {
                        StopAllCoroutines(); //prevent msgs if you clear the level just as getting killed
                        //StartCoroutine(GameLost()); //I will need to add a loser state
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


    void Start()
    {

        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        //set score to zero 
        gameScore = 0;

        //set the multiplier to 1
        scoreMultiplier = 1;
        start();
    }


    private void PauseGameMenu() 
    {

            if(Input.GetKeyDown(KeyCode.Escape))
            {
            if(gamePaused)
            {
                DisablePauseMenu();
            }
            else 
            {
                EnablePauseMenu();
            }
            }
          if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(gamePaused)
            {
                DisablePauseMenu();
            }
        } 
    }


    private void EnablePauseMenu()
    {

        inventoryBarUI.DestroyCurrentlyDraggedItems();

        inventoryBarUI.ClearCurrentlySelectedItems();

        gamePaused = true;
        Player.Instance.PlayerInputIsDisabled = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        //trigger garbage collector
        System.GC.Collect();

    }


    private void DisablePauseMenu()
    {
        //destroy any dragged items
        menuInventoryManagement.DestroyCurrentlyDraggedItems();

        gamePaused = false;
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1; 
        pauseMenu.SetActive(false);

    }


    //get the player
    public Player GetPlayer()
    {

        DefinePlayer();
        return playerToFind;
         
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
        //yield return StartCoroutine(DisplayMessageRoutine("So you actually were a tryhard " + GameResources.Instance.currentPlayer.playerName + ".", Color.white, 3f));

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
        //yield return StartCoroutine(DisplayMessageRoutine("Improve your tactics " + GameResources.Instance.currentPlayer.playerName + ".", Color.white, 2f));

        yield return StartCoroutine(DisplayMessageRoutine("You scored " + gameScore.ToString("###,###0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("Press SPACE or LEFTCLICK to go back to restart again.", Color.white, 0f));

        gameState = GameState.restartGame; //THIS WILL PROB BE REPLACED WITH A RESTART LEVEL / QUIT OPTION

    }


    //restart game
    private void RestartGame()
    {

        SceneManager.LoadScene("SampleScene"); //remember when I make or rename this scene to change this part

    }


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



}
