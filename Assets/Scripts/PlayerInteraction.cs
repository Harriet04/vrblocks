using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;  //how far player needs to be to interact
    Interactable currentInteractable;   //current interactable object being looked at
    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if(Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Invoke();
        }
    }

    //check every frame if an object is in view
    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //shoots a line straight from the camera
        if(Physics.Raycast(ray, out hit, playerReach))  //ray - ray being scanned, out hit - output variable, playerReach - limiting distance
        {
            if(hit.collider.tag == "Interactable")  //if it is looking at an object with tag "Interactable"
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();    //reference to interactable object
                if (currentInteractable && newInteractable != currentInteractable)  //if there is a currentInteractable and it is not the newInteractable
                {
                    currentInteractable.DisableOutline();
                }
                if (newInteractable.enabled)    //check if it's enabled
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else //if the interactable you're looking at is not currently enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else //if object does not have Interavtable tag
            {
                DisableCurrentInteractable();
            }
        }
        else //if nothing is in reach
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)    //when cursor is on object
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();    //turn on outline
        HUDController.instance.EnableInteractionText(currentInteractable.message);  //set interaction text to whatever the object's message is
    }

    void DisableCurrentInteractable()   //when cursor is taken off of object
    {
        HUDController.instance.DisableInteractionText();    //turn off interaction text
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();   //turn off outline
            currentInteractable = null;
        }
    }
}
