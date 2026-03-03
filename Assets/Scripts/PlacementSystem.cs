using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator, cellIndicator;
    
    [SerializeField]
    private GridTracking gridtracking;

    [SerializeField]
    public Grid grid;

    [SerializeField]
    private Vector3Int minVal, maxVal;

    [SerializeField]
    GameObject HoverObject, UnsnappedObject, Hand;

    int HoverObjectIndex = -1;

    [SerializeField]
    private Vector2 CastDistanceRange;
    private float CastDistance = 0.0f;
    [SerializeField]
    private float PushSensitivity = 0.2f; //How much to push by per second of having an input held

    public InputActionProperty PushInput;
    public InputActionProperty PullInput;
    public InputActionProperty PlaceInput;
    public InputActionProperty DeleteInput;
    public InputActionProperty CycleInput;

    public Vector3 goalPositionOffset;
    public Material HoverMaterial;

    //object to be placed into grid on input
    public List<GameObject> PlacementObjects = new List<GameObject>();
    private int CycleCounter = 0;
    //struct for placing new objects into managing vector
    public struct LevelObject
    {
        public GameObject obj;
        public Vector3 pos;

        public int type;
        public LevelObject(GameObject o, Vector3 v, int t)
        {
            obj = o;
            pos = v;
            type = t;
        }
    }
    //vector to hold all objects in grid
    public static List<LevelObject> levelObjects = new List<LevelObject>(); //changed to static for outside access
    public static int levelObjectsSize = 0;  //changed to static for outside access
    private void Start()
    {
        //Set CastDistance to the minimum cast value
        //We'll want to handle controller events to to push/pull this distance.
        CastDistance += CastDistanceRange.x;
    }


    private void Update()
    {
        /* Cast system
        Vector3 mousePosition = gridtracking.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        */

        //Push/pull the block if these are triggered
        if (PushInput.action.ReadValue<float>() > 0.5)
        {
            Debug.Log("Push");
            CastDistance += PushSensitivity * Time.deltaTime;
        }
        if (PullInput.action.ReadValue<float>() > 0.5)
        {
            Debug.Log("Pull");
            CastDistance -= PushSensitivity * Time.deltaTime;
        }
        CastDistance = Mathf.Clamp(CastDistance, CastDistanceRange.x, CastDistanceRange.y);

        //Sphere should be roughly CastDistance away from the hand. We'll snap that position to the 3D grid.
        //The forward vector on the hand mesh is facing backwards, that's why I'm subtracting.
        Vector3 LoosePosition = Hand.transform.position - Hand.transform.forward * CastDistance;
        //clamping to grid
        /*Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(LoosePosition.x, minVal.x, maxVal.x),
            Mathf.Clamp(LoosePosition.y, minVal.y, maxVal.y),
            Mathf.Clamp(LoosePosition.z, minVal.z, maxVal.z)
            );*/
        //WorldToCell floors rather than rounds, so add half a cell
        //lossyscale is the absolute scale of the grid
        Vector3Int SnappedCell = grid.WorldToCell(LoosePosition + Vector3.Scale(grid.cellSize,grid.transform.lossyScale) * 0.5f);
        //Clamp the Cell Position
        SnappedCell.x = Mathf.Clamp(SnappedCell.x, minVal.x, maxVal.x);
        SnappedCell.y = Mathf.Clamp(SnappedCell.y, minVal.y, maxVal.y);
        SnappedCell.z = Mathf.Clamp(SnappedCell.z, minVal.z, maxVal.z);

        Vector3 SnappedPosition = grid.CellToWorld(SnappedCell);

        //Change HoverObject to the object that is selected
        if(HoverObject == null || HoverObjectIndex != CycleCounter)
        {
            if(HoverObject != null) { Destroy(HoverObject);  }
            HoverObject = Instantiate(PlacementObjects[CycleCounter], SnappedPosition, Quaternion.Euler(0, 0, 0));
            HoverObject.transform.localScale = new Vector3(scaleFactor.localScale.x / 2, scaleFactor.localScale.y / 2, scaleFactor.localScale.z / 2);   //match scale of parent grid when placing
            HoverObject.transform.rotation = scaleFactor.transform.rotation;   //match rotation of parent grid when placing
            HoverObjectIndex = CycleCounter;

            //Set the highlight material on all of this GameObject's meshes.
            if (HoverMaterial != null)
            {
                foreach (Renderer r in HoverObject.GetComponentsInChildren<Renderer>(true))
                {
                    r.sharedMaterial = HoverMaterial;
                }
            }
        }


            HoverObject.transform.position = SnappedPosition;
        UnsnappedObject.transform.position = LoosePosition;


        //if place block input has been pressed, call place block function.
        if (PlaceInput.action.ReadValue<float>() > 0.5)
        {
            Debug.Log("Place Object");
            PlaceObject(SnappedPosition);
        }

        //if delete block input has been pressed
        if (DeleteInput.action.ReadValue<float>() > 0.5)
        {
            Debug.Log("Delete Object");
            DeleteObject(SnappedPosition);
        }

        //cycle through available objects to place
        if(CycleInput.action.WasPressedThisFrame())
        {
            Debug.Log("Cycle Objects");
            if (CycleCounter+1 == PlacementObjects.Count)
            {
                CycleCounter = 0;
            }
            else
            {
                CycleCounter += 1;
            }
        }
    }

    //check to make sure an object is not already in the position, then place object
    public Transform scaleFactor;
    private bool isTurtle = false;
    private bool isFlag = false;
    void PlaceObject(Vector3 posv)
    {
        bool canPlace = true;
        foreach (LevelObject temp in levelObjects)
        {
            if (temp.pos.x == posv.x && temp.pos.y == posv.y && temp.pos.z == posv.z)
            {
                canPlace = false;
                print("Cannot place");
            }
        }
        if (canPlace)
        {
            print("Attempting to Place");
            // Object might fail to instantiate?
            LevelObject temp = new LevelObject(Instantiate(PlacementObjects[CycleCounter], posv, Quaternion.Euler(0, 0, 0)), posv, CycleCounter);
            temp.obj.transform.localScale = new Vector3(scaleFactor.localScale.x/2, scaleFactor.localScale.y/2, scaleFactor.localScale.z/2);   //match scale of parent grid when placing
            temp.obj.transform.rotation = scaleFactor.transform.rotation;   //match rotation of parent grid when placing
            levelObjects.Add(temp);
            levelObjectsSize += 1;
            print("After instantiation");

            if (temp.type == 1 && isTurtle == false)    //remove old turtle if new one is placed
            {
                print("Is Turtle");
                isTurtle = true;
            }
            else if (temp.type == 1 && isTurtle == true)
            {
                foreach(LevelObject t in levelObjects)
                {
                    if(t.type == 1)
                    {
                        Destroy(t.obj);
                        levelObjects.Remove(t);
                        levelObjectsSize-=1;
                        continue;
                    }
                }
            }

            if (temp.type == 2 && isFlag == false)    //remove old flag if new one is placed
            {
                isFlag = true;
            }
            else if (temp.type == 2 && isFlag == true)
            {
                foreach(LevelObject t in levelObjects)
                {
                    if(t.type == 2)
                    {
                        Destroy(t.obj);
                        levelObjects.Remove(t);
                        levelObjectsSize-=1;
                        continue;
                    }
                }
            }
        }
        MonoBehaviour.print(levelObjectsSize);
    }

    //Save objects in their positions

    public int getListSize()
    {
        MonoBehaviour.print("save - system - size");
        MonoBehaviour.print(levelObjectsSize);
        return levelObjectsSize;
    }
    public List<LevelObject> getList()
    {
        MonoBehaviour.print("save - system - list");
        return levelObjects;
    }

    //delete object at given position
    void DeleteObject(Vector3 posv)
    {
        //We gotta check to see if this is the turtle or the flag
        foreach (LevelObject temp in levelObjects)
        {
            if(temp.pos.x == posv.x && temp.pos.y == posv.y && temp.pos.z == posv.z)
            {
                Debug.Log("block deleted");
                Destroy(temp.obj);
                levelObjects.Remove(temp);
                levelObjectsSize -= 1;

                //If we delete the turtle, or the flag, we need to set those bools to false
                if (temp.type == 1) { isTurtle = false; }
                if (temp.type == 2) { isFlag = false; }

                continue;
            }
        }
    }

    public void Clear()
    {
        //clear the table -> traverse and call deleteObject
        foreach (LevelObject temp in levelObjects)
        {
            Destroy(temp.obj);
        }
        levelObjects.Clear();
        levelObjectsSize = 0;

        isFlag = false;
        isTurtle = false;
    }

    void OnDisable()
    {
        //Claer objects
        foreach (LevelObject temp in levelObjects)
        {
            Destroy(temp.obj);
        }
        levelObjects.Clear();
        HoverObject.transform.position = Vector3.zero;
        UnsnappedObject.transform.position = Vector3.zero;
        
    }
    private void OnEnable()
    {
        
    }
}
