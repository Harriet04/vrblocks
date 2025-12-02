using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Button_Reciver : MonoBehaviour
{
    public string scriptPath = "Assets/Blocks/Scripts/Debug Call.cs";
    public void RespondToEvent()
    {

        Debug.Log("CALL RECIVED! Running: "+ scriptPath);
    }

}
