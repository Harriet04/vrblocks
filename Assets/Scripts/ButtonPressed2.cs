using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed2 : MonoBehaviour
{
     public ExecutionDirector executionDirector;
     public CodingModeSettings codingModeSettings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Call()
    {
        //delete all existing blocks
        executionDirector.mainBlockList.Clear();
        GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in oldblocks)
        {
            Destroy(obj);
        }
        codingModeSettings.spawnCounter = 1;
        codingModeSettings.heightOffset = codingModeSettings.startBlock.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
