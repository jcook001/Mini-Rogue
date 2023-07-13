using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Options : MonoBehaviour
{
    public int levelToLoad = 1;

    //Player character selection
    public string[] characters = { "Crusader", "Priestess", "Rogue", "Mage", "Bones", "Cleric", "Theif", "Witch" };
    public Sprite[] characterImages = new Sprite[8];
    public int P1CharacterChoice = 0;
    public TextMeshProUGUI P1ClassNameText;
    //public GameObject P1ImageObject;
    public UnityEngine.UI.Image P1Image;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        //P1Image = P1ImageObject.GetComponent<UnityEngine.UI.Image>();

        UpdateP1Character();

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

    private void UpdateP1Character()
    {
        P1ClassNameText.text = characters[P1CharacterChoice];
        P1Image.sprite = characterImages[P1CharacterChoice];
    }
}
