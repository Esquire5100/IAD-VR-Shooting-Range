using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("SpawnPoints")]
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [Header("Players")]
    public GameObject Player;

    [Header("Enemies")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    private List<enemyController> activeEnemies = new List<enemyController>();

    [Header("Spawn Interval")]
    public float baseInterval = 3f;
    public float minInterval = 0.3f;
    public float decayRate = 0.95f;      // exponential scaling

    [Header("Enemy Count")]
    public int baseGroupSize = 1;
    public int maxGroupSize = 5;
    public float groupSizeScaleTime = 150f; // seconds until max group size

    [Header("Settings")]
    [SerializeField] private int maxActiveEnemies;
    [SerializeField] private float gracePeriod;
    public float minSpawnDistanceFromPlayers;

    private bool isRunning;
    [HideInInspector] public float elapsedTime;
    private float timeSinceLastSpawn;

    public GameObject endGame;

    private void Start()
    {
        // Set up all the spawn points in the scene and target the player for the enemies to chase
        spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).ToList();

        if (spawnPoints.Count == 0)
            Debug.LogWarning("SpawnManager: No SpawnPoints found in scene!");

        Player = GameObject.FindGameObjectWithTag("MainCamera");

        StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) { return; }

        elapsedTime += Time.deltaTime;

        if (elapsedTime < gracePeriod) return;

        // Check if it's time to spawn a new group of enemies based on the current interval
        timeSinceLastSpawn += Time.deltaTime;
        float currentInterval = GetInterval(elapsedTime - gracePeriod);

        if (timeSinceLastSpawn >= currentInterval)
        {
            StartCoroutine(TrySpawnGroup());
            timeSinceLastSpawn = 0f;
        }

    }

    public void StartSpawning() { isRunning = true; }
    public void StopSpawning() { isRunning = false; }

    public float GetInterval(float elapsedTime)
    {
        float interval = baseInterval * Mathf.Pow(decayRate, elapsedTime);
        return Mathf.Max(minInterval, interval);
    }

    // Returns the current group size based on elapsed time, scaling from baseGroupSize to maxGroupSize over groupSizeScaleTime seconds
    public int GetGroupSize(float elapsedTime)
    {
        float t = Mathf.Clamp01(elapsedTime / groupSizeScaleTime);
        return Mathf.RoundToInt(Mathf.Lerp(baseGroupSize, maxGroupSize, t));
    }

    private IEnumerator TrySpawnGroup() // Attempt to spawn a group of enemies based on the current group size
    {
        int groupSize = GetGroupSize(elapsedTime);

        for (int i = 0; i < groupSize; i++)
        {
            if (activeEnemies.Count >= maxActiveEnemies) break;

            SpawnPoint point = ChooseSpawnPoint();
            if (point == null) continue;

            SpawnEnemy(point.transform);
            yield return null;
        }
    }

    private SpawnPoint ChooseSpawnPoint()
    {
        // Choose a random spawn point from the list
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
        
        //List<SpawnPoint> eligibleSpawnPoints = spawnPoints
        //    .Where(p => Vector3.Distance(p.transform.position, Player.transform.position) >= minSpawnDistanceFromPlayers ||
        //    Vector3.Distance(p.transform.position, Player.transform.position) >= minSpawnDistanceFromPlayers)
        //    .ToList();

        //if (eligibleSpawnPoints.Count == 0) return spawnPoints[0]; // fallback

        //return eligibleSpawnPoints[Random.Range(0, eligibleSpawnPoints.Count)];
    }
    private enemyController RandomEnemyPicker() // LETS GO GAMBLING BRRRRRR
    {
        float roll = Random.value;

        if (roll < 0.5f)
        {
            return enemy1.GetComponent<enemyController>();
        }

        else
        {
            // 50% chance to pick between enemy2 and enemy3 which have higher health
            if (roll < 0.75f)
            {
                return enemy2.GetComponent<enemyController>();
            }

            else
            {
                return enemy3.GetComponent<enemyController>();
            }
        }
    }
    private void SpawnEnemy(Transform point)
    {
        // Instantiate the enemy prefab at the spawn point
        GameObject enemy = Instantiate(RandomEnemyPicker().gameObject, point.position, Quaternion.identity);
        enemyController enemyController = enemy.GetComponent<enemyController>();

        // Set the target for the enemy to the player
        enemyController.target = Player.transform;

        enemy.transform.position += Vector3.up * enemyController.agent.baseOffset;

        enemyController.OnDied += HandleEnemyDied;

        // Add the enemy to the active list
        activeEnemies.Add(enemyController);
    }

    private void HandleEnemyDied(enemyController enemy)
    {
        // Unsubscribe from the event and remove the enemy from the active list
        enemy.OnDied -= HandleEnemyDied;
        activeEnemies.Remove(enemy);
    }

    public void GameEnd()
    {
        // Stop spawning enemies and clear all the ones on the scene
        StopSpawning();
        foreach (enemyController enemy in activeEnemies)
        {
            Destroy(enemy.gameObject);
        }

        activeEnemies.Clear();

        // Show the end game UI and load the main menu after a delay
        endGame.SetActive(true);
        new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
