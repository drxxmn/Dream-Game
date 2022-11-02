using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISFX : MonoBehaviour
{
    public AudioClip OnClickClip;
    public AudioClip PauseMenuOpenClip;
    public AudioClip PauseMenuCloseClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnClick()
    {
        _audioSource.PlayOneShot(OnClickClip);
    }

    public void PlayPauseMenuOpen()
    {
        _audioSource.PlayOneShot(PauseMenuOpenClip);
    }

    public void PlayPauseMenuClose()
    {
        _audioSource.PlayOneShot(PauseMenuCloseClip);
    }
}
