using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    Animator theBoxAnim;
    public string [] contentOfBox;
    private CircleCollider2D theColliderBox;
    private void Start()
    {
        theColliderBox = GetComponent<CircleCollider2D>();
        theBoxAnim = GetComponent<Animator>();
    }

    public void OpenTheBox()
    {
        theColliderBox.enabled = false;
        Debug.Log("The box is openned!");
    }
    
}
