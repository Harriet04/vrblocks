using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxMenu : MonoBehaviour
{
    //Buttons
    public Button ExitButton;
    public Button SaveButton;
    public Button UserLevelsButton;

    public GameObject UserLevelSelector;
    //public GameObject Savelevel;

    //handlers
    public level_loader LevelLoader;
    public PlacementSystem PlacementSystem;
    public GameObject SaveAsset;

    public GameObject SandboxSelectorMenu;
    public float AnimSpeed = 0.3f;
    void Start()
    {
        ExitButton.onClick.AddListener(ExitMenu);
        SaveButton.onClick.AddListener(SaveAssetHandle);
        UserLevelsButton.onClick.AddListener(OpenUserLevelMenu);
    }

    public void ExitMenu()
    {
        SandboxSelectorMenu.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
        PlacementSystem.enabled = false;
        DisableMenu();
    }

    public void SaveAssetHandle()
    {
        SaveAsset.SetActive(true);
        SaveAsset.SetActive(false);
        //Savelevel.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();

    }

    public void OpenUserLevelMenu()
    {
        UserLevelSelector.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
        UserLevelMenu UserMenu = UserLevelSelector.GetComponent<UserLevelMenu>();
        PlacementSystem.enabled = false;
        DisableMenu();
        print("menu disabled");
        if (UserMenu != null)
        {
            UserMenu.UpdateMenu();
        }
        
    }

    private void SetRelativeScale(Vector3 Scale)
    {
        //Scale the game object this script is applied to
        gameObject.LeanScale(Scale, AnimSpeed).setEaseInOutCubic();
    }

    public void DisableMenu()
    {
        SetRelativeScale(Vector3.zero);
    }

}
