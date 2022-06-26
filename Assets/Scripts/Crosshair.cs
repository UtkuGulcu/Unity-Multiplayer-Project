using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Crosshair : MonoBehaviour
{
    [HideInInspector] public Vector3 mousePosition;

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 1;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;
    }
}
