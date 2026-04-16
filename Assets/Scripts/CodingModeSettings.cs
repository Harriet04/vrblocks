using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
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
    public Transform RightHandTransform;

    private GameObject currentObject;
    public InputActionReference hitN;
    public int numcols = 0;
    public void SetModeNormal()
    {
        CodingMode=0;
        spawnCounter=1;

        //delete all existing blocks
        ExecutionDirector.mainBlockList.Clear();
        GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in oldblocks)
        {
            Destroy(obj);
        }


        //hide coding window
        CodingWindow.SetActive(false);

        startBlock.transform.LeanScale(new Vector3(0.5f,0.25f,0.5f),0.0f);
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
        startBlock.transform.position = new Vector3(window.position.x - 0.3f, window.position.y + 0.4f, window.position.z + 0.2f);
        startBlock.transform.eulerAngles = new Vector3(window.transform.eulerAngles.x, window.transform.eulerAngles.y, window.transform.eulerAngles.z);
        startBlock.GetComponent<Rigidbody>().isKinematic = true; //turn off physics
        heightOffset = startBlock.transform.position;
        //startBlock.GetComponent<BoxCollider>().isTrigger = true;

        Debug.Log("Current Mode = Simple, block list cleared");
    }

    private void Update()
    {
        if (CodingMode == 1)
        {
            Ray ray = new Ray(RightHandTransform.position, RightHandTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f)) {
                if (hit.collider.CompareTag("Block")) {
                    currentObject = hit.collider.gameObject;
                }
                else
                {
                    currentObject = null;
                }
            }
            if (hitN.action.WasPressedThisFrame())
            {
                int tempspot = 1;
                int tempcount = 0;
                int tempmode = 0;
                foreach(GameObject obj in ExecutionDirector.mainBlockList)
                {
                    if(obj == currentObject)
                    {
                        tempmode = 1;
                        tempcount=tempspot;
                    }
                    else
                    {
                        if(tempmode == 0){
                            tempspot+=1;
                        }else if (tempmode == 1)
                        {
                            tempcount+=1;
                            if ((tempcount+1) % 8 == 1)
                            {
                                Vector3 temp = new Vector3(-0.175f,0.0f,0.225f);
                                obj.transform.position+=(simpleOffset*7+temp);
                            }else{
                                obj.transform.position+=(-simpleOffset);
                            }
                        }
                    }
                }
                
                ExecutionDirector.mainBlockList.RemoveAt(tempspot);
                Destroy(currentObject);
                spawnCounter-=1;
            }
        }
    }

    private void DeleteBlock()
    {
        
    }




}
