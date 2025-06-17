using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public int Coins { get; private set; } = 500;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            Debug.Log($"Spent {amount} coins. Remaining: {Coins}");
            return true;
        }
        Debug.Log($"Not enough coins! Needed: {amount}, Available: {Coins}");
        return false;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log($"Earned {amount} coins. Total: {Coins}");
    }
}
