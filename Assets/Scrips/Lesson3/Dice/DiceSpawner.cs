using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    [Header("Что создавать")]
    [SerializeField] private GameObject dicePrefab; // префаб кубика из папки Prefabs
    [SerializeField] private int diceCount = 3;     // сколько кубиков создать
    [SerializeField] private int minDiceCount = 1;  // минимум кубиков
    [SerializeField] private int maxDiceCount = 10; // максимум кубиков

    [Header("Расстановка")]
    [SerializeField] private float spacing = 1.8f;        // расстояние между центрами кубиков
    [SerializeField] private float startHeight = 0.55f;   // высота центра кубика над столом
    [SerializeField] private float positionJitter = 0.1f; // небольшой случайный сдвиг позиции
    [SerializeField] private int dicePerRow = 5;          // сколько кубиков в одном ряду


    private List<DiceController> diceControllers = new List<DiceController>();
    private List<DiceValue> diceValues = new List<DiceValue>();

    private void Start()
    {
        SpawnDice();
    }

    public void SetDiceCount(int newCount)
    {
        diceCount = newCount;
        SpawnDice();
    }
    public int GetDiceCount()
    {
        return diceControllers.Count;
    }

    public DiceController GetController(int index)
    {
        return diceControllers[index];
    }
    public DiceValue GetValue(int index)
    {
        return diceValues[index];
    }

    public void SpawnDice()
    {
        ClearDice();

        diceCount = Mathf.Clamp(diceCount, minDiceCount, maxDiceCount);

        int rowCount = (diceCount + dicePerRow - 1) / dicePerRow;

        for (int i = 0; i < diceCount; i++)
        {
            int row = i / dicePerRow;
            int column = i % dicePerRow;

            int diceInThisRow = Mathf.Min(dicePerRow, diceCount - row * dicePerRow);

            float x = (column - (diceInThisRow - 1) / 2f) * spacing;
            float z = (row - (rowCount - 1) / 2f) * spacing;

            x += Random.Range(-positionJitter, positionJitter);
            z += Random.Range(-positionJitter, positionJitter);

            Vector3 position = new Vector3(x, startHeight, z);
            Quaternion rotation = GetRandomFlatRotation();

            GameObject newDice = Instantiate(dicePrefab, position, rotation, transform);

            diceControllers.Add(newDice.GetComponent<DiceController>());
            diceValues.Add(newDice.GetComponent<DiceValue>());
        }
    }


    private Quaternion GetRandomFlatRotation()
    {
        float angleX = Random.Range(0, 4) * 90f;
        float angleY = Random.Range(0f, 360f);
        float angleZ = Random.Range(0, 4) * 90f;
        return Quaternion.Euler(angleX, angleY, angleZ);
    }

    private void ClearDice()
    {
        for (int i = 0; i < diceControllers.Count; i++)
        {
            GameObject diceObject = diceControllers[i].gameObject;

            diceObject.SetActive(false);
            Destroy(diceObject);
        }

        diceControllers.Clear();
        diceValues.Clear();
    }
}