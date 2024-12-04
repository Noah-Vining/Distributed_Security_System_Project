using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorController : NetworkBehaviour
{

    public List<GameObject> doors = new List<GameObject>();
    private NetworkVariable<int> count = new NetworkVariable<int>();

    public GameObject levelCounter;

    private bool isDown = false; 

    // Start is called before the first frame update
    void Start()
    {
        int increment = 0;

        foreach (Transform child in transform)
        {

            if (child.tag == "DoorWall")
            {

                foreach (Transform door in child.transform)
                {

                    if (door.tag == "Door")
                    {

                        doors.Insert(increment, door.gameObject);

                        increment++;
                    }

                }
            }

        }


        levelCounter = GameObject.FindGameObjectWithTag("LevelCounter");
        levelCounter.GetComponent<LevelCounter>().FindDoor();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner) {

            if (count.Value >= 100) {
                isDown = false;
                MoveUp();
                MoveServerRPC(false);
            }

            if (count.Value < 100 && !isDown) {
               MoveDown();
                MoveServerRPC(true);
            }

        }

    }

    public void ChangeCount(int newCount)
    {
        count.Value = newCount;
    }

    public void MoveDown() {
        foreach (GameObject door in doors) {
            var distance = 0.8f * Time.deltaTime;
            door.transform.position = Vector3.MoveTowards(door.transform.position, door.GetComponent<IndividualDoor>().endPosition, distance);

            if (door.transform.position == door.GetComponent<IndividualDoor>().endPosition) {
                isDown = true;
            }

        }
    }

    public void MoveUp() {
        foreach (GameObject door in doors) {
            var distance = 0.8f * Time.deltaTime;
            door.transform.position = Vector3.MoveTowards(door.transform.position, door.GetComponent<IndividualDoor>().startPosition, distance);

        }
        
    }

    [ServerRpc]
    public void MoveServerRPC(bool isDown){
        if (isDown) {
            MoveDown();
        }

        else {
            MoveUp();
        }
    }

    
}
