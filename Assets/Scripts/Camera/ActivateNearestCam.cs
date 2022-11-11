using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ActivateNearestCam : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _playerCamera;
    [SerializeField] GameObject _initTrigger;
    [SerializeField] GameObject _triggerContainer;
    Dictionary<GameObject, Vector3> _triggerPositions = new();
    List<Quaternion> _rotationQueue = new();
    float checkTime = 2f;
    GameObject _nearestTrigger;
    GameObject _prevTrigger;
    Quaternion _prevRotation;
    float _lerpSpeed = 0.8f;
    float _timeCount = 0.0f;
    bool _rotating = false;

    void Start()
    {
        _nearestTrigger = _initTrigger;
        _prevTrigger = _initTrigger;

        foreach (Transform child in _triggerContainer.transform)
        {
            _triggerPositions.Add(child.gameObject, child.transform.position);
        }

        _prevRotation = new Quaternion(
                    _playerCamera.transform.rotation.x,
                    _playerCamera.transform.rotation.y,
                    _playerCamera.transform.rotation.z,
                    _playerCamera.transform.rotation.w
        );

        StartCoroutine(CheckDistanceToPlayer());
    }

    void Update()
    {
        if (_rotationQueue.Count > 0)
        {
            if (_rotating)
            {
                _playerCamera.transform.rotation = Quaternion.Slerp(_prevRotation, _rotationQueue[0], _timeCount * _lerpSpeed);
                _timeCount = _timeCount + Time.deltaTime;
            }
            else
            {
                StartCoroutine(RotateTimer());
            }
        }
    }

    IEnumerator CheckDistanceToPlayer()
    {
        while (enabled)
        {
            float closestDistance = Mathf.Infinity;

            foreach (KeyValuePair<GameObject, Vector3> trigger in _triggerPositions)
            {
                float distanceToPlayer = Vector3.Distance(trigger.Value, _player.transform.position);
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    _nearestTrigger = trigger.Key;
                }
            }

            if (_nearestTrigger != _prevTrigger)
            {
                _rotationQueue.Add(_nearestTrigger.GetComponent<PlayerCamTrigger>().CameraRotation);
                _prevTrigger = _nearestTrigger;
            }

            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator RotateTimer()
    {
        _rotating = true;
        yield return new WaitForSeconds(2.5f);
        CompleteRotation();
    }

    void CompleteRotation()
    {
        _prevRotation = new Quaternion(
                    _playerCamera.transform.rotation.x,
                    _playerCamera.transform.rotation.y,
                    _playerCamera.transform.rotation.z,
                    _playerCamera.transform.rotation.w
        );
        _timeCount = 0.0f;
        _rotationQueue.RemoveAt(0);
        _rotating = false;
    }
}
