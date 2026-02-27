using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlacementSystem;

public class DataManager : MonoBehaviour
{
    
    ScriptableObject newScriptableObject;
    List<LevelObject> objects = new List<LevelObject>();
    public ScriptableObject scriptableObject;
    public PlacementSystem s1;
    public ScreenshotCapturer screenshotCapturer;

    // Start is called before the first frame update
    public void OnEnable()
    {
        //The list is taken from the PlacementSystem script
        objects = s1.getList();
        
        //An instance of the scriptable object is created for editing
        MapBlockScriptableObject scriptReference = (MapBlockScriptableObject)scriptableObject;
        MapBlockScriptableObject dummyObject = ScriptableObject.CreateInstance<MapBlockScriptableObject>();
        
        //NEED TO get other names so multiple saves can be created
        dummyObject.name = "UserLevel" + Time.time;
        
        //A count is innitialized to track how many blocks have been placed
        int count = 0;

        //check that there is a turtle and flag present, and fail if not
        bool turtleExists = false, goalExists=false;
        foreach (LevelObject temp in objects)
        {
            if (temp.type == 1)
            {
                turtleExists=true;
            }
            else if (temp.type == 2)
            {
                goalExists=true;
            }
        }
        if (!turtleExists || !goalExists)
        {
            MonoBehaviour.print("Save Failed");
            return;
        }
        //The amount of items in the list is recorded two messages are sent to check if the value is consistant
        MonoBehaviour.print(s1.getListSize());
        int listSize = s1.getListSize();
        MonoBehaviour.print(listSize);
        //The amount of block spawn points is extrapilated 
        dummyObject.spawnPoints = new Vector3[listSize - 2];// This is assuming that there is a flag and a turtle, thus minus 2 as they are not recorded as blocks
        //A check to ensure that the program has not failed based on null values
        MonoBehaviour.print("CheckPoint");
        
        //Set default values
        dummyObject.blockPrefabName = "MapBlock";
        dummyObject.blockScale = new Vector3((float)0.5, (float)0.5, (float)0.5);
        dummyObject.turtlePrefabName = "Turtle";
        dummyObject.movementDuration = 1;
        dummyObject.animationSpeed = 4;
        dummyObject.goalPrefabName = "GoalFlag";
        dummyObject.goalScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
        dummyObject.offsetSpawnPoints = new Vector3Int(-5, -1, -5); //Offset from our sandbox mode grid

        //The assigning of positions based on the type of block
        foreach (LevelObject temp in objects) 
        {
            if (temp.type == 1)
            {
                dummyObject.turtleSpawnPoint = (Vector3)s1.grid.WorldToCell(temp.pos);
            }
            else if (temp.type == 2)
            {
                dummyObject.goalSpawnPoint = (Vector3)s1.grid.WorldToCell(temp.pos);
            }
            else
            {
                dummyObject.spawnPoints[count] = (Vector3)s1.grid.WorldToCell(temp.pos);
                count = count + 1;
            }
        }

        bool developerMode = true;
        if (!developerMode)
        {
            //We're storing this level in persistent memory; it was created by the player
            UnityEngine.Debug.Log(Application.persistentDataPath);
            string levelDir = Path.Combine(Application.persistentDataPath, "Levels", dummyObject.name);
            Directory.CreateDirectory(levelDir);

            string json = JsonUtility.ToJson(dummyObject, true);
            string path = Path.Combine(levelDir, "MapBlock");
            File.WriteAllText(path, json);
        }

        else
        {
            //Developer asset, save to Assets
            //The asset is created and saved to the system
            AssetDatabase.CreateAsset(dummyObject, "Assets/Map/SandboxLevels/" + dummyObject.name + ".asset");
            AssetDatabase.SaveAssets();
        }

        CreateMetaData(dummyObject.name,developerMode);


        s1.Clear();

        }

    public void CreateMetaData(string name, bool developerMode)
    {
        //Add Thumbnail to folder
        Texture2D tex = screenshotCapturer.CaptureFromCamera();
        byte[] png = tex.EncodeToPNG();



        string myPath;
        //If it's a developer-made level, use this path
        if (developerMode) { myPath = "Assets/LevelData/Thumbnails/"; }
        //Else, it's a player-made level
        else { myPath = Path.Combine(Application.persistentDataPath, "Levels", name); }

        string assetPath;
        if (developerMode) { assetPath = Path.Combine(myPath, name + ".png"); }
        else { assetPath = Path.Combine(myPath, "thumbnail.png"); }

        File.WriteAllBytes(
            assetPath,
            png
        );

        if (developerMode)
        {
            //Convert it to sprite
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            TextureImporter importer =
                (TextureImporter)AssetImporter.GetAtPath(assetPath);

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 100;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;

            importer.SaveAndReimport();
            AssetDatabase.Refresh();
        }


        // =================================================================================//
        //Create the Metadata
        // =================================================================================//

        LevelMetadataScriptableObject data= ScriptableObject.CreateInstance<LevelMetadataScriptableObject>();
        data.displayName = name;


        //developer-mode only
        if (developerMode)
        {
            data.levelThumbnail = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath); ;

            //This only works in editor; we'll need a separate pipeline for players
            AssetDatabase.CreateAsset(data, "Assets/LevelData/MetaData/" + name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        //Save to persistent memory for player
        else
        {
            string levelDir = Path.Combine(Application.persistentDataPath, "Levels", name);
            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(levelDir, "MetaData");
            File.WriteAllText(path, json);
        }

        //Clean-up texture
        Destroy(tex);
        Destroy(data);
    }
}
