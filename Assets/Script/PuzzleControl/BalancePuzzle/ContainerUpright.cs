using System;
using UnityEngine;


public class ContainerUpright : MonoBehaviour
{
    public Transform point;
    void LateUpdate()
    {
        transform.position = point.position;
    }
}