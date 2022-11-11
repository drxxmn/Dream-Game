using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class RebootTest : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] AudioMixerSnapshot _defaultSnapshot;

    public void ReloadGame()
    {
        _audioMixer.SetFloat("MasterVolume", 0);
        _defaultSnapshot.TransitionTo(0);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
