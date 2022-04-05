using UnityEngine;
using Vector3 = UnityEngine.Vector3;


[ExecuteInEditMode]
public class Lines : MonoBehaviour
{

   
    
    
    // This code uses the query backfaces from the physics menu, that needs to be Enabled.
    void Update()
    {
        var angle = 45f;
        var tAngle = transform.TransformDirection(new Vector3(45f, angle, 0f));
        
        RaycastHit hit;
        // First RAYCAST
        if (Physics.Raycast(transform.position, tAngle, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(Vector3.zero, hit.point, Color.red, 0.1f, false);
            
            RaycastHit hitTwo;
            
            var angletwo = 45f; //new angle is first sin(angle) 
            var rad = angle * Mathf.PI / 180f;
            
            float vacuumRefractIndex = 1f;
            float waterRefractIndex = 1.33f;
            
            var etai_over_etat = vacuumRefractIndex / waterRefractIndex;
            var cos_theta = Mathf.Cos(angle) * vacuumRefractIndex;
            var r_out_perp = etai_over_etat * cos_theta;
            var r_out_parallel = -Mathf.Sqrt(Mathf.Abs(1.0f - (r_out_perp * r_out_perp))) * vacuumRefractIndex;
            var res = r_out_perp + r_out_parallel;
            
            Debug.Log(res);
            
            // var tanTemp = Mathf.Pow(Mathf.Tan(temp), -1f);
            
            //We have to move a slight bit further to not be exactly on the hitbox but moved inside of it
            var radians = angletwo * Mathf.PI / 180f;
            var X = .01f * Mathf.Cos(radians);
            var Y = .01f * Mathf.Sin(radians);
                
            var newOrigin = hit.point + new Vector3(X, Y, 0f);
            
            // Second RAYCAST
            if (Physics.Raycast(newOrigin, transform.TransformDirection(new Vector3(45f, angletwo, 0f)), out hitTwo,
                Mathf.Infinity))
            {
                Debug.DrawLine(newOrigin, hitTwo.point, Color.yellow, 0.1f, false);
            }
            
        }
    }
}
