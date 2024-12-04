using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Security.Cryptography;
using System.Text;
using System;
using Random = UnityEngine.Random;
using System.Globalization;


public class CustomNetworkSync : NetworkBehaviour
{


    [SerializeField] public Transform spawnedObjectPrefab;
    [SerializeField] public Transform spawnedObjectTransform;
    
    [SerializeField] public NetworkVariable<float> randomNumber = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);

    

    [ServerRpc(RequireOwnership = false)]
    private void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //if (!IsOwner) return;
        Debug.Log("Running TestServerRpc remote func queued by ClientID >> " + serverRpcParams.Receive.SenderClientId);

        spawnedObjectTransform = Instantiate(spawnedObjectPrefab).transform;
        //spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        
        spawnedObjectTransform.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId,true);
        
        //spawnedObjectTransform.GetComponent<NetworkObject>().ChangeOwnership(1);

        SetTransformsClientRpc();

    }

    [ClientRpc]
    private void SetTransformsClientRpc()
    {
        spawnedObjectTransform = GameObject.Find("Water Mesh Networked(Clone)").transform;
        spawnedObjectTransform.GetComponent<WaveGeneratorNetworked>().localWaveScale = 1f;


    }
    
    [ClientRpc]
    private void TestClientRpc()
    {
        
        randomNumber.Value = Random.Range(0, 10);

        // it does nothing at the moment
        // to achieve true p2p arch, one needs to craft ClientRPCs well
        //Debug.Log("Testing ClientRpc owned by >> " + OwnerClientId);
        Debug.Log(OwnerClientId + "; random number:" + randomNumber.Value);
        string someTxt = this.GetComponent<AESHandler>().AESEncryption((randomNumber.Value).ToString());
        Debug.Log(someTxt);
        Debug.Log(this.GetComponent<AESHandler>().AESDecryption((someTxt)));


        spawnedObjectTransform.GetComponent<WaveGeneratorNetworked>().localWaveOffsetSpeed =
            float.Parse(this.GetComponent<AESHandler>().AESDecryption(someTxt));

        


    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        //TestServerRpc();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            //Debug.Log(OwnerClientId + "; random number:" + randomNumber.Value);
            //Debug.Log(spawnedObjectPrefab.GetComponent<NetworkObject>().IsOwner);

            TestClientRpc();
        }

        

    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

    

            if (GUILayout.Button("Deploy Water Mesh"))
            { 
                
                TestServerRpc();


                Debug.Log(NetworkManager.Singleton.LocalClientId);
                
                //wip
            
           


        }

       

        GUILayout.EndArea();
    }
    
    
}
