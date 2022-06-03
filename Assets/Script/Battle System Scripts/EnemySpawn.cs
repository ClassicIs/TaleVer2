using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private int health;       // надо поменять
    public event Action OnDeath;

    private void Awake()
    {
        health = 0;
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
