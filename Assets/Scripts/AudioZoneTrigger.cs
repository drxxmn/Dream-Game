using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider))]
public class AudioZoneTrigger : MonoBehaviour
{
    [SerializeField] AudioMixerSnapshot _snapshotIn;
    [SerializeField] AudioMixerSnapshot _snapshotOut;

    [SerializeField] float _transitionIn = 1.5f;
    [SerializeField] float _transitionOut = 2.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _snapshotIn.TransitionTo(_transitionIn);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _snapshotOut.TransitionTo(_transitionOut);
        }
    }
}
