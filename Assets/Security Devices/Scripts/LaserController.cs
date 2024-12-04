using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class LaserController : NetworkBehaviour
{
    private bool foundObjects = false;

    private GameObject levelCounter;
    private GameObject lights; 

    void Start()
    {
        levelCounter = GameObject.FindGameObjectWithTag("LevelCounter");
    }


    private void OnTriggerEnter(Collider other) {

        if (!foundObjects)
        {
            lights = GameObject.FindGameObjectWithTag("LightClient");
            foundObjects = true;
        }

        if (IsOwner)
        {
            HitDetectedServerRPC();
        }

    }

    [ServerRpc]
    public void HitDetectedServerRPC()
    {
        lights.GetComponent<LightController>().ColorChangeClientRPC(true);
        levelCounter.GetComponent<LevelCounter>().ChangeLevel(30);
        // Debug.Log("hit detected");
    }


}
