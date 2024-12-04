using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkButtons : NetworkBehaviour {
    

    [SerializeField] private GameObject communicator;
    [SerializeField] private GameObject securityLevel;

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            if (GUILayout.Button("Host")) { 
                NetworkManager.Singleton.StartHost();
                SpawnCommunicatorServerRpc(NetworkManager.Singleton.LocalClientId);
            }

            if (GUILayout.Button("Server")) { 
                NetworkManager.Singleton.StartServer();
            }

            if (GUILayout.Button("Client")) { 
                NetworkManager.Singleton.StartClient();
            }
        }

        GUILayout.EndArea();
    }

 
    
    [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    public void SpawnCommunicatorServerRpc(ulong clientId)
    {
        GameObject newPlayer;
        newPlayer = (GameObject)Instantiate(communicator);
        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId, true);
        

    }

    public void ChangeSecurityLevel() {
        securityLevel.GetComponent<LevelCounter>().ChangeLevel(1);
    }
 


    // private void Awake() {
    //     GetComponent<UnityTransport>().SetDebugSimulatorParameters(
    //         packetDelay: 120,
    //         packetJitter: 5,
    //         dropRate: 3);
    // }
}