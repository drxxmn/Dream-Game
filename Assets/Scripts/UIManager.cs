using StarterAssets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Put your Pause Menu gameobject here")]
    private GameObject _pauseMenu;

    private void Start()
    {
        if (_pauseMenu == null)
        {
            _pauseMenu = GameObject.Find("Pause Menu");
            if (_pauseMenu == null) Debug.LogError("Pause Menu UI object in UIManager is missing!");
        }
    }

    public void MenuToggle()
    {
        if (!_pauseMenu.activeInHierarchy) ShowMenu();
        else ResumeGame();
    }

    private void ShowMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
