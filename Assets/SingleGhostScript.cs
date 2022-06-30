using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGhostScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void BeAGhost(Sprite GhostSprite, Color GhostColor, string name = "Ghost")
    {

        spriteRenderer.sprite = GhostSprite;
        spriteRenderer.color = GhostColor;
        transform.name = name;
        StartCoroutine(GhostTime(GhostColor));
    }

    private IEnumerator GhostTime(Color startColor)
    {
        Color tmpColor = startColor;
        while(tmpColor.a > 0.1f)
        {
            tmpColor.a -= 0.1f;
            spriteRenderer.color = tmpColor;
            //Debug.LogFormat("Ghost {0} is stiling trying to die.", transform.name);
            yield return null;
        }
        tmpColor.a = 0;
        spriteRenderer.color = tmpColor;
        Destroy(gameObject);
    }
}
