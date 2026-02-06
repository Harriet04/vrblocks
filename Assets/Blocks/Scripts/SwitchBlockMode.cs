using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlockMode : MonoBehaviour
{
    bool SwitchON = false;// boolean for detecting when the switch is active
    // Start is called before the first frame update
    void Start()
    {
  
    }
    public void Switch()
    {
        if (SwitchON == true)// switch statment for when the switch is active
        {
            SwitchON = false; Debug.Log("Simple Mode: OFF");
        } else
        {
            SwitchON = true; Debug.Log("Simple Mode: ON");
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
