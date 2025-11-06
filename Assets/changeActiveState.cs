using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeActiveState : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {

      
    }

    public void changeButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
