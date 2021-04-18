using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int[] enemyCounts;
        public float rate;
    }

    private Bounds bounds;

    public GameObject[] waveEnemyPrefabs;

    private int[] waveEnemyCount;

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

        waveEnemyCount = new int[waveEnemyPrefabs.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossWave)
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
        else
        {
            // TODO: Summon boss
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
            // TODO: Finish game for now; add boss fight
            bossWave = true;
            GameMaster.instance.gameFinished = true;
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
            waveEnemyCount[i] = wave.enemyCounts[i];
            totalEnemyCount += wave.enemyCounts[i];
        }

        for (int i = 0; i < totalEnemyCount; i++)
        {
            int index = Random.Range(0, waveEnemyCount.Length);

            while (waveEnemyCount[index] <= 0)
            {
                index = Random.Range(0, waveEnemyCount.Length);
            }

            SpawnEnemy(index);
            waveEnemyCount[index]--;

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
