using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    private SpriteRenderer InterObjSpriteRend;
    // Start is called before the first frame update
    void Start()
    {
        InterObjSpriteRend = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeAlpha());
    }

    IEnumerator ChangeAlpha()
    {
        float ChangeCol = -1;
        while (true)
        {
            if (InterObjSpriteRend.color.b == 1)
            {
                ChangeCol = -1;
            }
            else if (InterObjSpriteRend.color.b <= 0)
            {
                ChangeCol = 1;
            }

            InterObjSpriteRend.color = new Color(InterObjSpriteRend.color.r, InterObjSpriteRend.color.g, InterObjSpriteRend.color.b + 0.05f * ChangeCol);
            yield return null;
        }
    }
}
