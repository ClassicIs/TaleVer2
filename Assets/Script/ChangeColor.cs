using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Renderer theRend;
    public Color colorToTurnTo = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        theRend = GetComponent<Renderer>();
        theRend.material.color = colorToTurnTo;
    }
}
