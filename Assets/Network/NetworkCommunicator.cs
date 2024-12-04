using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class NetworkCommunicator : NetworkBehaviour
{
    private NetworkVariable<int> securityLevel = new(0);
    public Dictionary<string, GameObject> objects = new();

    bool startGUI = false;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject cameras;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject light;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject button;


    // Start is called before the first frame update
    void Start()
    {

        if (IsServer) {
            Debug.Log("ClientId: " + NetworkManager.Singleton.LocalClientId + "= Server");
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        }

        else {
            startGUI = true;
        }

    }

    [ServerRpc(RequireOwnership = false)] 
    public void SpawnPlayerServerRpc(ulong clientId, int prefabId)
    {
        GameObject newPlayer;
        NetworkObject netObj;

        switch (prefabId) {
            case 0:
                newPlayer = (GameObject)Instantiate(character);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
            case 1:  
                newPlayer = (GameObject)Instantiate(cameras);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
            case 2:
                newPlayer = (GameObject)Instantiate(laser);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
            case 3:
                newPlayer = (GameObject)Instantiate(light);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
            case 4:
                newPlayer = (GameObject)Instantiate(door);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
            case 5:
                newPlayer = (GameObject)Instantiate(button);
                netObj = newPlayer.GetComponent<NetworkObject>();
                newPlayer.SetActive(true);
                netObj.SpawnAsPlayerObject(clientId, true);
                break;
        }
    
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if(startGUI) {
            if (GUILayout.Button("Cameras"))
                {     
                    SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);
                    startGUI = false;
            }

             if (GUILayout.Button("Lasers"))
                {
                    SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 2);
                    startGUI = false;
             }

            if (GUILayout.Button("Lights"))
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 3);
                startGUI = false;
            }

            if (GUILayout.Button("Doors"))
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 4);
                startGUI = false;
            }

            if (GUILayout.Button("Buttons"))
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 5);
                startGUI = false;
            }


        }
        
        GUILayout.EndArea();

    }


}
