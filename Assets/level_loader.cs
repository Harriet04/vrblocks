using System.Collections;
using System.Collections.Generic;
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
        foreach (Vector3 Point in Level.spawnPoints)
        {
            Vector3Int GridCoord = Vector3Int.RoundToInt(Point);
            Vector3 SpawnCoords = Grid.CellToWorld(GridCoord);
            GameObject NewObj = Instantiate(PlacementObjects[0], SpawnCoords, Quaternion.identity);
            NewObj.transform.localScale = Grid.transform.lossyScale*0.5f;
            ActiveGameObjects.Add(NewObj);


        }
    }

}
