using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // Start is called before the first frame update
    public string Character = "none";
    public int level = 1;
    public int Armor = 0;
    public int HP = 0;
    public int XP = 0;
    public int Food = 0;
    public int Gold = 0;
    public potion potion1 = potion.None;
    public potion potion2 = potion.None;
    public bool isPoisoned = false;
    public bool isCursed = false;

    public enum stat
    {
        Armor,
        Food,
        Gold,
        HP,
        Potion,
        XP
    }

    public enum potion
    {
        None,
        Fire,
        Frost,
        Poison,
        Healing,
        Holy,
        Perception
    }

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
                potion1 = potion.None;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Priestess":
                Character = className;
                Armor = 0;
                HP = 13;
                XP = 0;
                Food = 3;
                Gold = 2;
                potion1 = potion.Holy;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Rogue":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 3;
                Gold = 5;
                potion1 = potion.None;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Mage":
                Character = className;
                Armor = 0;
                HP = 11;
                XP = 0;
                Food = 4;
                Gold = 3;
                potion1 = potion.Perception;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Bones":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 2;
                Food = 2;
                Gold = 0;
                potion1 = potion.None;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = true;
                break;
            case "Cleric":
                Character = className;
                Armor = 1;
                HP = 9;
                XP = 0;
                Food = 2;
                Gold = 0;
                potion1 = potion.Healing;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Thief":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 1;
                Gold = 10;
                potion1 = potion.Perception;
                potion2 = potion.None;
                isPoisoned = false;
                isCursed = false;
                break;
            case "Witch":
                Character = className;
                Armor = 0;
                HP = 10;
                XP = 0;
                Food = 3;
                Gold = 3;
                potion1 = potion.None;
                potion2 = potion.None;
                isPoisoned = true;
                isCursed = false;
                break;
            default:
                Console.Error.WriteLine("Class has not been found! Class is: %s", className);
                break;
        }
    }
}
