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
    public TMP_FontAsset fontAsset; //Debug

    private Player P1 = new Player();
    private Player P2 = new Player();

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
        //Place a default card in each card slot
        foreach (GameObject tile in CardPoints)
        {
            GameObject newDungeonCard = Instantiate(DungeonCardsTest[0], tile.transform);
            newDungeonCard.transform.rotation = Quaternion.Euler(180, 180, 180);
        }

        P1.SetClass("Crusader");

        //Show character stats in debug overlay
        debugOverlay.text = 
            "Chosen Character: " + P1.Character + "\n" +
            "<#656868><font=\"Icons SDF\">a</font> Armor: </color>" + P1.Armor + "\n" +
            "<#c84d4a><font=\"Icons SDF\">h</font> HP: </color>" + P1.HP + "\n" +
            "<#2a737c><font=\"Icons SDF\">(</font> XP: </color>" + P1.XP + "\n" +
            "<#7d5641><font=\"Icons SDF\">f</font> Food: </color>" + P1.Food + "\n" +
            "<#ce982c><font=\"Icons SDF\">g</font> Gold: </color>" + P1.Gold + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 1: </color>" + P1.potion1 + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 2: </color>" + P1.potion2 + "\n" +
            "<#00bc62><font=\"Icons SDF\">p</font> Poisoned: </color>" + P1.poisoned + "\n" +
            "<#6600bf><font=\"Icons SDF\">c</font> Cursed: </color>" + P1.cursed + "\n";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Debug functions
    string PrintAllChars()
    {
        string allCharacters = "";

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
