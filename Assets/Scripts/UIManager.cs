using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Put your Canvas gameobject here")]
    private GameObject _canvas;

    [SerializeField]
    [Tooltip("Put your Pause Menu gameobject here")]
    private GameObject _pauseMenu;

    [SerializeField]
    [Tooltip("Put your Player Stamina gameobject here")]
    private PlayerStamina _playerStamina;

    [SerializeField]
    [Tooltip("Put your Stamina Bar gameobject here")]
    private GameObject _staminaBar;
    private GameObject _referenceStaminaUnit;
    private List<GameObject> _staminaUnits = new List<GameObject>();
    private List<Image> _staminaUnitsImages = new List<Image>();

    [SerializeField]
    [Tooltip("Put your Shard Indicator gameobject here")]
    private GameObject _shardIndicator;
    private TextMeshProUGUI _shardIndicatorText;
    private Image _shardIndicatorImage;

    [Header("Stamina Unit Sprites")]
    [SerializeField] private Sprite _staminaUnitFull;
    [SerializeField] private Sprite _staminaUnitEmpty;

    [SerializeField] private Sprite[] _shardSprites;

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

        _referenceStaminaUnit = _staminaBar.transform.GetChild(0).gameObject;
        _referenceStaminaUnit.SetActive(false);
        UpdateStaminaBar();

        _shardIndicatorText = _shardIndicator.GetComponentInChildren<TextMeshProUGUI>();
        _shardIndicatorImage = _shardIndicator.transform.Find("Shard Image").GetComponent<Image>();
        UpdateShardIndicator();
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
        Time.timeScale = 0;
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
        Time.timeScale = 1;
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
            GameObject staminaUnit = GameObject.Instantiate(_referenceStaminaUnit);
            staminaUnit.transform.SetParent(_staminaBar.transform, false);
            staminaUnit.SetActive(true);
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

    private void ErrorAndDisable(string text)
    {
        Debug.LogError(text);
        enabled = false;
    }
}