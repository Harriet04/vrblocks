using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    public CodingModeSettings CodingModeSettings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Call()
    {
        if(CodingModeSettings.CodingMode == 0)// if mode is normal, switch to simple
        {
            CodingModeSettings.SetModeSimple();
        }
        else if (CodingModeSettings.CodingMode == 1)   //else switch to normal
        {
            CodingModeSettings.SetModeNormal();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
