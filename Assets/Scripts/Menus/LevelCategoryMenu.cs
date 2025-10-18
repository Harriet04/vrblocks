using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCategoryManager : MonoBehaviour
{
    public Button EasyButton;
    public Button MediumButton;
    public Button HardButton;
    public Button ChallengeButton;

    public GameObject LevelSelectMenu;
    public float AnimSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        EasyButton.onClick.AddListener(OpenEasyMenu);
        MediumButton.onClick.AddListener(OpenMediumMenu);
        HardButton.onClick.AddListener(OpenHardMenu);
        ChallengeButton.onClick.AddListener(OpenChallengeMenu);
    }

    void OpenLevelMenu(int StartLevel, int EndLevel)
    {
        //Get the LevelSelectorMenu component, set the min/max level
        LevelSelectorMenu LevelSelect = LevelSelectMenu.GetComponent<LevelSelectorMenu>();
        if (LevelSelect != null)
        {
            //Open the LevelSelectorMenu
            LevelSelectMenu.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
            //Getting a null error here, that's probably okay for now, we don't have any levels to load in categories
            //LevelSelect.SetMinMaxLevel(StartLevel, EndLevel);

            //Close this menu
            SetRelativeScale(Vector3.zero);
        }


    }

    void OpenEasyMenu() { OpenLevelMenu(0, 15); }
    void OpenMediumMenu() { OpenLevelMenu(16, 31); }
    void OpenHardMenu() { OpenLevelMenu(32, 47); }
    void OpenChallengeMenu() { OpenLevelMenu(48, 63); }


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
