using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithChance : MonoBehaviour
{

    [Range(0, 1)]
    public float chanceOfStaying = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > chanceOfStaying) Destroy(gameObject);
    }
}
