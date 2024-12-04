using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestListener : MonoBehaviour
{
    private UnityAction onTest;

    private void Awake()
    {
        onTest = new UnityAction(OnTest);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Test", onTest);
        
    }

    private void OnDisable()
    {
        EventManager.StopListening("Test", onTest);
    }

    private void OnTest()
    {
        //Debug.Log("No, by causing the death of innocent people.");
        

        Debug.Log($"{gameObject.name} has received the message 'test'!");

        StartCoroutine(RespondToTriggerEvent(this.gameObject.name));
    }


    IEnumerator RespondToTriggerEvent(string name)
    {
        yield return null;

        if (name == "Lex")
        {
            Debug.Log("No, by causing the death of innocent people.");
        }
        else if (name == "Hex")
        {
            yield return new WaitForSeconds(0.2f);

            Debug.Log("No, Lex! That's my dialogue..");
        }
        



    }
}