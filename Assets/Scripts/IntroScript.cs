using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class IntroScript : MonoBehaviour
{
    [SerializeField] float _secondsToMusic = 14.0f;
    [SerializeField] float _fade = 2.0f;
    [SerializeField] AudioMixerSnapshot _forestSnapshot;

    void Start()
    {
        StartCoroutine(FireUpMusic());
    }

    IEnumerator FireUpMusic()
    {
        yield return new WaitForSeconds(_secondsToMusic);
        _forestSnapshot.TransitionTo(_fade);
    }
}
