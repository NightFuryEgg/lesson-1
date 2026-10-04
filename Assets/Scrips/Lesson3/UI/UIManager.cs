using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Ссылки на другие скрипты")]
    [SerializeField] private GameManager gameManager; // кому передавать нажатие кнопки
    [SerializeField] private InputManager inputManager; // откуда брать название клавиши

    [Header("Элементы интерфейса")]
    [SerializeField] private Button rollButton;         // кнопка "БРОСИТЬ КУБИКИ"
    [SerializeField] private TMP_Text resultText;       // сумма очков или сообщение о состоянии
    [SerializeField] private TMP_Text diceValuesText;   // значения отдельных кубиков
    [SerializeField] private TMP_Text diceCountText;    // сколько кубиков используется
    [SerializeField] private TMP_Text hintText;         // подсказка про клавишу

    private void Start()
    {
        rollButton.onClick.AddListener(OnRollButtonClicked);
    }

    private void Update()
    {
        string newHint = "Бросок: кнопка или клавиша [" + inputManager.GetRollKeyName() + "]";

        if (hintText.text != newHint)
        {
            hintText.text = newHint;
        }
    }

    private void OnRollButtonClicked()
    {
        gameManager.RollDice();
    }

    public void ShowReady(int diceCount)
    {
        resultText.text = "Нажмите «БРОСИТЬ КУБИКИ»";
        diceValuesText.text = "";
        diceCountText.text = "Количество кубиков: " + diceCount;
        rollButton.interactable = true;
    }

    public void ShowRolling()
    {
        resultText.text = "Кубики брошены...";
        diceValuesText.text = "";
        rollButton.interactable = false; 
    }

    public void ShowResult(int[] values, int total)
    {
        string valuesLines = "";

        for (int i = 0; i < values.Length; i++)
        {
            valuesLines += "Кубик " + (i + 1) + ": " + values[i];

            if (i < values.Length - 1)
            {
                valuesLines += "\n";
            }
        }

        diceValuesText.text = valuesLines;
        resultText.text = "Сумма очков: " + total;
        rollButton.interactable = true; 
    }
}