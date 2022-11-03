using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Collider))]
public class LocationVisitedTrigger : MonoBehaviour
{
    [SerializeField] string _locationName;
    VariableStorageBehaviour _variableStorage;

    void Start()
    {
        _variableStorage = FindObjectOfType<VariableStorageBehaviour>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _variableStorage.SetValue($"$visited{_locationName}", true);
            gameObject.SetActive(false);
        }
    }
}
