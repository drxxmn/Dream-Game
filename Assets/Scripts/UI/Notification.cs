using System.Collections;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class Notification : MonoBehaviour
{
    [SerializeField] float _displayDuration = 5.0f;
    Animator _animator;
    TextMeshProUGUI _text;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    [YarnCommand("display_notification")]
    public void DisplayNotification(string text)
    {
        _text.text = text;
        _animator.SetBool("visible", true);
        StartCoroutine(DisplayTimer());
    }

    IEnumerator DisplayTimer()
    {
        yield return new WaitForSeconds(_displayDuration);
        _animator.SetBool("visible", false);
    }
}
