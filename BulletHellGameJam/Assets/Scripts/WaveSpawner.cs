using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    public class Wave
    {
        public string name;
        public int[] enemyCounts;
        public float rate;
    }

    private Bounds bounds;

    public GameObject[] waveEnemyPrefabs;

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave; }
    }

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;
    private float searchTimer;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    private bool bossWave = false;

    // Start is called before the first frame update
    void Start()
    {
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        bounds.center = confiner.m_BoundingShape2D.bounds.center * 3 / 5;
        bounds.extents = confiner.m_BoundingShape2D.bounds.extents * 3 / 5;

        waveCountdown = timeBetweenWaves;

        searchTimer = searchCountdown;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private bool EnemyIsAlive()
    {
        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0f)
        {
            searchTimer = searchCountdown;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 >= waves.Length)
        {
            // TODO: Change to spawn boss wave
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);
        
        state = SpawnState.SPAWNING;

        int totalEnemyCount = 0;

        for (int i = 0; i < wave.enemyCounts.Length; i++)
        {
            totalEnemyCount += wave.enemyCounts[i];
        }

        for (int i = 0; i < totalEnemyCount; i++)
        {
            int index = Random.Range(0, wave.enemyCounts.Length);

            while (wave.enemyCounts[index] <= 0)
            {
                index = Random.Range(0, wave.enemyCounts.Length);
            }

            SpawnEnemy(index);
            wave.enemyCounts[index]--;

            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(int index)
    {
        float spawnPosX = Random.Range((float)-bounds.extents.x, (float)bounds.extents.x);
        float spawnPosY = Random.Range((float)-bounds.extents.y, (float)bounds.extents.y);

        Vector2 spawnPos = new Vector2(spawnPosX, spawnPosY);

        Instantiate(waveEnemyPrefabs[index], spawnPos, Quaternion.identity);
    }
}
