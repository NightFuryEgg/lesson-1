using UnityEngine;

public class CubeCreate : MonoBehaviour
{
    [Header("Cube Prefab")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float height = 0f;
    [SerializeField] private int numberOfCubes = 5;
    // [SerializeField] private float angleIncrement = 30; // Angle increment for each cube
    private GameObject[] cubes;

    void Awake()
    {


        if (cubePrefab == null)
        {
            // for (int i = 0; i < numberOfCubes; i++)
        
            Debug.LogError("Cube Prefab is not assigned in the inspector.");
            return;
            // float angle = i * angleIncrement;
            // Vector3 position = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * cubePosition.z, cubePosition.y, Mathf.Cos(angle * Mathf.Deg2Rad) * cubePosition.z);
            // cube = Instantiate(cubePrefab, position, Quaternion.identity);
            // cube.transform.SetParent(this.transform);
        }
            // cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
            // cube.transform.SetParent(this.transform);
            CreateCubes();
      
    }

    private void CreateCubes()
    {
        cubes = new GameObject[numberOfCubes];

        for (int i = 0; i < numberOfCubes; i++)
        {
            cubes[i] = Instantiate(cubePrefab, transform);
        }
    }


    // Update is called once per frame
    private void Update()
    {
        if (cubes == null)
            return;

        for (int i = 0; i < cubes.Length; i++)
        {
            float angle = i * (360f / numberOfCubes);

            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

            cubes[i].transform.localPosition = new Vector3(x,height,z);
        }
    }
}
    // void Update()
    // {
    //     // cube.transform.localPosition = cubePosition;
    // }

