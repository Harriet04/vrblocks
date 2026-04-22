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
                Debug.Log("CORRECT UI DETECTED"); //RIGHT HAND HOVER DETECTED
                Keyboard.SetActive(true);//activates keyboard group
            }
            
        }
        if (LrayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit2))
        {
            //Debug.Log($"Hovering over UI: {uiHit.gameObject.name}");
            if (uiHit2.gameObject.name == UI_InputName)
            {
                Debug.Log("CORRECT UI DETECTED"); //LEFT HAND HOVER DETECTED
                Keyboard.SetActive(true);//activates keyboard group
            }

        }
    }
}

