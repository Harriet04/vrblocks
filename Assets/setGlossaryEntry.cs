using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGlossaryEntry : MonoBehaviour
{

    //string currentEntry = "MoveForward";
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void onEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Debug.Log(child);
            //if (child.gameObject.CompareTag(currentEntry))
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    void onDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            //if (child.gameObject.CompareTag(currentEntry))
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
