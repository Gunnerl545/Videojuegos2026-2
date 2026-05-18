using UnityEngine;

public class ZombieDeathNotifier : MonoBehaviour
{
    public ZombieSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            if (SaveSystem.Instance != null &&
                SaveSystem.Instance.isRollingBack)
            {
                return;
            }
            spawner.OnZombieDied();
        }
    }
}