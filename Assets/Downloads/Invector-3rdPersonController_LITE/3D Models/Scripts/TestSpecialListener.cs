using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSpecialListener : MonoBehaviour
{
    private UnityAction onTest;

    public SpriteRenderer spriteRenderer;
    public Renderer ren3d;

    private void Awake()
    {
        onTest = new UnityAction(OnTest);
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ren3d = this.gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("Test", onTest);

        //added for 09/07/22 class
        //EventManager.StartListening("ChangeColor", OnChangeColor);
        AdvancedEventManager.StartListening("ChangeColor", OnChangeColorAdvanced);

    }

    private void OnDisable()
    {
        EventManager.StopListening("Test", onTest);
        //added for 09/07/22 class
        //EventManager.StopListening("ChangeColor", OnChangeColor);
        AdvancedEventManager.StopListening("ChangeColor", OnChangeColorAdvanced);
    }

    private void OnTest()
    {
        //Debug.Log("No, by causing the death of innocent people.");

        //Debug.Log($"{gameObject.name} has received the message 'test'!");

        Debug.Log($"{gameObject.name} received VERY SPECIAL test!");

        StartCoroutine(RespondToTriggerSpecialEvent(this.gameObject.name));
    }


    IEnumerator RespondToTriggerSpecialEvent(string name)
    {
        yield return null;

        if (name == "Mex")
        {
            yield return new WaitForSeconds(0.3f);

            Debug.Log("Hey guys.. sorry I am late. What's going on?");
        }

    }

    private void OnChangeColor()
    {
        spriteRenderer.color = spriteRenderer.color == Color.blue ? Color.green : Color.blue;


    }

    private void OnChangeColorAdvanced(object data)
    {
        Color c = (Color)data;
        //spriteRenderer.color = c;
        ren3d.material.color = c;
    }
}