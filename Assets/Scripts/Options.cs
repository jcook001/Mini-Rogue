using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Options : MonoBehaviour
{
    public static Options Instance { get; private set; } // Static property to access the instance

    public int levelToLoad = 1;
    public level chosenLevel = level.soloDungeon;

    //Player character selection
    public string[] characters = { "Crusader", "Priestess", "Rogue", "Mage", "Bones", "Cleric", "Theif", "Witch" };
    public Sprite[] characterImages = new Sprite[8];
    public int P1CharacterChoice = 0;
    public int P2CharacterChoice = 1;
    public TextMeshProUGUI P1ClassNameText;
    public TextMeshProUGUI P2ClassNameText;
    public UnityEngine.UI.Image P1Image;
    public UnityEngine.UI.Image P2Image;
    public TMP_Dropdown gameTypeDropdown;
    public GameObject player2UI;

    public enum level
    {
        soloDungeon,
        soloTower,
        soloCampaign,
        coopDungeon,
        coopTower
    }

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

#if PLATFORM_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        UpdateP1Character();
        UpdateP2Character();

        gameTypeDropdown.onValueChanged.AddListener(delegate { GameTypeChanged(gameTypeDropdown); });

        GameTypeChanged(gameTypeDropdown);

    }

    public void P1CharacterNext()
    {
        //update the character choice index
        if (P1CharacterChoice >= characters.Length -1)
        {
            P1CharacterChoice = 0;
        }
        else
        {
            P1CharacterChoice += 1;
        }

        UpdateP1Character();
    }

    public void P1CharacterPrev()
    {
        //update the character choice index
        if (P1CharacterChoice <= 0)
        {
            P1CharacterChoice = characters.Length -1;
        }
        else
        {
            P1CharacterChoice -= 1;
        }

        UpdateP1Character();
    }

    public void P2CharacterNext()
    {
        //update the character choice index
        if (P2CharacterChoice >= characters.Length - 1)
        {
            P2CharacterChoice = 0;
        }
        else
        {
            P2CharacterChoice += 1;
        }

        UpdateP2Character();
    }

    public void P2CharacterPrev()
    {
        //update the character choice index
        if (P2CharacterChoice <= 0)
        {
            P2CharacterChoice = characters.Length - 1;
        }
        else
        {
            P2CharacterChoice -= 1;
        }

        UpdateP2Character();
    }

    private void UpdateP1Character()
    {
        P1ClassNameText.text = characters[P1CharacterChoice];
        P1Image.sprite = characterImages[P1CharacterChoice];
    }

    private void UpdateP2Character()
    {
        P2ClassNameText.text = characters[P2CharacterChoice];
        P2Image.sprite = characterImages[P2CharacterChoice];
    }

    void GameTypeChanged(TMP_Dropdown change)
    {
        //Show or hide player 2 options
        switch (change.value)
        {
            case 0:
            case 1:
            case 2:
                if (player2UI.activeSelf)
                {
                    player2UI.SetActive(false);
                }
                break;

            case 3:
            case 4:
                if (!player2UI.activeSelf)
                {
                    player2UI.SetActive(true);
                }
                break;
        }

        //Set Level type
        switch (change.value)
        {
            case 0:
                chosenLevel = level.soloDungeon;
                levelToLoad = 1;
                break;
            case 1:
                chosenLevel = level.soloTower;
                levelToLoad = 1;
                break;
            case 2:
                chosenLevel = level.soloCampaign;
                levelToLoad = 2;
                break;
            case 3:
                chosenLevel = level.coopDungeon;
                levelToLoad = 1;
                break;
            case 4:
                chosenLevel = level.coopTower;
                levelToLoad = 1;
                break;
        }
    }
}
