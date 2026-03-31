using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class CodingModeSettings : MonoBehaviour
{

    public int CodingMode = 0;  //Coding mode setting. 0 = Normal Mode (default) 1 = Simple Mode
    public ExecutionDirector ExecutionDirector;
    public GameObject startBlock;
    public GameObject CodingWindow;
    public Transform window;
    public int spawnCounter = 1;

    private Vector3 scaleValue = new Vector3(0.25f, 0.125f, 0.25f);
    public Vector3 simpleOffset = new Vector3(0.0f, -0.125f, 0.0f);
    public Vector3 heightOffset;
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


        //hide coding window
        CodingWindow.SetActive(false);

        startBlock.GetComponent<Rigidbody>().useGravity = true;
        startBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        startBlock.GetComponent<Rigidbody>().isKinematic = false; //turn on physics
        //startBlock.GetComponent<BoxCollider>().isTrigger = false;

        Debug.Log("Current Mode = Normal, block list cleared");

    }
    public void SetModeSimple()
    {
        CodingMode=1;
        spawnCounter=1;

        //delete all existing blocks
        ExecutionDirector.mainBlockList.Clear();
        GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in oldblocks)
        {
            Destroy(obj);
        }


        //show coding window
        CodingWindow.SetActive(true);

        //pull start block to initial position
        startBlock.GetComponent<Rigidbody>().useGravity = false;
        startBlock.transform.LeanScale(scaleValue,0.0f);
        startBlock.transform.position = new Vector3(window.position.x - 0.3f, window.position.y + 0.4f, window.position.z);
        startBlock.transform.eulerAngles = new Vector3(window.transform.eulerAngles.x, window.transform.eulerAngles.y, window.transform.eulerAngles.z);
        startBlock.GetComponent<Rigidbody>().isKinematic = true; //turn off physics
        heightOffset = startBlock.transform.position;
        //startBlock.GetComponent<BoxCollider>().isTrigger = true;

        Debug.Log("Current Mode = Simple, block list cleared");
    }




}
