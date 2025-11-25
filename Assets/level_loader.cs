using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(level_loader))]
public class LevelLoaderMenu : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        level_loader LevelLoader = (level_loader)target;

        if (GUILayout.Button("Load Level"))
        {
            LevelLoader.LoadLevel(LevelLoader.TestLevel);
        }
        if (GUILayout.Button("Clear Level"))
        {
            LevelLoader.clearLevel();
        }
    }
}

public class level_loader : MonoBehaviour
{
    // Start is called before the first frame update
    public MapBlockScriptableObject TestLevel;
    public List<GameObject> PlacementObjects = new List<GameObject>();
    public Grid Grid;


    private List<GameObject> ActiveGameObjects = new List<GameObject>();
    void Start()
    {
        LoadLevel(TestLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(MapBlockScriptableObject Level)
    {
        clearLevel(); //Don't want multiple levels to stack on top of each other

        //load terrain blocks
        foreach (Vector3 Point in Level.spawnPoints)
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Point) + Level.offsetSpawnPoints;
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[0], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale*0.5f;
            ActiveGameObjects.Add(NewObj);
            //Parent it to the level loader
            NewObj.transform.SetParent(this.gameObject.transform);

        }

        //load flag
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Level.goalSpawnPoint) + Level.offsetSpawnPoints;
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord)+Level.goalPositionOffset*Grid.transform.lossyScale.x;
            GameObject NewObj = Instantiate(PlacementObjects[2], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale * 0.5f;
            ActiveGameObjects.Add(NewObj);
            NewObj.transform.SetParent(this.gameObject.transform);
        }
        
        //load turtle
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Level.turtleSpawnPoint) + Level.offsetSpawnPoints;
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[3], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale * 0.5f;
            ActiveGameObjects.Add(NewObj);
            NewObj.transform.SetParent(this.gameObject.transform);
        }
    }

    public void clearLevel()
    {
        foreach (GameObject NewObj in ActiveGameObjects)
        {
            DestroyImmediate(NewObj);
        }
        ActiveGameObjects.Clear();
    }
}
