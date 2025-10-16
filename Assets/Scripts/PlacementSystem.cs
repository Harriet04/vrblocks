using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;

    [SerializeField]
    private GridTracking gridtracking;

    private void Update()
    {
        Vector3 mousePosition = gridtracking.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;
    }
}
