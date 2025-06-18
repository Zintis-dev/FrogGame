using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
