using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGlossaryEntry : MonoBehaviour
{
    public string currentEntry = "";
    // Start is called before the first frame update
    void Start()
    {


    }

    public void changeEntry(string entry)
    {
        currentEntry = entry;
    }

    public void updateEntry()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.CompareTag(currentEntry))
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
