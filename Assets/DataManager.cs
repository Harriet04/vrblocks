using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlacementSystem;

public class DataManager : MonoBehaviour
{
    //ScriptableObject newScriptableObject = ScriptableObject.CreateInstance(typeof(MapBlockScriptableObject));
    ScriptableObject newScriptableObject;
    // Start is called before the first frame update
    List<LevelObject> objects = new List<LevelObject>();
    public ScriptableObject scriptableObject;
    PlacementSystem s1;

    /*void Start()
    {
        
        //New game

        //lod game
    }
    /*public void AccessObjectList()
    {
        PlacementSystem sourceInstance = new PlacementSystem();
        List<LevelObject> accessedObjects = sourceInstance.LevelObjects;

        foreach (LevelObject obj in accessedObjects) {

    }*/

    // Update is called once per frame
    public void Start()
    {
        //save game
        //objects = PlacementSystem.getList();
        s1 = GetComponent<PlacementSystem>();
        //newScriptableObject = Instantiate(scriptableObject);

        MapBlockScriptableObject scriptReference = (MapBlockScriptableObject)scriptableObject;
        //newScriptableObject = (MapBlockScriptableObject.Instantiate);     .GetComponent<MapBlockScriptableObject>();
        MapBlockScriptableObject dummyObject = ScriptableObject.CreateInstance<MapBlockScriptableObject>();
        //newScriptableObject.SetDirty();
        //scriptReference.name = "newMap";
        dummyObject.name = "DummyName";
        //newScriptableObject.animationSpeed = 4;
        //newScriptableObject.blockPrefabName = "MapBlock";
        //newScriptableObject.blockScale = new Vector3(0.5f,0.5f,0.5f);
        //newScriptableObject.goalPositionOffset = ;
        //newScriptableObject.goalPrefabName = "GoalFlag";
        //newScriptableObject.goalRotation = ;
        //newScriptableObject.goalScale = new Vector3(0.3f, 0.3f, 0.3f);

        dummyObject.goalSpawnPoint = new Vector3(0, 1, 0);
        //newScriptableObject.movementDuration = ;
        dummyObject.spawnPoints = new Vector3[2];
        dummyObject.spawnPoints[0] = new Vector3(0,0,0);
        dummyObject.spawnPoints[1] = new Vector3(-1,0,0);
        //newScriptableObject.turtlePrefabName = "Turtle";
        //newScriptableObject.turtleRotation = 0;
        dummyObject.turtleSpawnPoint = new Vector3(-1, 1, 0);
        /*int count = 0;
        
        //NEED TO check that there is a turtle and flag present, and fail if not
        dummyObject.spawnPoints = new Vector3[4];// This is assuming that there is a flag and a turtle
        foreach (LevelObject temp in s1.levelObjects) 
        {
            if (temp.type == 2)
            {
                dummyObject.turtleSpawnPoint = new Vector3(temp.pos.x, temp.pos.y, temp.pos.z);
            }
            else if (temp.type == 3)
            {
                dummyObject.goalSpawnPoint = new Vector3(temp.pos.x, temp.pos.y, temp.pos.z);
            }
            else
            {
                dummyObject.spawnPoints[0] = new Vector3(temp.pos.x, temp.pos.y, temp.pos.z);
                count = count + 1;
            }
        }*/
     //    newScriptableObject = scriptReference;
        AssetDatabase.CreateAsset(dummyObject, "Assets/Map/MapLayouts/" + dummyObject.name + ".asset");
        
        AssetDatabase.SaveAssets();
    }
}
