using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class OutroScript : MonoBehaviour
{
    [SerializeField] float _secondsToReset = 6.0f;
    [SerializeField] AudioMixerSnapshot _defaultSnapshot;

    [YarnCommand("start_outro")]
    public void StartOutro()
    {
        GetComponent<PlayableDirector>().Play();
        _defaultSnapshot.TransitionTo(_secondsToReset - .5f);
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(_secondsToReset);
        SceneManager.LoadScene(0);
    }
}
