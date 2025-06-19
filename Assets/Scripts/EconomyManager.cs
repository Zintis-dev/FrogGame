using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    private int startCoinAmount = 500;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject coinsGO = GameObject.FindWithTag("CoinsText");
        if (coinsGO != null)
        {
            coinsText = coinsGO.GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }
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

    public void ResetEconomy()
    {
        Coins = startCoinAmount;
        UpdateUI();
        Debug.Log($"Economy reset. Coins: {Coins}");
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = Coins.ToString();
        }
    }
}
