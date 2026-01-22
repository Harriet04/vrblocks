using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlacementSystem;

public class DataManager : MonoBehaviour
{
    
    ScriptableObject newScriptableObject;
    List<LevelObject> objects = new List<LevelObject>();
    public ScriptableObject scriptableObject;
    PlacementSystem s1 = new PlacementSystem();

    // Start is called before the first frame update
    public void Start()
    {
        //The list is taken from the PlacementSystem script
        objects = s1.getList();
        
        //An instance of the scriptable object is created for editing
        MapBlockScriptableObject scriptReference = (MapBlockScriptableObject)scriptableObject;
        MapBlockScriptableObject dummyObject = ScriptableObject.CreateInstance<MapBlockScriptableObject>();
        
        //NEED TO get other names so multiple saves can be created
        dummyObject.name = "DummyName";
        
        //A count is innitialized to track how many blocks have been placed
        int count = 0;

        //NEED TO check that there is a turtle and flag present, and fail if not

        //The amount of items in the list is recorded two messages are sent to check if the value is consistant
        MonoBehaviour.print(s1.getListSize());
        int listSize = s1.getListSize();
        MonoBehaviour.print(listSize);
        //The amount of block spawn points is extrapilated 
        dummyObject.spawnPoints = new Vector3[listSize - 2];// This is assuming that there is a flag and a turtle, thus minus 2 as they are not recorded as blocks
        //A check to ensure that the program has not failed based on null values
        MonoBehaviour.print("CheckPoint");
        
        //The assigning of positions based on the type of block
        foreach (LevelObject temp in objects) 
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
                dummyObject.spawnPoints[count] = new Vector3(temp.pos.x, temp.pos.y, temp.pos.z);
                count = count + 1;
            }
        }
        
        //The asset is created and saved to the system
        AssetDatabase.CreateAsset(dummyObject, "Assets/Map/MapLayouts/" + dummyObject.name + ".asset");
        AssetDatabase.SaveAssets();

        //NEED TO clear the table -> traverse and call deleteObject
    }
}
