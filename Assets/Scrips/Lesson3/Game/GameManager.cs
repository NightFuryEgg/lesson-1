using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private DiceSpawner diceSpawner; // откуда брать кубики
    [SerializeField] private UIManager uiManager;     // куда отправлять результат

    [Header("Настройки броска")]
    [SerializeField] private float throwStrength = 1f;    // множитель силы броска
    [SerializeField] private float delayAfterStop = 0.3f; // пауза после остановки всех кубиков
    [SerializeField] private float maxRollTime = 8f;      // максимальное время ожидания остановки

    private bool isRolling = false;
    [ContextMenu("Roll Dice (тест)")]
    public void RollDice()
    {
        if (isRolling)
        {
            return;
        }

        isRolling = true;
        uiManager.ShowRolling(); 
        StartCoroutine(RollRoutine());
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void SetThrowStrength(float newStrength)
    {
        throwStrength = newStrength;
    }

    public void SetDiceCount(int newCount)
    {
        if (isRolling)
        {
            return;
        }

        diceSpawner.SetDiceCount(newCount);
        uiManager.ShowReady(diceSpawner.GetDiceCount());
    }

    private IEnumerator RollRoutine()
    {
        int diceCount = diceSpawner.GetDiceCount();
        for (int i = 0; i < diceCount; i++)
        {
            diceSpawner.GetController(i).Roll(throwStrength);
        }

        float rollTimer = 0f;
        while (AllDiceStopped() == false && rollTimer < maxRollTime)
        {
            rollTimer += Time.deltaTime;
            yield return null;
        }

        if (rollTimer >= maxRollTime)
        {
            Debug.LogWarning("Кубики не остановились вовремя, считаем результат принудительно.");
        }

        yield return new WaitForSeconds(delayAfterStop);

        CalculateResult();

        isRolling = false;
    }

    private bool AllDiceStopped()
    {
        int diceCount = diceSpawner.GetDiceCount();
        for (int i = 0; i < diceCount; i++)
        {
            if (diceSpawner.GetController(i).IsRolling())
            {
                return false;
            }
        }
        return true;
    }

    private void CalculateResult()
    {
        int diceCount = diceSpawner.GetDiceCount();
        int[] values = new int[diceCount];
        int total = 0;
        string valuesText = "";

        for (int i = 0; i < diceCount; i++)
        {
            values[i] = diceSpawner.GetValue(i).GetTopValue();
            total += values[i];

            if (i > 0)
            {
                valuesText += ", ";
            }
            valuesText += values[i];
        }
        
        Debug.Log("Кубики: " + valuesText + " | Сумма очков: " + total);

        uiManager.ShowResult(values, total);
    }
}