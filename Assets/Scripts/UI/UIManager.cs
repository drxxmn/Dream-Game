using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Put your Canvas gameobject here")]
    private GameObject _canvas;

    [SerializeField]
    [Tooltip("Put your Pause Menu gameobject here")]
    private GameObject _pauseMenu;

    [SerializeField]
    [Tooltip("Put your Player Stamina script here")]
    private PlayerStamina _playerStamina;

    [SerializeField]
    [Tooltip("Put your Stamina Bar gameobject here")]
    private GameObject _staminaBar;
    [SerializeField] private GameObject _staminaUnitPrefab;
    [SerializeField] private GameObject _tutorialModal;
    private List<GameObject> _staminaUnits = new List<GameObject>();
    private List<Image> _staminaUnitsImages = new List<Image>();

    [SerializeField]
    [Tooltip("Put your Shard Indicator gameobject here")]
    private GameObject _shardIndicator;
    private TextMeshProUGUI _shardIndicatorText;
    private Image _shardIndicatorImage;
    private Animator _shardIndicatorAnimator;
    [SerializeField] private float _shardIndicatorShowDuration = 2f;
    [SerializeField] private AudioSource _UISFXAudioSource;
    [SerializeField] private AudioClip _openPauseMenuClip;
    [SerializeField] private AudioClip _closePauseMenuClip;

    [Header("Float indicator")]
    [SerializeField] private GameObject _floatIndicator;
    [SerializeField] private Sprite _floatIndicatorOn;
    [SerializeField] private Sprite _floatIndicatorOff;

    [Header("Stamina Unit Sprites")]
    [SerializeField] private Sprite _staminaUnitFull;
    [SerializeField] private Sprite _staminaUnitEmpty;

    [SerializeField] private Sprite[] _shardSprites;

    private ThirdPersonController _playerController;

    private void Start()
    {
        if (_canvas == null)
        {
            _canvas = transform.Find("Canvas").gameObject;
            if (_canvas == null) ErrorAndDisable("Canvas object in UIManager is missing!");
        }

        if (_pauseMenu == null)
        {
            _pauseMenu = _canvas.transform.Find("Pause Menu").gameObject;
            if (_pauseMenu == null) ErrorAndDisable("Pause Menu UI object in UIManager is missing!");
        }

        if (_playerStamina == null)
        {
            _playerStamina = FindObjectOfType<PlayerStamina>();
            if (_playerStamina == null) ErrorAndDisable("Player Stamina object in UIManager is missing!");
        }

        if (_staminaBar == null)
        {
            _staminaBar = _canvas.transform.Find("Stamina Bar").gameObject;
            if (_staminaBar == null) ErrorAndDisable("Stamina Bar UI object in UIManager is missing!");
        }

        if (_shardIndicator == null)
        {
            _shardIndicator = _canvas.transform.Find("Shard Indicator").gameObject;
            if (_shardIndicator == null) ErrorAndDisable("Shard Indicator UI object in UIManager is missing!");
        }

        UpdateStaminaBar();

        _shardIndicatorText = _shardIndicator.GetComponentInChildren<TextMeshProUGUI>();
        _shardIndicatorImage = _shardIndicator.transform.Find("Shard Image").GetComponent<Image>();
        _shardIndicatorAnimator = _shardIndicator.GetComponent<Animator>();
        UpdateShardIndicator();

        _pauseMenu.SetActive(false);

        _playerController = FindObjectOfType<ThirdPersonController>();
    }

    private void OnEnable()
    {
        StarterAssetsInputs.FloatPressed += ShowFloatIndicator;
    }

    private void OnDisable()
    {
        StarterAssetsInputs.FloatPressed -= ShowFloatIndicator;
    }

    private void Update()
    {
        for (int i = 0; i < _playerStamina.MaxStamina; i++)
        {
            if (i + 1 > _playerStamina.CurStamina) _staminaUnitsImages[i].sprite = _staminaUnitEmpty;
            else _staminaUnitsImages[i].sprite = _staminaUnitFull;
        }
    }

    public void MenuToggle()
    {
        if (!_pauseMenu.activeInHierarchy) ShowMenu();
        else ResumeGame();
    }

    private void ShowMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (Transform child in _canvas.transform)
        {
            if (child.gameObject.tag == "Yarn") child.gameObject.SetActive(false);
        }
        _pauseMenu.SetActive(true);
        _shardIndicatorAnimator.SetBool("visible", true);
        Time.timeScale = 0;
        _UISFXAudioSource.PlayOneShot(_openPauseMenuClip);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (Transform child in _canvas.transform)
        {
            if (child.gameObject.tag == "Yarn") child.gameObject.SetActive(true);
        }
        _pauseMenu.SetActive(false);
        _shardIndicatorAnimator.SetBool("visible", false);
        Time.timeScale = 1;
        _UISFXAudioSource.PlayOneShot(_closePauseMenuClip);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateStaminaBar()
    {
        int maxStamina = Mathf.FloorToInt(_playerStamina.MaxStamina);
        int unitsToAdd = maxStamina - _staminaUnits.Count;

        for (int i = 0; i < unitsToAdd; i++)
        {
            GameObject staminaUnit = GameObject.Instantiate(_staminaUnitPrefab);
            staminaUnit.transform.SetParent(_staminaBar.transform, false);
            _staminaUnits.Add(staminaUnit);
            _staminaUnitsImages.Add(staminaUnit.GetComponent<Image>());
        }
    }

    public void UpdateShardIndicator()
    {
        _shardIndicatorText.text = $"{_playerStamina.UpgradeProgress}/{_playerStamina.ShardsToUpgrade}";

        if (_playerStamina.UpgradeProgress < _shardSprites.Length)
        {
            _shardIndicatorImage.sprite = _shardSprites[_playerStamina.UpgradeProgress];
        }
        else
        {
            _shardIndicatorImage.sprite = _staminaUnitFull;
        }
    }

    public void ShowShardIndicator()
    {
        _shardIndicatorAnimator.SetBool("visible", true);
        StartCoroutine(ShardIndicatorTimer());
    }

    [YarnCommand("show_tutorial_modal")]
    public void ShowTutorialModal()
    {
        if (_tutorialModal == null) return;

        _tutorialModal.SetActive(true);
    }

    private System.Collections.IEnumerator ShardIndicatorTimer()
    {
        yield return new WaitForSeconds(_shardIndicatorShowDuration);
        _shardIndicatorAnimator.SetBool("visible", false);
    }

    private void ShowFloatIndicator()
    {
        Image image = _floatIndicator.GetComponent<Image>();
        if (_playerController.FloatMode) image.sprite = _floatIndicatorOn;
        else image.sprite = _floatIndicatorOff;
    }

    private void ErrorAndDisable(string text)
    {
        Debug.LogError(text);
        enabled = false;
    }
}
