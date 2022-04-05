using UnityEngine;


[ExecuteInEditMode]
public class Refraction : MonoBehaviour
{
    public Transform lineOne;
    public Transform lineTwo;

    // Update is called once per frame
    void Update()
    {
        var newAngle = Refract(1f, 1.2f, Vector3.zero, lineOne.rotation.eulerAngles);
        lineTwo.rotation = Quaternion.Euler(newAngle);
    }
    
    /**
      * returns:
      *  normalized Vector3 refracted by passing from one medium to another in a realistic manner according to Snell's Law
      *
      * parameters:
      *  RI1 - the refractive index of the first medium
      *  RI2 - the refractive index of the second medium
      *  surfNorm - the normal of the interface between the two mediums (for example the normal returned by a raycast)
      *  incident - the incoming Vector3 to be refracted
      *
      * usage example (laser pointed from a medium with RI roughly equal to air through a medium with RI roughly equal to water):
      *  Vector3 laserRefracted = Refract(1.0f, 1.33f, waterPointNorm, laserForward);
    */
    
    public static Vector3 Refract(float RI1, float RI2, Vector3 surfNorm, Vector3 incident)
    {
        surfNorm.Normalize(); //should already be normalized, but normalize just to be sure
        incident.Normalize();
 
        return (RI1/RI2 * Vector3.Cross(surfNorm, Vector3.Cross(-surfNorm, incident)) - surfNorm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(surfNorm, incident)*(RI1/RI2*RI1/RI2), Vector3.Cross(surfNorm, incident)))).normalized;
    }
}
