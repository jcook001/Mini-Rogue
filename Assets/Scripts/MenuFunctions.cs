using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    GameObject options;
    int levelIndex;

    // Start is called before the first frame update
    void Start()
    {
        options = GameObject.Find("Options_Manager");
        levelIndex = options.GetComponent<Options>().levelToLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        if (options.GetComponent<Options>().gameTypeDropdown.value > 1) { Debug.LogError("This gamemode is not ready yet!"); return; }
        SceneManager.LoadScene(levelIndex);
    }
}
