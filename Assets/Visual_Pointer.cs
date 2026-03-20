using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual_Pointer : MonoBehaviour
{
    public float amplitude = 1.0f; // Maximum distance 
    public float frequency = 1.0f; // Speed of the oscillation
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float Displace = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(0, Displace, 0);
        
    }
}
