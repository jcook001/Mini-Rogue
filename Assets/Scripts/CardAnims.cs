using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnims : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //left click
    public void OnMouseDown()
    {
        //Flip the card
        gameManager.FlipCard(this.gameObject, 1.0f, 1.0f);
        
    }

    //right click
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameManager.MoveActivePieceToCard(this.gameObject);
        }
        else if (Input.GetMouseButtonDown(2))
        {
            gameManager.Zoom(this.gameObject);
        }
    }
}
