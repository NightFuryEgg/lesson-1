using UnityEngine;

public class DiceValue : MonoBehaviour
{
    public int GetTopValue()
    {
        Vector3[] faceDirections = new Vector3[6];
        faceDirections[0] = transform.up;        // верх кубика
        faceDirections[1] = -transform.up;       // низ кубика
        faceDirections[2] = transform.forward;   // перед кубика (+Z)
        faceDirections[3] = -transform.forward;  // зад кубика (-Z)
        faceDirections[4] = transform.right;     // право кубика (+X)
        faceDirections[5] = -transform.right;    // лево кубика (-X)


        int[] faceValues = { 1, 6, 2, 5, 3, 4 };

        int bestValue = faceValues[0];
        float bestDot = -2f;

        for (int i = 0; i < faceDirections.Length; i++)
        {
            float dot = Vector3.Dot(faceDirections[i], Vector3.up);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestValue = faceValues[i];
            }
        }

        return bestValue;
    }
}