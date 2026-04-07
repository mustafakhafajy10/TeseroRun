using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the slider component
    public Image fillImage; // Reference to the fill image

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        // Change color based on health percentage
        if (health > slider.maxValue * 0.5f)
        {
            fillImage.color = Color.green; // Healthy
        }
        else if (health > slider.maxValue * 0.25f)
        {
            fillImage.color = Color.yellow; // Medium health
        }
        else
        {
            fillImage.color = Color.red; // Low health
        }
    }
}
