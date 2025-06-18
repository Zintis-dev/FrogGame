using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public int Coins { get; private set; } = 500;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            UpdateUI();
            Debug.Log($"Spent {amount} coins. Remaining: {Coins}");
            return true;
        }

        Debug.Log($"Not enough coins! Needed: {amount}, Available: {Coins}");
        return false;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateUI();
        Debug.Log($"Earned {amount} coins. Total: {Coins}");
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = $"{Coins}";
        }
    }
}
