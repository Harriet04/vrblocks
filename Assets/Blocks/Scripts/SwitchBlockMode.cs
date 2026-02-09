using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlockMode : MonoBehaviour
{
    //AddObject TestLink;//refrence variable to AddObject script 
    bool SwitchON = false;// boolean for detecting when the switch is active
    // Start is called before the first frame update
    void Start()
    {
       // TestLink = GameObject.FindGameObjectWithTag("AddObject").GetComponent<AddObject>();// locates existing refrence of AddObject
    }

    public void ClearList()// set clear list function here
    {
        Debug.Log("Clear List Called");
    }
    public void Switch()
    {
        ClearList(); //place holder for actual clear block list function
        //TestLink.LinkTest();//calls public function fron object


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
