using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
