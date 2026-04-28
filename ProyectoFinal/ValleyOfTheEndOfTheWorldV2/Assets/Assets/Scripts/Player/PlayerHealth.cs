using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Vida: " + currentHealth);
        Debug.Log("Jugador recibió daño. Vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jugador murió → rollback");

        // eliminar todos los zombies
        Zombie[] zombies = FindObjectsOfType<Zombie>();

        foreach (Zombie z in zombies)
        {
            Destroy(z.gameObject);
        }

        // rollback
        SaveSystem.Instance.LoadGame();

        currentHealth = maxHealth;
    }
}