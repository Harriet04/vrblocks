using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

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

    void LoadLevel(MapBlockScriptableObject Level)
    {
        //load terrain blocks
        foreach (Vector3 Point in Level.spawnPoints)
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Point);
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[0], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale*0.5f;
            ActiveGameObjects.Add(NewObj);

        }

        //load flag
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Level.goalSpawnPoint);
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[2], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale * 0.5f;
            ActiveGameObjects.Add(NewObj);
        }

        //load turtle
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Level.turtleSpawnPoint);
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[3], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale * 0.5f;
            ActiveGameObjects.Add(NewObj);
        }
    }

}
