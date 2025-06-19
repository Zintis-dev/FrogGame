using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void OnTryAgainButtonClicked()
    {
        EconomyManager.Instance.ResetEconomy();
        SceneManager.LoadScene("MainScene");
    }
}
