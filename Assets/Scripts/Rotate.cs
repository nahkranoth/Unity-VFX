using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction, speed * Time.deltaTime);
    }
}
