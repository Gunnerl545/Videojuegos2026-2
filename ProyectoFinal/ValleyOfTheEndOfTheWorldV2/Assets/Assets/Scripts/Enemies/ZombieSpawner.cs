using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int zombiesToSpawn = 5;
    public float spawnRadius = 8f;
    private TimeManager timeManager;
    private int zombiesAlive = 0;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    void OnEnable()
    {
        TimeManager.OnDefenseStart += StartSpawning;
    }

    void OnDisable()
    {
        TimeManager.OnDefenseStart -= StartSpawning;
    }

    void StartSpawning()
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        zombiesAlive++;

        zombie.GetComponent<ZombieDeathNotifier>().spawner = this;
    }

    public void OnZombieDied()
    {
        zombiesAlive--;

        if (zombiesAlive <= 0)
        {
            Debug.Log("Todos los zombies muertos");

            timeManager.ForceEndDay();
        }
    }
}