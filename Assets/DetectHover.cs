using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine;

public class DetectHover : MonoBehaviour
{
    public XRRayInteractor RrayInteractor;
    public XRRayInteractor LrayInteractor;
    public GameObject Keyboard;
    public string UI_InputName;

    private bool RightHovered = false;
    private bool LeftHovered = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Detect UI Element Hits
        if (RrayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit))
        {
            //Debug.Log($"Hovering over UI: {uiHit.gameObject.name}");
            if(uiHit.gameObject.name== UI_InputName)
            {
                //Debug.Log("CORRECT UI DETECTED"); //RIGHT HAND HOVER DETECTED
                //Keyboard.SetActive(true);//activates keyboard group
                RightHovered = true; //conditional for if the right hand is hovering
            }
            else
            {
                RightHovered = false; //conditional for if the right hand is not hovering
            }

        }
        else
        {
            RightHovered = false; //conditional for if the hands are not on ui
        }

        if (LrayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit2))
        {
            //Debug.Log($"Hovering over UI: {uiHit.gameObject.name}");
            if (uiHit2.gameObject.name == UI_InputName)
            {
                //Debug.Log("CORRECT UI DETECTED"); //LEFT HAND HOVER DETECTED
                LeftHovered = true; //conditional for if the left hand is hovering
            }
            else
            {
                LeftHovered = false; //conditional for if the left hand is not hovering
            }

        }
        else
        {
            LeftHovered = false; //conditional for if the hands are not on ui
        }

        //logic for turning the keyboard on and off based on if a hand is hovered
        if (RightHovered | LeftHovered)
        {
            Keyboard.SetActive(true);
            //Debug.Log("hovered");
        }
        else
        {
            Keyboard.SetActive(false);
            //Debug.Log("not hovered");
        }
    }
}

