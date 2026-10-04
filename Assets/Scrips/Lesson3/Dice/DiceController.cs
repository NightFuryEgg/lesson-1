using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    [Header("Сила броска (импульс)")]
    [SerializeField] private float minUpImpulse = 6f;     // минимальный импульс вверх
    [SerializeField] private float maxUpImpulse = 9f;     // максимальный импульс вверх
    [SerializeField] private float minSideImpulse = 1f;   // минимальный импульс в сторону
    [SerializeField] private float maxSideImpulse = 3f;   // максимальный импульс в сторону
    [SerializeField] private float maxTorqueImpulse = 3f; // максимальное вращение по каждой оси

    [Header("Определение остановки")]
    [SerializeField] private float stopLinearSpeed = 0.1f;  // порог скорости движения
    [SerializeField] private float stopAngularSpeed = 0.1f; // порог скорости вращения
    [SerializeField] private float stopTime = 0.5f;         // сколько секунд скорость должна быть низкой

    [Header("Защита от вылета со стола")]
    [SerializeField] private float fallLimitY = -5f; // если кубик упал ниже этой высоты - вернём его

    private Rigidbody diceRigidbody;
    private bool isRolling = false;
    private float lowSpeedTimer = 0f;
    private Vector3 positionBeforeRoll;

    private void Awake()
    {
        diceRigidbody = GetComponent<Rigidbody>();

        diceRigidbody.maxAngularVelocity = 50f;
    }

    public void Roll(float strengthMultiplier)
    {
        positionBeforeRoll = transform.position;

        float upImpulse = Random.Range(minUpImpulse, maxUpImpulse) * strengthMultiplier;
        float sideImpulse = Random.Range(minSideImpulse, maxSideImpulse) * strengthMultiplier;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 sideDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        Vector3 throwImpulse = Vector3.up * upImpulse + sideDirection * sideImpulse;

        diceRigidbody.AddForce(throwImpulse, ForceMode.Impulse);

        Vector3 torqueImpulse = new Vector3(
            Random.Range(-maxTorqueImpulse, maxTorqueImpulse),
            Random.Range(-maxTorqueImpulse, maxTorqueImpulse),
            Random.Range(-maxTorqueImpulse, maxTorqueImpulse));
        diceRigidbody.AddTorque(torqueImpulse, ForceMode.Impulse);

        isRolling = true;
        lowSpeedTimer = 0f;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    private void Update()
    {
        if (isRolling == false)
        {
            return;
        }

        if (transform.position.y < fallLimitY)
        {
            ReturnToTable();
        }

        float linearSpeed = diceRigidbody.linearVelocity.magnitude;
        float angularSpeed = diceRigidbody.angularVelocity.magnitude;

        if (linearSpeed < stopLinearSpeed && angularSpeed < stopAngularSpeed)
        {
            lowSpeedTimer += Time.deltaTime;

            if (lowSpeedTimer >= stopTime)
            {
                isRolling = false;
            }
        }
        else
        {
            lowSpeedTimer = 0f;
        }
    }

    private void ReturnToTable()
    {
        diceRigidbody.linearVelocity = Vector3.zero;
        diceRigidbody.angularVelocity = Vector3.zero;
        diceRigidbody.position = positionBeforeRoll + Vector3.up;
    }
}