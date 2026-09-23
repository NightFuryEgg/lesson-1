using UnityEngine;

public class Cube_rotation : MonoBehaviour

{
    [Header("Rotation Speed")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0f, 1f, 0f);


  
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
