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
    private float fadeDuration = 0.5f; // Duration for the fade effect
    private Coroutine fadeCoroutine;
    private bool isFadingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Renderer cardRenderer = this.GetComponent<Renderer>();
        cardMaterials = cardRenderer.materials; // Get all materials
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMouseInGameWindow())
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0) // Scroll down
            {
                gameManager.ZoomOut(this.gameObject);
            }
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

    bool IsMouseInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
               mousePosition.y >= 0 && mousePosition.y <= Screen.height;
    }


    public void FadeOut()
    {
        if (!isFadingOut)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeToTransparency(0f, false));
            isFadingOut = true;
        }
    }

    public void FadeIn()
    {
        if (isFadingOut)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeToTransparency(1.0f, true));
            isFadingOut = false;
        }
    }

    IEnumerator FadeToTransparency(float targetTransparency, bool fadeIn)
    {
        float elapsedTime = 0;
        float startAlpha = fadeIn ? cardMaterials[0].color.a : 1.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetTransparency, elapsedTime / fadeDuration);

            foreach (Material mat in cardMaterials)
            {
                Color newColor = mat.color;
                newColor.a = alpha;
                mat.color = newColor;
            }

            yield return null;
        }
    }
}
