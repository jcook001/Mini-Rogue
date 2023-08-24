using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardAnims : MonoBehaviour
{
    private GameManager gameManager;
    public bool isCardZoomed = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // Scroll down
        {
            gameManager.ZoomOut(this.gameObject);
        }
    }

    //left click
    public void OnMouseDown()
    {
        //Flip the card
        gameManager.FlipCard(this.gameObject, 1.0f, 1.0f);
        
    }

    
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            gameManager.MoveActivePieceToCard(this.gameObject); 
        }
        else if (Input.GetMouseButtonDown(2)) // Middle click
        {
            if (gameManager.isAnyCardZoomed)
            {
                gameManager.ZoomOut(this.gameObject);
            }
            else
            {
                gameManager.ZoomIn(this.gameObject);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0) // Scroll up
        {
            gameManager.ZoomIn(this.gameObject);
        }
    }
}
