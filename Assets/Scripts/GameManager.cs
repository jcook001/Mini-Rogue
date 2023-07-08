using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] CardPoints = new GameObject[9];
    public GameObject[] DungeonCardsTest = new GameObject[1];

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject tile in CardPoints)
        {
            GameObject newDungeonCard = Instantiate(DungeonCardsTest[0], tile.transform);
            newDungeonCard.transform.rotation = Quaternion.Euler(180,180,180);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
