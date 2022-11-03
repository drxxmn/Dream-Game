using UnityEngine;
using UnityEngine.SceneManagement;

public class RebootTest : MonoBehaviour
{
    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
