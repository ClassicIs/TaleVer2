using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    public event EventHandler OnBattleStarted;
    public event EventHandler OnBattleOver;

    private enum State
    {
        Idle,
        Active,
        BattleOver,
    }

    [SerializeField]
    private ColliderTrigger colliderTrigger;

    [SerializeField]
    private Wave[] waveArray;

    private State state;

    private void Awake()
    {
        state = State.Idle;
    }


    // Start is called before the first frame update
    void Start()
    {
        colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
    }

    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        if (state == State.Idle)
        {
            StartBattle();
            colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
        }
    }

    private void StartBattle()
    {
        Debug.Log("Battle starts");
        state = State.Active;
        OnBattleStarted?.Invoke(this, EventArgs.Empty);
    }


    private void Update()
    {
        switch (state)
        {
            case State.Active:

                foreach (Wave wave in waveArray)
                {
                    wave.Update();
                }

                TestBattleOver();
                break;
        }
    }

    private void TestBattleOver()
    {
        if (state == State.Active)
        {
            if (AreWavesOver())
            {
                // ивент закончился
                state = State.BattleOver;
                Debug.Log("Battle is Over");
                OnBattleOver?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool AreWavesOver()
    {
        foreach (Wave wave in waveArray)
        {
            if (wave.IsWaveOver())
            {
                // волна закончилась
            }

            else
            {
                // волна не закончилась
                return false;
            }
        }

        return true;
    }



    [System.Serializable]
    private class Wave
    {
        [SerializeField] private EnemySpawn[] enemySpawnArray;
        [SerializeField] private float timer;

        public void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    SpawnEnemies();
                }
            }
        }

        private void SpawnEnemies()
        {
            foreach (EnemySpawn enemySpawn in enemySpawnArray)
            {
                enemySpawn.Spawn();
            }
        }

        public bool IsWaveOver()
        {
            if (timer < 0)
            {
                // волна заспавнилась
                foreach (EnemySpawn enemySpawn in enemySpawnArray)
                {
                    if (enemySpawn.IsAlive())
                    {
                        return false;
                    }
                }
                return true;
            }

            else
            {
                // враги не заспавнились
                return false;
            }
        }
    }

}
