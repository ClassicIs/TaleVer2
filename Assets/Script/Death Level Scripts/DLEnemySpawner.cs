using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLEnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float enemySpawnInterval = 3f;

    [SerializeField]
    private int curEnemyCount = 0;

    [SerializeField]
    private int maxEnemyCount = 7;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnEnemy(enemySpawnInterval, enemyPrefab));
    }

    private void Update()
    {
        if (curEnemyCount == maxEnemyCount)
        {
            //StopCoroutine(SpawnEnemy(enemySpawnInterval, enemyPrefab));
            StopAllCoroutines();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(SpawnEnemy(enemySpawnInterval, enemyPrefab));
        }
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(transform.position.x - 5f, transform.position.x + 5f), Random.Range(transform.position.y - 3f, transform.position.y + 3f), 0), Quaternion.identity);
        //StartCoroutine(SpawnEnemy(interval, enemy));
        curEnemyCount += 1;
    }
}
