using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayGameFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Loads by name
    }

    public void LoadByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex); // Loads by build index
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}