using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;

    public PlayerHealth playerHealth;

    void Update()
    {
        slider.maxValue = playerHealth.maxHealth;

        slider.value = playerHealth.GetCurrentHealth();
    }
}