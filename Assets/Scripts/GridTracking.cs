using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GridTracking : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    public Transform pointer;
    [Header("Select to transform")]
    public bool x;
    public bool y;
    public bool z;

    [SerializeField]
    private LayerMask placementLayermask;

    public Vector3 GetSelectedMapPosition()
    {
        float newZ = z ? pointer.position.z : transform.position.z;
        float newX = x ? pointer.position.x : transform.position.x;
        float newY = y ? pointer.position.y : transform.position.y;
        transform.position = new Vector3(newX, newY, newZ);
        Vector3 controlerPos = transform.position;

        controlerPos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(controlerPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
