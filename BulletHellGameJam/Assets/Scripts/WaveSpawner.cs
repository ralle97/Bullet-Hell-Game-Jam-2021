using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int[] enemyCounts;
        public float rate;
        public int upgradePointsReward;
    }

    private Bounds bounds;

    public GameObject[] waveEnemyPrefabs;
    public GameObject bossEnemyPrefab;

    private int[] waveEnemyCount;

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave; }
    }

    public float timeBetweenWaves = 10f;
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
    private bool bossSpawned = false;

    [SerializeField]
    private GameObject waveCountdownUI;
    [SerializeField]
    private TextMeshProUGUI waveCountdownText;

    [SerializeField]
    private GameObject upgradeMenuUI;

    private AudioManager audioManager;

    [SerializeField]
    private string waveIncomingSound = "WaveIncoming";
    [SerializeField]
    private string bossWaveSound = "BossWave";

    [SerializeField]
    private GameObject waveUI;
    [SerializeField]
    private TextMeshProUGUI waveUIText;

    private PlayerController player;
    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        bounds.center = confiner.m_BoundingShape2D.bounds.center * 3 / 5;
        bounds.extents = confiner.m_BoundingShape2D.bounds.extents * 3 / 5;

        waveCountdown = timeBetweenWaves / 2f;

        searchTimer = searchCountdown;

        waveEnemyCount = new int[waveEnemyPrefabs.Length];

        audioManager = AudioManager.instance;
        gm = GameMaster.instance;

        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossSpawned)
        {
            return;
        }

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

            if (waveCountdown <= 0f)
            {
                if (state != SpawnState.SPAWNING)
                {
                    GameMaster.instance.canUpgrade = false;
                    
                    waveUI.SetActive(true);
                    waveUIText.text = "Wave <color=green>" + (nextWave + 1).ToString() + "</color> Incoming";
                    
                    audioManager.PlaySound(waveIncomingSound);
                    
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
                waveCountdownText.text = "Next Wave in: " + (int)waveCountdown + "s";
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            waveCountdownText.text = "Next Wave in: " + (int)waveCountdown + "s";

            if (waveCountdown <= 0f)
            {
                GameMaster.instance.canUpgrade = false;
                GameMaster.instance.WaveStart();
                
                waveCountdownUI.gameObject.SetActive(false);

                waveUI.SetActive(true);
                waveUIText.fontSize = 42;
                waveUIText.text = "<color=#0060ff>Boss Wave Incoming</color>";

                audioManager.PlaySound(bossWaveSound);

                Instantiate(bossEnemyPrefab, Vector2.zero, Quaternion.identity);
                bossSpawned = true;
            }
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
        if (gm.isGameOver)
        {
            return;
        }

        waveCountdownUI.SetActive(true);

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        waveUI.SetActive(false);

        GameMaster.instance.WaveFinished(waves[nextWave].upgradePointsReward);

        if (nextWave + 1 >= waves.Length)
        {
            bossWave = true;
        }
        else
        {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        GameMaster.instance.WaveStart();
        
        waveCountdownUI.SetActive(false);

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
        float spawnPosX;
        float spawnPosY;
        Vector2 spawnPos;

        while (true)
        {
            spawnPosX = Random.Range((float)-bounds.extents.x, (float)bounds.extents.x);
            spawnPosY = Random.Range((float)-bounds.extents.y, (float)bounds.extents.y);

            spawnPos = new Vector2(spawnPosX, spawnPosY);

            if (Vector2.Distance(spawnPos, player.transform.position) > 1.75f)
            {
                break;
            }
        }
        
        Instantiate(waveEnemyPrefabs[index], spawnPos, Quaternion.identity);
    }
}
