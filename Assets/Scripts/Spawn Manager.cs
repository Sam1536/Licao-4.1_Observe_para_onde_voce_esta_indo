using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private float spawnRange = 9;

    public int enemyCount;
    public int waveNumber = 1;

    public GameObject[] powerupPrefab;
    public int indexPower = 2;

    public GameObject bossPrefab;
    bool bossSpawned = false;
    int bossNumber = 0;



    // Start is called before the first frame update
    void Start()
    {
        Instantiate(powerupPrefab[indexPower], GenerateSpawnPosition(), powerupPrefab[indexPower].transform.rotation);
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {        
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if(enemyCount == 0)
        {
            Instantiate(powerupPrefab[indexPower], GenerateSpawnPosition(), powerupPrefab[indexPower].transform.rotation);
            SpawnEnemyWave(waveNumber);
            waveNumber++;
        }

        if (waveNumber == 3 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    private  Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    void SpawnEnemyWave(int inimigosToSpawn)
    {
        for(int i = 0; i < inimigosToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefab.Length -1);
            Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPosition(), enemyPrefab[enemyIndex].transform.rotation);
            indexPower = Random.Range(0, powerupPrefab.Length);

        }

    }

    void SpawnBoss()
    {
        if (bossNumber < 1)
        {
            Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
            bossNumber++;
        }
        bossSpawned = true;
    }


}
