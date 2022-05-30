using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private int health = 100;       // надо поменять

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public bool IsAlive()
    {
        if (health > 0)
        {
            return true;
        }

        else return false;
    }
}
