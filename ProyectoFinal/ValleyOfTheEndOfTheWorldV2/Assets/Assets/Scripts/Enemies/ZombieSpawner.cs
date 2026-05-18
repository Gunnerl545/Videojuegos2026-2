using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Evita spawnear varias veces el mismo día
    private bool hasSpawnedToday = false;

    // Prefab del zombie
    public GameObject flankerZombiePrefab;
    public GameObject zombiePrefab;

    // Cantidad de zombies por noche
    public int zombiesToSpawn = 5;

    // Radio de spawn
    public float spawnRadius = 8f;

    // Referencia al TimeManager
    private TimeManager timeManager;

    // Zombies vivos actualmente
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

    // 🌙 Iniciar spawn de zombies
    void StartSpawning()
    {
        // Evitar doble spawn
        if (hasSpawnedToday)
        {
            return;
        }

        hasSpawnedToday = true;

        Debug.Log("🧟 Spawneando zombies");

        zombiesAlive = 0;

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
        }
        int flankersToSpawn =
            Random.Range(1, 4);

        for (int i = 0; i < flankersToSpawn; i++)
        {
            SpawnFlankerZombie();
        }
    }

    void SpawnFlankerZombie()
    {
        Vector2 spawnPos =
            (Vector2)transform.position +
            Random.insideUnitCircle * spawnRadius;

        GameObject zombie =
            Instantiate(
                flankerZombiePrefab,
                spawnPos,
                Quaternion.identity);

        zombiesAlive++;

        ZombieDeathNotifier notifier =
            zombie.GetComponent<ZombieDeathNotifier>();

        if (notifier != null)
        {
            notifier.spawner = this;
        }
    }

    // Crear zombie
    void SpawnZombie()
    {
        Vector2 spawnPos =
            (Vector2)transform.position +
            Random.insideUnitCircle * spawnRadius;

        GameObject zombie =
            Instantiate(
                zombiePrefab,
                spawnPos,
                Quaternion.identity);

        zombiesAlive++;

        ZombieDeathNotifier notifier =
            zombie.GetComponent<ZombieDeathNotifier>();

        if (notifier != null)
        {
            notifier.spawner = this;
        }
    }

    // ☠️ Zombie muerto
    public void OnZombieDied()
    {
        zombiesAlive--;

        Debug.Log("Zombie muerto. Restantes: " + zombiesAlive);

        if (zombiesAlive <= 0)
        {
            Debug.Log("🌅 Todos los zombies muertos");

            timeManager.ForceEndDay();
        }
    }

    // 🔁 Reiniciar spawner para nuevo día
    public void ResetSpawner()
    {
        hasSpawnedToday = false;

        zombiesAlive = 0;

        Debug.Log("🔁 Spawner reiniciado");
    }
}