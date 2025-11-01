using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void SetColorOriginal()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
    public void SetColorHighlight()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
