using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRotate : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1f;

    public bool isFloating = false;
    public bool rotatesOnY = true;
    public bool isRotating = true;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        posOffset = transform.position;
    }


    void Update()
    {

        if (isFloating == true)
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;

            transform.position = tempPos;
        }

        if (isRotating == true)
        { 
             if (rotatesOnY == true)
             {
                  transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f), Space.Self);
             }
              else
             {
                  transform.Rotate(new Vector3(Time.deltaTime * rotateSpeed, 0f, 0f), Space.Self);
             }
        }
    }
}
