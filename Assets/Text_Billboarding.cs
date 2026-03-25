using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Billboarding : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;

    public enum BillboardType { LookAtCamera, CameraForward}; //options for billboard method
    
    //late update to wait until everything finished moving before running
    private void LateUpdate()
    {
        //switch cases for billboard methods
        switch (billboardType)
        {
            case BillboardType.LookAtCamera: //code for "LookAtCamera" function
                transform.LookAt(Camera.main.transform.position, Vector3.up); //aims text at camera position
                break;
            case BillboardType.CameraForward: //code for "LookAtCamera" function
                transform.forward = Camera.main.transform.forward; //makes text parallel to camera forward
                break;
            default:
                break;
        }
    }
}
