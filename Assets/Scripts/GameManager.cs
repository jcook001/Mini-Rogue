using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] CardPoints = new GameObject[9];
    public GameObject[] DungeonCardsTest = new GameObject[1];

    public TextMeshProUGUI debugOverlay;
    public TMP_FontAsset fontAsset;

    //Player 1 information
    private string P1_Character = "none";
    private int P1_Armor = 0;
    private int P1_HP = 0;
    private int P1_XP = 0;
    private int P1_Food = 0;
    private int P1_Gold = 0;
    private string P1_potion1 = "none";
    private string P1_potion2 = "none";

    /*Icon Map
     * a = Armor
     * b = Ignore Armor
     * c = Curse
     * e = Player Dice 3
     * f = Food
     * g = Gold
     * h = HP/Health
     * i = Merchant
     * k = Perception Potion
     * l = Blindness
     * n = Heal 3
     * p = Poison
     * q = Player Dice (1) Miss
     * r = Player Dice 4
     * s = Skill Check
     * t = Player Dice 5 (crit)
     * v = Nothing happens
     * w = Player Dice 2
     * x = Feat
     * y = Player Dice 6 (crit)
     * B = Blessing
     * C = Curse
     * D = Dungeon Dice
     * F = Fall/Drop
     * G = Circle?
     * H = Health Potion
     * I = Item
     * J = Square border?
     * K = Refresh Skills
     * L = Lore card
     * N = Heal 2
     * P = Poison potion
     * Q = Heal 1
     * R = Perk Point
     * V = Tick
     * W = Weaken
     * X = Exchange
     * Y = Heal 4
     * ( = Crossed Swords (XP)
     * ) = Potion
     * = = Room Card (Door)
     * _ = Floor
     * 1 = Enemy Dice 1
     * 2 = Enemy Dice 2
     * 3 = Enemy Dice 3
     * 4 = Enemy Dice 4
     * 5 = Enemy Dice 5
     * 6 = Enemy Dice 6
     * 7 = Fire potion
     * 8 = Ice Potion
     * 9 = Holy Potion
     * 0 = Dice Blank
    */

    // Start is called before the first frame update
    void Start()
    {
        // Load the font asset
        TMP_FontAsset myFont = Resources.Load<TMP_FontAsset>("Icons SDF");

        //Place a default card in each card slot
        foreach (GameObject tile in CardPoints)
        {
            GameObject newDungeonCard = Instantiate(DungeonCardsTest[0], tile.transform);
            newDungeonCard.transform.rotation = Quaternion.Euler(180, 180, 180);
        }

        //Replace default text with something
        debugOverlay.text = 
            "Chosen Character: " + P1_Character + "\n" +
            "<#656868><font=\"Icons SDF\">a</font> Armor: </color>" + P1_Armor + "\n" +
            "<#c84d4a><font=\"Icons SDF\">h</font> HP: </color>" + P1_HP + "\n" +
            "<#2a737c><font=\"Icons SDF\">(</font> XP: </color>" + P1_XP + "\n" +
            "<#7d5641><font=\"Icons SDF\">f</font> Food: </color>" + P1_Food + "\n" +
            "<#ce982c><font=\"Icons SDF\">g</font> Gold: </color>" + P1_Gold + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 1: </color>" + P1_potion1 + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 2: </color>" + P1_potion2 + "\n";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Debug functions
    string PrintAllChars(string allCharacters)
    {
        // Iterate over all characters in the font asset.
        for (int i = 0; i < fontAsset.characterTable.Count; i++)
        {
            // Get the Unicode representation of the character.
            uint unicode = fontAsset.characterTable[i].unicode;

            // Convert the Unicode to a string.
            string characterString = char.ConvertFromUtf32((int)unicode);

            allCharacters += characterString;
        }

        allCharacters += " \n<font=\"Icons SDF\">";

        for (int i = 0; i < fontAsset.characterTable.Count; i++)
        {
            // Get the Unicode representation of the character.
            uint unicode = fontAsset.characterTable[i].unicode;

            // Convert the Unicode to a string.
            string characterString = char.ConvertFromUtf32((int)unicode);

            allCharacters += characterString;
        }
        
        return allCharacters;
    }
}
