using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
public class Lines : MonoBehaviour
{
    public float refractIndexOne = 1f;
    public float refractIndexTwo = 1f;

    public float angle = 45f;
    
    // This code uses the query backfaces from the physics menu, that needs to be Enabled.
    void Update()
    {
        var tAngle = transform.TransformDirection(new Vector3(45f, angle, 0f));
 
        RaycastHit hit;
        // First RAYCAST
        if (Physics.Raycast(transform.position, tAngle, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(Vector3.zero, hit.point, Color.red, 0.01f, false);
 
            RaycastHit hitTwo;

            var angletwo = GetRefractionAngle(angle, refractIndexOne, refractIndexTwo);
            var newOrigin = hit.point + ProjectPosition(angle, 0.01f);
 
            // Second (internal) RAYCAST
            if (Physics.Raycast(newOrigin, transform.TransformDirection(new Vector3(45f, angletwo, 0f)), out hitTwo,
                Mathf.Infinity))
            {
                Debug.DrawLine(newOrigin, hitTwo.point, Color.yellow, 0.01f, false);
                var angleThree = GetRefractionAngle(angletwo, refractIndexTwo, refractIndexOne);
                
                var newOriginTwo = hitTwo.point + ProjectPosition(angle, 0.01f);
                var tAngleTwo = transform.TransformDirection(new Vector3(45f, angleThree, 0f));
                
                Debug.DrawRay(newOriginTwo, tAngleTwo, Color.red, 0.01f, false);
            }
        }
    }

    private Vector3 ProjectPosition(float angle, float stepSize)
    {
        var x = stepSize * Mathf.Cos(angle * Mathf.Deg2Rad);
        var y = stepSize * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x, y, 0f);
    }

    private float GetRefractionAngle(float angle, float refractIndexOne, float refractIndexTwo)
    {
        var rad = angle * Mathf.PI / 180f;
        var stepOne = refractIndexOne * Mathf.Sin(rad);
        var stepTwo = stepOne / refractIndexTwo;
        return Mathf.Asin(stepTwo) * 180f / Mathf.PI;
    }
}