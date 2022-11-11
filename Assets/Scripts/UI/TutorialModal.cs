using UnityEngine;
using StarterAssets;

public class TutorialModal : MonoBehaviour
{
    private void OnEnable()
    {
        StarterAssetsInputs.InteractPressed += HideModal;
    }

    private void OnDisable()
    {
        StarterAssetsInputs.InteractPressed -= HideModal;
    }

    private void HideModal()
    {
        gameObject.SetActive(false);
    }
}
