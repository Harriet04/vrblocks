using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectMenu : MonoBehaviour
{
    // Buttons
    public Button SandboxButton;
    public Button UserLevelButton;
    public Button LevelButton;

    // GameObjects
    public GameObject SandboxModeView;
    public GameObject LevelModeView;
    public GameObject LevelSelector;
    public GameObject UserLevelSelector;

    //Sandbox/Level Handlers
    public level_loader LevelLoader;
    public PlacementSystem PlacementSystem;

    public float AnimSpeed = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        //Link onClick event to load sandbox mode
        SandboxButton.onClick.AddListener(OpenSandboxMenu);
        UserLevelButton.onClick.AddListener(OpenUserLevelMenu);
        LevelButton.onClick.AddListener(OpenLevelSelectMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelSelectMenu()
    {
        LevelSelector.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
        PlacementSystem.enabled = false;
        DisableMenu();
    }

    public void OpenUserLevelMenu()
    {
        UserLevelSelector.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
        UserLevelMenu UserMenu = UserLevelSelector.GetComponent<UserLevelMenu>();
        if(UserMenu!=null){
            UserMenu.UpdateMenu();
        }
        PlacementSystem.enabled = false;
        DisableMenu();
    }

    public void OpenSandboxMenu()
    {
        //Sandbox not active, play error sound
        //Perhaps a sub-menu of previously created levels. Otherwise just load the mode and close this UI
        //DisableMenu();
        LevelLoader.clearLevel();
        PlacementSystem.enabled = true;
    }


    // Useful when you need to show/hide this menu.
    // If elements are originally scaled differently, this will maintain thier original scale.
    private void SetRelativeScale(Vector3 Scale)
    {
        //Scale the game object this script is applied to
        gameObject.LeanScale(Scale, AnimSpeed).setEaseInOutCubic();
    }

    public void EnableMenu()
    {
        SetRelativeScale(Vector3.one);
    }

    public void DisableMenu()
    {
        SetRelativeScale(Vector3.zero);
    }
}
