using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerChanger : MonoBehaviour
{
    public TMP_InputField myInputField;
    public int targetLayer = 2;

    // Start is called before the first frame update
    void Start()
    {
        myInputField.onSelect.AddListener(ChangeCaretLayer);
    }

    void ChangeCaretLayer(string text)
    {
        //GameObject caret = myInputField.transform.Find(myInputField + "/TextArea/Caret")?.gameObject;
        GameObject caret = GameObject.Find("Caret");
        if (caret != null)
        {
            print("caret found");
            caret.layer = targetLayer;
        }
        else { print("Caret not found"); }
    }

}
