using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float rotateAngle;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        rotateAngle = 60 * Time.deltaTime;
        transform.Rotate(0, rotateAngle, 0);
    }
}
