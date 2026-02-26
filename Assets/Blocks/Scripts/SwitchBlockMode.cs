using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlockMode : MonoBehaviour
{
    //AddObject TestLink;//refrence variable to AddObject script 
    bool SwitchON = false;// boolean for detecting when the switch is active
    public CodingModeSettings CodingModeSettings;



    // Start is called before the first frame update
    void Start()
    {
       // TestLink = GameObject.FindGameObjectWithTag("AddObject").GetComponent<AddObject>();// locates existing refrence of AddObject
    }

    public void Switch()
    {
        // *****ADD CLEAR LIST FUNCTION CALL IN EXECUTIONDIRECTOR*****

        //TestLink.LinkTest();//calls public function fron object


        if (SwitchON == true)// switch statment for when the switch is active
        {
            SwitchON = false; Debug.Log("Switch to Normal Mode");
            CodingModeSettings.SetModeNormal();
        } else
        {
            SwitchON = true; Debug.Log("Switch to Simple Mode");
            CodingModeSettings.SetModeSimple();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
