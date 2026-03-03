using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    public float deadTime = 1.0f; //Time to prevent button activation after press in frame?

    private bool _deadTimeActive = false; // Bool for button lock state

    private Vector3 initalLocalPos;// position refrence
    public Transform visualTarget;

    public UnityEvent onPressed, OnReleased;//public events for editor functions 

    private void OnTriggerEnter(Collider other) // checks if button has entered Pressed and sets onPressed
    {
        if(other.tag == "Button" && !_deadTimeActive)
        {
            onPressed.Invoke();
            Debug.Log("VR Button Pressed");
        }
    }
    private void OnTriggerExit(Collider other) // checks if button has entered Released and sets onReleased
    {
        if(other.tag=="Button" && !_deadTimeActive)
        {
            OnReleased?.Invoke();
            Debug.Log("VR Button Released");
            StartCoroutine(WaitForDeadTime());
        }
    }

    IEnumerator WaitForDeadTime() //Locks Button Press until deadTime times out
    {
        _deadTimeActive = true; Debug.Log("DEADTIME: active");
        yield return new WaitForSeconds(deadTime);
        _deadTimeActive = false; Debug.Log("DEADTIME: inactive");
    }
    // Start is called before the first frame update
    void Start()
    {
        initalLocalPos = visualTarget.localPosition;
    }

}
