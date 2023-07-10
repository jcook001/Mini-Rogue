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

    public void OnMouseDown()
    {
        //Flip the card
        //this.transform.Rotate(0, 0, 180);

        gameManager.MoveActivePieceToCard(this.gameObject);
    }
}
