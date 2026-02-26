using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodingModeSettings : MonoBehaviour
{

    public int CodingMode = 0;  //Coding mode setting. 0 = Normal Mode (default) 1 = Simple Mode
    public ExecutionDirector ExecutionDirector;

    public void SetModeNormal()
    {
        CodingMode=0;

        //delete all existing blocks
        ExecutionDirector.mainBlockList.Clear();
        GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in oldblocks)
        {
            Destroy(obj);
        }

        Debug.Log("Current Mode = Normal, block list cleared");

    }
    public void SetModeSimple()
    {
        CodingMode=1;

        //delete all existing blocks
        ExecutionDirector.mainBlockList.Clear();
        GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in oldblocks)
        {
            Destroy(obj);
        }

        Debug.Log("Current Mode = Simple, block list cleared");
    }




}
