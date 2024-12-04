using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmitter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            StartCoroutine("TriggerTestEvent");

            
        }

        // added for 09/07/2022 class

        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine("TriggerChangeColorEvent");
        }
    }

    IEnumerator TriggerTestEvent()
    {
        Debug.Log("Is that how a warped brain like yours gets its kicks? By planning the death of innocent people?");

        yield return new WaitForSeconds(0.2f);

        EventManager.TriggerEvent("Test");

        
    }

    IEnumerator TriggerChangeColorEvent()
    {
        Debug.Log("Come, join my side to save humanity!");

        yield return new WaitForSeconds(0.2f);

        EventManager.TriggerEvent("ChangeColor");

        AdvancedEventManager.TriggerEvent("ChangeColor", Color.green);

        yield return new WaitForSeconds(0.4f);

        AdvancedEventManager.TriggerEvent("ChangeColor", Color.blue);

    }
}