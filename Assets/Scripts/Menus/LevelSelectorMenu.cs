/*
 Level Selector + animations
*/
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(LevelSelectorMenu))]
public class LevelSelectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelSelectorMenu LevelSelector = (LevelSelectorMenu)target;

        if (GUILayout.Button("Propagate Dev Levels"))
        {
           AutoFillLevels(LevelSelector);
        }
    }
    private void AutoFillLevels(LevelSelectorMenu menu)
    {
        string[] guids = AssetDatabase.FindAssets(
            "t:MapBlockScriptableObject",
            new[] { menu.devLevelsPath }
        );

        menu.levelData.Clear();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var level = AssetDatabase.LoadAssetAtPath<MapBlockScriptableObject>(path);
            if (level != null)
                menu.levelData.Add(level);
        }

        // Mark dirty so Unity saves the change
        EditorUtility.SetDirty(menu);
    }
}

public class LevelSelectorMenu : MonoBehaviour
{
    public textChanger textChanger;
    public Button playLevelButton;
    public Button leftNavigateButton;
    public Button rightNavigateButton;
    public Button lockedLevelButton;

    public GameObject level1_1Tutorial;
    public GameObject level1_2Tutorial;
    public GameObject level1_3Tutorial;
    public GameObject level1_4Tutorial;
    public GameObject level1_5Tutorial;
    public GameObject level2_1Tutorial;
    public GameObject level2_2Tutorial;
    public GameObject level2_3Tutorial;

    public GameObject middleLevelView;
    public GameObject leftLevelView;
    public GameObject rightLevelView;
    public level_loader LevelLoader;
    public bool LoadIntoCurrentScene = false;
    public string devLevelsPath = "Assets/Map/MapLayouts";
    public List<MapBlockScriptableObject> levelData;

    private int selectedLevelIndex = SceneTransitionStates.GetSelectedLevel();
    private int MinLevel = 0;
    private int MaxLevel = 16;
    private Sprite[] levelThumbnails;
    private string[] levelTitles;

    // Animations
    private readonly float levelNavAnimationSpeed = 0.1f;
    private Vector3 middleLevelViewPos;
    private Vector3 leftLevelViewPos;
    private Vector3 rightLevelViewPos;
    private Quaternion middleLevelViewRot;
    private Quaternion leftLevelViewRot;
    private Quaternion rightLevelViewRot;
    private Vector2 middleLevelViewScale;

    private Vector2 leftLevelViewScale;
    private Vector2 rightLevelViewScale;

    void Awake()
    {
        // Animations
        // Note: Keeping this here to fix scaling bugs when returning to the level selector menu from a level since Start() is run each time.
        middleLevelViewPos = middleLevelView.transform.position;
        middleLevelViewRot = middleLevelView.transform.localRotation;
        middleLevelViewScale = middleLevelView.transform.localScale;
        leftLevelViewPos = leftLevelView.transform.position;
        leftLevelViewRot = leftLevelView.transform.localRotation;
        leftLevelViewScale = leftLevelView.transform.localScale;
        rightLevelViewPos = rightLevelView.transform.position;
        rightLevelViewRot = rightLevelView.transform.localRotation;
        rightLevelViewScale = rightLevelView.transform.localScale;
    }

    void Start()
    {
      
        LevelMetadataScriptableObject[] levelMetadataScriptables = GameObject.Find("/LevelStatesManager").GetComponent<LevelStatesManager>().levelMetadataScriptables;
        levelThumbnails = new Sprite[levelMetadataScriptables.Length];
        levelTitles = new string[levelMetadataScriptables.Length];
        for(int i = 0; i < levelMetadataScriptables.Length; i++)
        {
            levelThumbnails[i] = levelMetadataScriptables[i].levelThumbnail;
        }
        for(int i = 0; i < levelMetadataScriptables.Length; i++)
        {
            levelTitles[i] = levelMetadataScriptables[i].displayName;
        }

        playLevelButton.onClick.AddListener(GoToLevel);
        leftNavigateButton.onClick.AddListener(() =>
        {
            selectedLevelIndex = Math.Max(0, selectedLevelIndex - 1);
            UpdateDisplayView();
            AnimateNavigateLeft();
        });
        rightNavigateButton.onClick.AddListener(() =>
        {
            selectedLevelIndex = Math.Min(selectedLevelIndex + 1, levelThumbnails.Length - 1);
            UpdateDisplayView();
            AnimateNavigateRight();
        });

        UpdateDisplayView();
    }

    private void Update()
    {
        string path = "Assets/Map/SandboxLevels/";
        var level = AssetDatabase.LoadAssetAtPath<MapBlockScriptableObject>(path);
        if (level != null)
            levelData.Add(level);
    }

    public void SetMinMaxLevel(int minLevel, int maxLevel)
    {
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        selectedLevelIndex = minLevel;
        UpdateDisplayView();
    }

    public void chooseHintText()
    {
        switch (selectedLevelIndex)
        {
            case 0:
                textChanger.hintText.text = "Hint 1-1";
                level1_1Tutorial.SetActive(true);
                break;
            case 1:
                textChanger.hintText.text = "Hint 1-2";
                level1_2Tutorial.SetActive(true);
                break;
            case 2:
                textChanger.hintText.text = "Hint 1-3";
                level1_3Tutorial.SetActive(true);
                break;
            case 3:
                textChanger.hintText.text = "Hint 1-4";
                level1_4Tutorial.SetActive(true);
                break;
            case 4:
                textChanger.hintText.text = "Hint 1-5";
                level1_5Tutorial.SetActive(true);
                break;
            case 5:
                textChanger.hintText.text = "Hint 2-1";
                level2_1Tutorial.SetActive(true);
                break;
            case 6:
                textChanger.hintText.text = "Hint 2-2";
                level2_2Tutorial.SetActive(true);
                break;
            case 7:
                textChanger.hintText.text = "Hint 2-3";
                level2_3Tutorial.SetActive(true);
                break;
            case 8:
                textChanger.hintText.text = "Hint 2-4";
                break;
            case 9:
                textChanger.hintText.text = "Hint 2-5";
                break;
            case 10:
                textChanger.hintText.text = "Hint 3-1";
                break;
            case 11:
                textChanger.hintText.text = "Hint 3-2";
                break;
            case 12:
                textChanger.hintText.text = "Hint 3-3";
                break;
            case 13:
                textChanger.hintText.text = "Hint 3-4";
                break;
            case 14:
                textChanger.hintText.text = "Hint 3-5";
                break;
            default:
                break;
        }
        
    }
    public void GoToLevel()
    {
        //Load into the levelLoader
        if (LoadIntoCurrentScene)
        {
            if(levelData.Count>selectedLevelIndex)
            {
                chooseHintText();
                LevelLoader.LoadLevel(levelData[selectedLevelIndex]);
            }
        }
        else
        {
            SceneTransitionManager.singleton.GoToSceneAsync(selectedLevelIndex, LoadSceneBy.LevelStatesManagerArrayOrder);
            // SceneTransitionManager.singleton.GoToSceneAsync(selectedLevelIndex, LoadSceneBy.BuildSettingsOrder);
            SceneTransitionStates.SetSelectedLevel(selectedLevelIndex);
        }
    }

    public void AnimateNavigateLeft()
    {
        middleLevelView.transform.position = leftLevelViewPos;
        middleLevelView.transform.rotation = leftLevelViewRot;
        middleLevelView.transform.localScale = leftLevelViewScale;
        middleLevelView.LeanMove(middleLevelViewPos, levelNavAnimationSpeed).setEaseOutCubic();
        middleLevelView.LeanRotate(middleLevelViewRot.eulerAngles, levelNavAnimationSpeed).setEaseOutCubic();
        middleLevelView.LeanScale(middleLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        rightLevelView.transform.position = middleLevelViewPos;
        rightLevelView.transform.rotation = middleLevelViewRot;
        rightLevelView.transform.localScale = middleLevelViewScale;
        rightLevelView.LeanMove(rightLevelViewPos, levelNavAnimationSpeed).setEaseOutCubic();
        rightLevelView.LeanRotate(rightLevelViewRot.eulerAngles, levelNavAnimationSpeed).setEaseOutCubic();
        rightLevelView.LeanScale(rightLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        leftLevelView.LeanScale(Vector3.zero, 0f);
        if (selectedLevelIndex!=0 && selectedLevelIndex!=5 && selectedLevelIndex!=10 && selectedLevelIndex!=15)
        {
            leftLevelView.LeanScale(leftLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        }
    }

    public void AnimateNavigateRight()
    {
        middleLevelView.transform.position = rightLevelViewPos;
        middleLevelView.transform.rotation = rightLevelViewRot;
        middleLevelView.transform.localScale = rightLevelViewScale;
        middleLevelView.LeanMove(middleLevelViewPos, levelNavAnimationSpeed).setEaseOutCubic();
        middleLevelView.LeanRotate(middleLevelViewRot.eulerAngles, levelNavAnimationSpeed).setEaseOutCubic();
        middleLevelView.LeanScale(middleLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        leftLevelView.transform.position = middleLevelViewPos;
        leftLevelView.transform.rotation = middleLevelViewRot;
        leftLevelView.transform.localScale = middleLevelViewScale;
        leftLevelView.LeanMove(leftLevelViewPos, levelNavAnimationSpeed).setEaseOutCubic();
        leftLevelView.LeanRotate(leftLevelViewRot.eulerAngles, levelNavAnimationSpeed).setEaseOutCubic();
        leftLevelView.LeanScale(leftLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        rightLevelView.LeanScale(Vector3.zero, 0f);
        if (selectedLevelIndex!=4 && selectedLevelIndex!=9 && selectedLevelIndex!=14 && selectedLevelIndex!=19)
        {
            rightLevelView.LeanScale(rightLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        }
    }

    public void UpdateDisplayView()
    {
        // For error handling, show nothing if there are no levels
        if (levelThumbnails.Length > MinLevel)
        {
            middleLevelView.SetActive(true);
            Image mThumbnail = middleLevelView.transform.Find("LevelThumbnail").GetComponent<Image>();
            mThumbnail.sprite = levelThumbnails[selectedLevelIndex];
            TextMeshProUGUI mTextMesh = middleLevelView.transform.Find("LevelTitleText").GetComponent<TextMeshProUGUI>();
            mTextMesh.text = levelTitles[selectedLevelIndex];
        }
        else
        {
            middleLevelView.SetActive(false);
        }

        // Display Left View
        if (selectedLevelIndex > MinLevel && levelThumbnails.Length>selectedLevelIndex-1)
        {
            leftNavigateButton.gameObject.SetActive(true);
            Image lThumbnail = leftLevelView.transform.Find("LevelThumbnail").GetComponent<Image>();
            lThumbnail.sprite = levelThumbnails[selectedLevelIndex - 1];
            TextMeshProUGUI lTextMesh = leftLevelView.transform.Find("LevelTitleText").GetComponent<TextMeshProUGUI>();
            lTextMesh.text = levelTitles[selectedLevelIndex - 1];
        }
        else
        {
            leftNavigateButton.gameObject.SetActive(false);
            leftLevelView.LeanScale(Vector3.zero, 0f);
        }

        // Display Right View
        if (selectedLevelIndex < levelThumbnails.Length - 1 && selectedLevelIndex<MaxLevel)
        {
            rightNavigateButton.gameObject.SetActive(true);
            Image rThumbnail = rightLevelView.transform.Find("LevelThumbnail").GetComponent<Image>();
            rThumbnail.sprite = levelThumbnails[selectedLevelIndex + 1];
            TextMeshProUGUI rTextMesh = rightLevelView.transform.Find("LevelTitleText").GetComponent<TextMeshProUGUI>();
            rTextMesh.text = levelTitles[selectedLevelIndex + 1];  
        }
        else
        {
            rightNavigateButton.gameObject.SetActive(false);
            rightLevelView.LeanScale(Vector3.zero, 0f);
        }

        // Display Locked Level Button
        if (LevelStates.getIsLockedLevel(selectedLevelIndex))
        {
            playLevelButton.gameObject.SetActive(false);
            lockedLevelButton.gameObject.SetActive(true);
        }
        else
        {
            playLevelButton.gameObject.SetActive(true);
            lockedLevelButton.gameObject.SetActive(false);
        }
    }
}
