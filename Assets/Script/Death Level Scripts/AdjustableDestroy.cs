using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustableDestroy : MonoBehaviour
{
    [SerializeField]
    private int countToLeave;

    // Start is called before the first frame update
    void Start()
    {
        while(transform.childCount > countToLeave)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
