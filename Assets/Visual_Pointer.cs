using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual_Pointer : MonoBehaviour
{
    public float amplitude = 1.0f; // Maximum distance 
    public float frequency = 1.0f; // Speed of the oscillation

    public Transform pointerArrow; //variable to hold secondary transform information
    public Vector3 ArrowRotation; //variable to edit global rotation in editor
    public Transform textLabel; // variable to hold text label transform information
    // Start is called before the first frame update
    void Start()
    {
        pointerArrow.transform.rotation = Quaternion.Euler(ArrowRotation); //set arrow global rotation to ArrowRotation values.
    }

    // Update is called once per frame
    void Update()
    {

        float Displace = Mathf.Sin(Time.time * frequency) * amplitude; //sine wave ossilation function, runs everyframe, stored as a variable
        pointerArrow.transform.localPosition = new Vector3(0, Displace, 0); //offsets pointer Arrow Local Position by (Displace) distance along the y axis
        
    }
}
