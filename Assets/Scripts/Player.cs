using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // Start is called before the first frame update
    public string Character = "none";
    public int Armor = 0;
    public int HP = 0;
    public int XP = 0;
    public int Food = 0;
    public int Gold = 0;
    public string potion1 = "none";
    public string potion2 = "none";
    public bool poisoned = false;
    public bool cursed = false;

    /// <summary>
    /// Set the default values for a specified class
    /// </summary>
    /// <param name="className"></param>
    public void SetClass(string className)
    {
        switch (className)
        {
            case "Crusader":
                Character = className;
                Armor = 1;
                HP = 10;
                XP = 0;
                Food = 4;
                Gold = 0;
                potion1 = "none";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Priestess":
                Character = className;
                Armor = 0;
                HP = 13;
                XP = 0;
                Food = 3;
                Gold = 2;
                potion1 = "Holy";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Rogue":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 3;
                Gold = 5;
                potion1 = "none";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Mage":
                Character = className;
                Armor = 0;
                HP = 11;
                XP = 0;
                Food = 4;
                Gold = 3;
                potion1 = "Perception";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Bones":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 2;
                Food = 2;
                Gold = 0;
                potion1 = "none";
                potion2 = "none";
                poisoned = false;
                cursed = true;
                break;
            case "Cleric":
                Character = className;
                Armor = 1;
                HP = 9;
                XP = 0;
                Food = 2;
                Gold = 0;
                potion1 = "Health";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Thief":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 1;
                Gold = 10;
                potion1 = "Perception";
                potion2 = "none";
                poisoned = false;
                cursed = false;
                break;
            case "Witch":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 3;
                Gold = 3;
                potion1 = "none";
                potion2 = "none";
                poisoned = true;
                cursed = false;
                break;
        }
    }
}
