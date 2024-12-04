using NavKeypad;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonController : NetworkBehaviour
{

    private List<GameObject> triggers = new List<GameObject>();
    private List<GameObject> keypads = new List<GameObject>();

    private GameObject levelCounter;

    private bool inArea = false;


    // Start is called before the first frame update
    void Start()
    {
        levelCounter = GameObject.FindGameObjectWithTag("LevelCounter");

        int triggerIncrement = 0;
        int keypadIncrement = 0; 

        foreach (Transform child in transform)
        {
            if (child.tag == "Button")
            {
                foreach (Transform buttonChild in child.transform)
                {
                    if (buttonChild.tag == "Keypad")
                    {
                        keypads.Insert(keypadIncrement, buttonChild.gameObject);
                        keypadIncrement++;
                    }

                    if (buttonChild.tag == "KeypadTrigger")
                    {
                        triggers.Insert(triggerIncrement, buttonChild.gameObject);
                        triggerIncrement++;
                    }
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {      
        if (Input.GetKeyDown("e")) {
            ButtonClickedClientRPC();
        }
        
        if (levelCounter.GetComponent<LevelCounter>().count >= 100) {

            foreach (GameObject keypad in keypads) {
                keypad.GetComponent<Keypad>().AccessDenied();
            }
        }
    }

    [ClientRpc]
    private void ButtonClickedClientRPC()
    {
        if (IsOwner)
        {
            bool detected = false;

            foreach (GameObject trigger in triggers)
            {

                if (trigger.GetComponent<ButtonSensor>().isClose)
                {
                    detected = true;
                }
            }

            if (detected)
            {
                ButtonHit();
                ButtonToServerRPC();
            }
        }
    }

    public void ButtonHit() {
        levelCounter.GetComponent<LevelCounter>().ResetCount();

        foreach (GameObject keypad in keypads) {
            StartCoroutine(keypad.GetComponent<Keypad>().DisplayResultRoutine(true));
        }
    }

    [ServerRpc]
    private void ButtonToServerRPC()
    {
        ButtonHit();
    }


}
