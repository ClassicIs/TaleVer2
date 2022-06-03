using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSortScript : MonoBehaviour
{
    [SerializeField]
    private int spriteBase = 50000;
    [SerializeField]
    private float offset;
    private Renderer[] renderer;
    private bool sortOnlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        spriteBase = 5000;
        renderer = GetComponentsInChildren<Renderer>();
        if(transform.CompareTag("Player") || transform.CompareTag("Enemy"))
        {
            sortOnlyOnce = false;
            offset = 0.7f;
        }
        else
        {
            sortOnlyOnce = true;
            offset = 0;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (Renderer rend in renderer)
        {
            rend.sortingOrder = (int)(spriteBase - transform.position.y - offset);
            if (sortOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
