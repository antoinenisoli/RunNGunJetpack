using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    enum RotationType
    {
        ForwardX,
        ForwardY,
        ForwardZ,
        ReverseX,
        ReverseY,
        ReverseZ,
    }

    [SerializeField] float speed = 0f;
    [SerializeField] RotationType type;

    void Update()
    {
        switch (type)
        {
            case RotationType.ForwardX:
                transform.Rotate(Time.deltaTime * speed, 0, 0, Space.Self);
                break;
            case RotationType.ForwardY:
                transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
                break;
            case RotationType.ForwardZ:
                transform.Rotate(0, 0, Time.deltaTime * speed, Space.Self);
                break;
            case RotationType.ReverseX:
                transform.Rotate(-Time.deltaTime * speed, 0, 0, Space.Self);
                break;
            case RotationType.ReverseY:
                transform.Rotate(0, -Time.deltaTime * speed, 0, Space.Self);
                break;
            case RotationType.ReverseZ:
                transform.Rotate(0, 0, -Time.deltaTime * speed, Space.Self);
                break;
        }
    }
}
