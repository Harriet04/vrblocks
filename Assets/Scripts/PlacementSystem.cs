using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator, cellIndicator;
    
    [SerializeField]
    private GridTracking gridtracking;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Vector3 minVal, maxVal;

    [SerializeField]
    GameObject PlacementObject, UnsnappedObject, Hand;

    [SerializeField]
    private Vector2 CastDistanceRange;
    private float CastDistance = 0.0f;
    [SerializeField]
    private float PushSensitivity = 0.2f; //How much to push by per second of having an input held

    public InputActionProperty PushInput;
    public InputActionProperty PullInput;

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
        CastDistance = Mathf.Clamp(CastDistance,CastDistanceRange.x,CastDistanceRange.y);

            //Sphere should be roughly CastDistance away from the hand. We'll snap that position to the 3D grid.
            //The forward vector on the hand mesh is facing backwards, that's why I'm subtracting.
            Vector3 LoosePosition = Hand.transform.position - Hand.transform.forward * CastDistance;
        //clamping to grid
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(LoosePosition.x, minVal.x, maxVal.x),
            Mathf.Clamp(LoosePosition.y, minVal.y, maxVal.y),
            Mathf.Clamp(LoosePosition.z, minVal.z, maxVal.z)
            );
        //WorldToCell floors rather than rounds, so add half a cell
        Vector3Int SnappedCell = grid.WorldToCell(clampedPosition + grid.cellSize*0.5f);
        Vector3 SnappedPosition = grid.CellToWorld(SnappedCell);
        PlacementObject.transform.position = SnappedPosition;
        UnsnappedObject.transform.position = clampedPosition;

    }
}
