using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnims : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        this.transform.rotation = Quaternion.Euler(0, 180, 180);
    }
}
