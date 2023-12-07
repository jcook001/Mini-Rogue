using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardAnims : MonoBehaviour
{
    private GameManager gameManager;
    public bool isZoomed = false;
    public bool isFaceUp = false;
    public bool isFlipping = false;
    public Material[] cardMaterials;
    float[] originalTransparency;
    private bool IsTransparent = false;
    private bool IsFading = false;
    private float fadeDuration = 1.0f; // Duration for the fade effect

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Renderer cardRenderer = this.GetComponent<Renderer>();
        cardMaterials = cardRenderer.materials; // Get all materials

        // Store original transparency of each material
        originalTransparency = new float[cardMaterials.Length];
        for (int i = 0; i < cardMaterials.Length; i++)
        {
            originalTransparency[i] = cardMaterials[i].color.a;
        }
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

    public IEnumerator FadeCardToTransparency(float targetTransparency)
    {
        if(IsFading || IsTransparent) { yield break; }
        IsFading = true;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(originalTransparency[0], targetTransparency, elapsedTime / fadeDuration);
            foreach (Material mat in cardMaterials)
            {
                if (mat.HasProperty("_BaseColor"))
                {
                    Color newColor = mat.GetColor("_BaseColor");
                    newColor.a = currentAlpha;
                    mat.SetColor("_BaseColor", newColor);
                }
                else
                {
                    Debug.LogError("Material " + mat + "in card " + this.gameObject.name + "has no _BaseColor property");
                }
            }

            yield return null;
        }
        IsFading = false;
        IsTransparent = true;
    }
}
