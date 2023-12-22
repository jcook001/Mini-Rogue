using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    int levelIndex;

    // Start is called before the first frame update
    void Start()
    {
        levelIndex = Options.Instance.levelToLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        if (Options.Instance.gameTypeDropdown.value > 1) { Debug.LogError("This gamemode is not ready yet!"); return; }
        if (Options.Instance.chosenLevel == Options.level.coopDungeon || Options.Instance.chosenLevel == Options.level.coopTower) 
            { Debug.LogError("This gamemode is not ready yet!"); return; }

        SceneManager.LoadScene(levelIndex);
    }
}
