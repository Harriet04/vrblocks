using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class CodingModeSettings : MonoBehaviour
{

    public int CodingMode = 0;  //Coding mode setting. 0 = Normal Mode (default) 1 = Simple Mode
    public ExecutionDirector ExecutionDirector;
    public GameObject startBlock;
    public Transform window;

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

        startBlock.GetComponent<Rigidbody>().useGravity = true;
        startBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //startBlock.GetComponent<BoxCollider>().isTrigger = false;

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

        //pull start block to initial position
        startBlock.GetComponent<Rigidbody>().useGravity = false;
        startBlock.transform.position = new Vector3(window.position.x, window.position.y, window.position.z);
        startBlock.transform.eulerAngles = new Vector3(window.transform.eulerAngles.x, window.transform.eulerAngles.y, window.transform.eulerAngles.z);
        startBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        startBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        //startBlock.GetComponent<BoxCollider>().isTrigger = true;

        Debug.Log("Current Mode = Simple, block list cleared");
    }




}
