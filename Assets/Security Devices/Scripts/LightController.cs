using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.ProBuilder.MeshOperations;

public class LightController : NetworkBehaviour
{
    // Start is called before the first frame update


    public List<GameObject> singLights = new List<GameObject>();
    public List<GameObject> cameras = new List<GameObject>();

    public GameObject cameraClient;

    private int timer = 0;
    private bool needRed = false;

    void Start()
    {
        int increment = 0;

        cameraClient = GameObject.FindGameObjectWithTag("CameraClient");

        if (cameraClient != null)
        {
            
            foreach (Transform child in cameraClient.transform)
            {

                if (child.tag == "Camera")
                {

                    foreach (Transform area in child.transform)
                    {

                        if (area.tag == "DetectionArea")
                        {
                            Debug.Log("Made it here");
                            cameras.Insert(increment, area.gameObject);

                            increment++;
                        }

                    }

                }
            }
        }

        foreach (GameObject camera in cameras) {
            camera.GetComponent<CameraController>().GetLights();
        }

        increment = 0;

        foreach (Transform child in transform)
        {
            if (child.tag == "RoomLight")
            {

                foreach (Transform currentLight in child.transform)
                {

                    if (currentLight.tag == "Light")
                    {

                        foreach (Transform lightSource in currentLight.transform)
                        {

                            if (lightSource.name == "Light")
                            {

                                singLights.Insert(increment, lightSource.gameObject);

                                increment++;

                            }
                        }
                    }
                }
            }

            

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner) {

            if (needRed && (timer <= 30)) {

                foreach (GameObject currentLight in singLights) {

                    currentLight.GetComponent<Light>().color = Color.red;
                }

                LightChangeServerRPC(true);
                timer += 1;
            }

            else {
                timer = 0;
                needRed = false;
                foreach (GameObject currentLight in singLights) {

                    currentLight.GetComponent<Light>().color = Color.white;
                }
                LightChangeServerRPC(false);
            }

        }
    }

    [ClientRpc]
    public void ColorChangeClientRPC(bool alerted)
    {
        if (IsOwner)
        {
            if (alerted)
            {
                needRed = true;
                timer = 0; 
            }
            else
            {
                needRed = false;
            }

        }
    }

    [ServerRpc]
    public void LightChangeServerRPC(bool alerted)
    {
        if (alerted)
        {
            foreach (GameObject currentLight in singLights)
            {
                currentLight.GetComponent<Light>().color = Color.red;
            }
        }

        else
        {
            foreach (GameObject currentLight in singLights)
            {
                currentLight.GetComponent<Light>().color = Color.white;
            }
        }

    }


}
