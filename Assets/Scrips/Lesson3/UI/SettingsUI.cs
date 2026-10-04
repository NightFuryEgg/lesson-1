using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsUI : MonoBehaviour
{
    [Header("Ссылки на другие скрипты")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;

    [Header("Панель и кнопки")]
    [SerializeField] private GameObject settingsPanel;   // сама панель настроек
    [SerializeField] private Button openSettingsButton;  // кнопка открытия (на главном экране)
    [SerializeField] private Button closeSettingsButton; // кнопка закрытия (на панели)
    [SerializeField] private Button rebindButton;        // кнопка "Изменить клавишу"
    [SerializeField] private Button resetButton;         // кнопка "Сбросить настройки"

    [Header("Количество кубиков")]
    [SerializeField] private Slider diceCountSlider;
    [SerializeField] private TMP_Text diceCountValueText;

    [Header("Сила броска")]
    [SerializeField] private Slider throwStrengthSlider;
    [SerializeField] private TMP_Text throwStrengthValueText;

    [Header("Клавиша броска")]
    [SerializeField] private TMP_Text keyText;

    [Header("Значения по умолчанию")]
    [SerializeField] private int defaultDiceCount = 3;
    [SerializeField] private float defaultThrowStrength = 1f;


    private const string DiceCountKey = "DiceCount";
    private const string ThrowStrengthKey = "ThrowStrength";

    private void Start()
    {

        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
        rebindButton.onClick.AddListener(OnRebindButtonClicked);
        resetButton.onClick.AddListener(ResetSettings);
        diceCountSlider.onValueChanged.AddListener(OnDiceCountChanged);
        throwStrengthSlider.onValueChanged.AddListener(OnThrowStrengthChanged);

        LoadSettings();


        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        bool isRolling = gameManager.IsRolling();


        diceCountSlider.interactable = !isRolling;
        resetButton.interactable = !isRolling;

        bool isRebinding = inputManager.IsRebinding();
        rebindButton.interactable = !isRebinding;

        if (isRebinding)
        {
            keyText.text = "Нажмите клавишу... (Esc - отмена)";
        }
        else
        {
            keyText.text = inputManager.GetRollKeyName();
        }
    }

    private void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        PlayerPrefs.Save();
    }

    private void OnRebindButtonClicked()
    {
        inputManager.StartRebinding();
    }


    private void LoadSettings()
    {
        int savedDiceCount = PlayerPrefs.GetInt(DiceCountKey, defaultDiceCount);
        float savedThrowStrength = PlayerPrefs.GetFloat(ThrowStrengthKey, defaultThrowStrength);


        diceCountSlider.SetValueWithoutNotify(savedDiceCount);
        throwStrengthSlider.SetValueWithoutNotify(savedThrowStrength);

        OnDiceCountChanged(diceCountSlider.value);
        OnThrowStrengthChanged(throwStrengthSlider.value);
    }


    private void OnDiceCountChanged(float newValue)
    {
        int diceCount = Mathf.RoundToInt(newValue);

        diceCountValueText.text = diceCount.ToString();
        gameManager.SetDiceCount(diceCount);
        PlayerPrefs.SetInt(DiceCountKey, diceCount);
    }


    private void OnThrowStrengthChanged(float newValue)
    {
        throwStrengthValueText.text = newValue.ToString("0.0");
        gameManager.SetThrowStrength(newValue);
        PlayerPrefs.SetFloat(ThrowStrengthKey, newValue);
    }


    private void ResetSettings()
    {

        diceCountSlider.value = defaultDiceCount;
        throwStrengthSlider.value = defaultThrowStrength;

        inputManager.ResetBinding(); 

        PlayerPrefs.Save();
    }
}