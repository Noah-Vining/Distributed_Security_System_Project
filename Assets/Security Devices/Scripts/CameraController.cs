using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using JetBrains.Annotations;

public class CameraController : NetworkBehaviour
{

   private GameObject levelCounter;
   private GameObject lights;

    private bool foundObjects = false; 
    private bool foundLights = false;

    void Start() {
        levelCounter = GameObject.FindGameObjectWithTag("LevelCounter");
        lights = GameObject.FindGameObjectWithTag("LightClient");

        if (lights != null)
        {
            foundLights = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        /*
        if (!foundObjects) {
                lights = GameObject.FindGameObjectWithTag("LightClient");
                foundObjects = true;
            }
        */
        if (IsOwner) { 
            HitDetectedServerRPC(true);
        }
       
    }

    private void OnTriggerExit(Collider other) {
        if (IsOwner) {
            HitDetectedServerRPC(false);
        }
    }
        [ServerRpc]
    public void HitDetectedServerRPC(bool enter) {

        if (enter) {
            if (foundLights) {
                lights.GetComponent<LightController>().ColorChangeClientRPC(true);
            }
            
            levelCounter.GetComponent<LevelCounter>().ChangeLevel(1);
        }

        else {
            if (foundLights) {
                lights.GetComponent<LightController>().ColorChangeClientRPC(true);
            }
        }

      
        // Debug.Log("hit detected");
    }

    public void GetLights()
    {
        lights = GameObject.FindGameObjectWithTag("LightClient");
        foundLights = true;
    }

 
}
