using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodingModeSettings : MonoBehaviour
{

    public int CodingMode = 0;  //Coding mode setting. 0 = Normal Mode (default) 1 = Simple Mode

    public void SetModeNormal()
    {
        CodingMode=0;
        Debug.Log("Current Mode = Normal");
    }
    public void SetModeSimple()
    {
        CodingMode=1;
        Debug.Log("Current Mode = Simple");
    }




}
