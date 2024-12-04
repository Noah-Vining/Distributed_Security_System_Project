using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSensor : MonoBehaviour
{

    public bool isClose = false;

    private void OnTriggerEnter(Collider other)
    {
        isClose = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isClose = false;
    }
}
