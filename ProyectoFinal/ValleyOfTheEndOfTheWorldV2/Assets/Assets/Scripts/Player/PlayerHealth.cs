using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("❤️ Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Morir
    void Die()
    {
        Debug.Log("☠️ Jugador murió");

        // Destruir zombies normales
        Zombie[] zombies =
            FindObjectsOfType<Zombie>();

        foreach (Zombie z in zombies)
        {
            Destroy(z.gameObject);
        }

        // Destruir flankers
        FlankerZombie[] flankers =
            FindObjectsOfType<FlankerZombie>();

        foreach (FlankerZombie f in flankers)
        {
            Destroy(f.gameObject);
        }

        // Rollback
        SaveSystem.Instance.LoadGame();
    }

    // 🔁 Restaurar vida completa
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;

        Debug.Log("💚 Vida restaurada");
    }

    // Obtener vida actual (útil para UI)
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Evitar pasar vida máxima
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("💚 Vida curada: " + currentHealth);
    }
}