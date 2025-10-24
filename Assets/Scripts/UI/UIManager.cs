using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
  
    private bool _MenuOn = false;
    [SerializeField] private InventoryBarUI inventoryBarUI = null;
    [SerializeField] private MenuInventoryManagement menuInventoryManagement = null;
    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject[] menuTabs = null;
    [SerializeField] private Button[] menuButtons = null;
   
    public bool MenuOn { get =>  _MenuOn; set => _MenuOn = value; }


    protected override void Awake() 
    {

        base.Awake();

        menu.SetActive(false);

    } 


    private void Update() 
    {

        Menu();

    }


    private void Menu() 
    {

        //toggle the menu if tab or P is pressed
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
        {
            if(MenuOn)
            {
                DisableMenu();
            }
            else 
            {
                EnableMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(MenuOn)
            {
                DisableMenu();
            }
        }

    }


    private void EnableMenu()
    {

        inventoryBarUI.DestroyCurrentlyDraggedItems();
        
        inventoryBarUI.ClearCurrentlySelectedItems();

        MenuOn = true;
        Player.Instance.PlayerInputIsDisabled = true;
        Time.timeScale = 0; //i think i want the moving around while in inv
        menu.SetActive(true);

        //trigger garbage collector
        System.GC.Collect();

        //highlight selected button
        HighlighButtonForSelectedTab();

    }


    private void DisableMenu()
    {
        //destroy any dragged items
        menuInventoryManagement.DestroyCurrentlyDraggedItems();

        MenuOn = false;
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1; //diabled as I wand dungeons to continue moving while in this menu
        menu.SetActive(false);

    }


    private void HighlighButtonForSelectedTab() 
    {

        for(int i = 0; i < menuTabs.Length; i++)
        {   
            if(menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }
            else 
            {
                SetButtonColorToInActive(menuButtons[i]);
            }
        }

    }


    private void SetButtonColorToActive(Button button) 
    {

        ColorBlock colors = button.colors;
        
        colors.normalColor = colors.pressedColor;

        button.colors = colors;

    }


    public void SetButtonColorToInActive(Button button)
    {

        ColorBlock colors = button.colors;

        colors.normalColor = colors.disabledColor;

        button.colors = colors;

    }


    public void SwitchMenuTab(int tabNum)
    {

        for(int i = 0; i < menuTabs.Length; i++)
        {
            if(i != tabNum)
            {
                menuTabs[i].SetActive(false);
            }
        
            else 
            {
                menuTabs[i].SetActive(true);
            }
        }
        HighlighButtonForSelectedTab();

    }


    public void QuitGame() 
    {

        Application.Quit(); 
        //to test go to build settings - add in the scenes - target platform - 64 bit or 32 (64 for me) - player settings - untick all collision except default in collision/collision2D
        // - resolution and presentation make fullscreen exclusive - untick 4:3, 5:4, 16:10, others, but keep 16:9 - other settings and changw scripting backend to L2CPP and Api Compatibility to .NET 4.x
        // - quality tab and go to Vsync and chanfe it to Every V Blank

    }
    

}
