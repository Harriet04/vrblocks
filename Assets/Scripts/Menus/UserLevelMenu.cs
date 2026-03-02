/*
 Level Selector + animations
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(UserLevelMenu))]
public class UserLevelSelectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UserLevelMenu LevelSelector = (UserLevelMenu)target;

        if (GUILayout.Button("Propagate Dev Levels"))
        {
            AutoFillLevels(LevelSelector);
        }
    }
    private void AutoFillLevels(UserLevelMenu menu)
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

public class UserLevelMenu : MonoBehaviour
{
    public Button playLevelButton;
    public Button leftNavigateButton;
    public Button rightNavigateButton;
    public Button lockedLevelButton;

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
    private List<Sprite> levelThumbnails;
    private List<string> levelTitles;

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
        FindPersistentLevels();
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
        FindPersistentLevels();
        //Need to retrieve the levels from Application.persistentDataPath/Levels
        //Each level is in its own folder; that folder being the level name.
        //In that folder, you have the MetaData and MapBlock objects in JSON; need to convert them to scriptable objects
        //You also have the thumbnail as a png. You'll need to convert that to a sprite.

        /*LevelMetadataScriptableObject[] levelMetadataScriptables = GameObject.Find("/LevelStatesManager").GetComponent<LevelStatesManager>().levelMetadataScriptables;
        levelThumbnails = new Sprite[levelMetadataScriptables.Length];
        levelTitles = new string[levelMetadataScriptables.Length];
        for (int i = 0; i < levelMetadataScriptables.Length; i++)
        {
            levelThumbnails[i] = levelMetadataScriptables[i].levelThumbnail;
        }
        for (int i = 0; i < levelMetadataScriptables.Length; i++)
        {
            levelTitles[i] = levelMetadataScriptables[i].displayName;
        }*/

        playLevelButton.onClick.AddListener(GoToLevel);
        leftNavigateButton.onClick.AddListener(() =>
        {
            selectedLevelIndex = Math.Max(0, selectedLevelIndex - 1);
            UpdateDisplayView();
            AnimateNavigateLeft();
        });
        rightNavigateButton.onClick.AddListener(() =>
        {
            selectedLevelIndex = Math.Min(selectedLevelIndex + 1, levelThumbnails.Count - 1);
            UpdateDisplayView();
            AnimateNavigateRight();
        });

        UpdateDisplayView();
    }

    //Function that searches Application.persistentDataPath/Levels and stores them
    void FindPersistentLevels()
    {
        print("FINDING PERSISTENT LEVELS");

        //First, clear all of the lists/arrays
        //Or create them if they don't exist
        if (levelData == null){ levelData = new List<MapBlockScriptableObject>(); }
        else { levelData.Clear(); }

        if (levelThumbnails == null) { levelThumbnails = new List<Sprite>(); }
        else { levelThumbnails.Clear(); }

        if(levelTitles == null) { levelTitles = new List<string>(); }
        else { levelTitles.Clear(); }


        string rootPath = Path.Join(Application.persistentDataPath, "Levels");
        if (Directory.Exists(rootPath))
        {
            print("Directory Found!");
            string[] SubDirectories = Directory.GetDirectories(rootPath);

            foreach (string subDirectory in SubDirectories)
            {
                print("Searching " + subDirectory);

                //INSERT A CHECK TO VERIFY THIS FOLDER HAS ALL 3 FILES
                if (
                    !File.Exists(Path.Join(subDirectory, "thumbnail.png"))
                    || !File.Exists(Path.Join(subDirectory, "MetaData"))
                    || !File.Exists(Path.Join(subDirectory, "MapBlock"))

                    ) {
                    print("A file is missing from " + subDirectory);
                    continue;
                }


                //Retrieve the MetaData
                string meta_JSON = File.ReadAllText(Path.Join(subDirectory, "MetaData"));
                LevelMetadataScriptableObject meta = LevelMetadataScriptableObject.CreateInstance<LevelMetadataScriptableObject>();
                JsonUtility.FromJsonOverwrite(meta_JSON, meta);
                print("metadata display name: " + meta.displayName);
                levelTitles.Add(meta.displayName);



                //Retrieve the Thumbnail
                byte[] png = File.ReadAllBytes(Path.Join(subDirectory, "thumbnail.png"));

                Texture2D texture = new Texture2D(2, 2); // size doesn't matter, will resize
                texture.LoadImage(png); // loads and resizes texture

                texture.filterMode = FilterMode.Bilinear; // optional
                texture.wrapMode = TextureWrapMode.Clamp;

                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f) // pivot center
                );
                levelThumbnails.Add(sprite);



                //Retrieve the MapBlocks
                string mb_JSON = File.ReadAllText(Path.Join(subDirectory,"MapBlock"));
                MapBlockScriptableObject mb = MapBlockScriptableObject.CreateInstance<MapBlockScriptableObject>();
                JsonUtility.FromJsonOverwrite(mb_JSON, mb);
                levelData.Add(mb);
            }
        }
        else
        {
            print("Root Directory: " + rootPath + " not found");
        }
        //Otherwise, the directory doesnt exist; ie no user levels yet
    }

    private void Update() //all mono prints have been comented to provide a less cluttered console at runtime (they are all just checkpoints for testing)
    {
        //MonoBehaviour.print("Updating user level list");
        string path = "Assets/Map/SandboxLevels/"; //initial path to follow (could possibly be improved by taking the dev input path?)
        
        string[] fileEntries = Directory.GetFiles(path);//This is gettting the scriptable object files and metadata from the specified path
        //Only adding the level asset data, the metat dat needs to go elsewhere (I don't know where the level selector metatdata is taken from, but it needs to be routed there)
        List<MapBlockScriptableObject> levelList = new List<MapBlockScriptableObject>();
        foreach (string entry in fileEntries)
        {
            //MonoBehaviour.print(entry);
            if (entry.EndsWith(".meta")) { /*MonoBehaviour.print("metadata skipped"); */}
            else {
                //MonoBehaviour.print("Adding scriptable object");
                levelList.Add(AssetDatabase.LoadAssetAtPath<MapBlockScriptableObject>(entry));
            }
                
        }
        //This makes sure the collection was successfull
            if (levelList != null)
        {
            //MonoBehaviour.print("list=! null");
            //MonoBehaviour.print(levelList.Count());
            
            //check the list before adding new values so there aren't infinate
            foreach (MapBlockScriptableObject level in levelList)
            {
                //MonoBehaviour.print(level.name);
                if (!levelData.Contains(level))
                {
                    //MonoBehaviour.print("level not found, AddingNewEventArgs to list");
                    levelData.Add(level);//This method clears them after session ends, but will reload them on each start up
                }
                else { /*MonoBehaviour.print("Level found, skipping"); */}
            }
        }
    }

    public void SetMinMaxLevel(int minLevel, int maxLevel)
    {
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        selectedLevelIndex = minLevel;
        UpdateDisplayView();
    }

    public void GoToLevel()
    {
        //Load into the levelLoader
        if (LoadIntoCurrentScene)
        {
            if (levelData.Count > selectedLevelIndex)
            {
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
        if (selectedLevelIndex > 0)
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
        if (selectedLevelIndex < levelThumbnails.Count - 1)
        {
            rightLevelView.LeanScale(rightLevelViewScale, levelNavAnimationSpeed).setEaseOutCubic();
        }
    }

    public void UpdateDisplayView()
    {

        // For error handling, show nothing if there are no levels
        if (levelThumbnails.Count > MinLevel)
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
        if (selectedLevelIndex > MinLevel && levelThumbnails.Count > selectedLevelIndex - 1)
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
        if (selectedLevelIndex < levelThumbnails.Count - 1 && selectedLevelIndex < MaxLevel)
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
        //No user levels should be locked, I need to work on metat data next sprint
        /*if (LevelStates.getIsLockedLevel(selectedLevelIndex))
        {
            playLevelButton.gameObject.SetActive(false);
            lockedLevelButton.gameObject.SetActive(true);
        }
        else
        {*/
            playLevelButton.gameObject.SetActive(true);
            lockedLevelButton.gameObject.SetActive(false);
        //}
    }
}