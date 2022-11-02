using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(AudioSource))]
public class UISFX : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    [YarnCommand("play_clip")]
    public void PlayClip(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>("SFX/" + clipName);
        if (clip == null)
        {
            Debug.LogError($"Audioclip {clipName} not found!");
            return;
        }
        else _audioSource.PlayOneShot(clip);
    }
}
