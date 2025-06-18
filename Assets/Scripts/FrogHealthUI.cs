using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FrogHealthUI : MonoBehaviour
{
    [SerializeField] private HealthComponent frogHealth;
    [SerializeField] private TextMeshProUGUI frogHPText;

    private void Start()
    {
        if (frogHealth == null)
        {
            Debug.LogError("Frog HealthComponent not assigned!");
            return;
        }

        frogHealth.OnHealthChanged += UpdateHPText;
        frogHealth.OnDeath += OnFrogDeath;

        UpdateHPText(frogHealth.GetHealth(), frogHealth.GetMaxHealth());
    }

    private void UpdateHPText(float current, float max)
    {
        frogHPText.text = $"{current}";
    }

    private void OnFrogDeath()
    {
        frogHPText.text = "0";
        Debug.Log("Frog died! Game Over!");
        SceneManager.LoadScene("GameOver");
    }

    private void OnDestroy()
    {
        if (frogHealth != null)
        {
            frogHealth.OnHealthChanged -= UpdateHPText;
            frogHealth.OnDeath -= OnFrogDeath;
        }
    }
}
