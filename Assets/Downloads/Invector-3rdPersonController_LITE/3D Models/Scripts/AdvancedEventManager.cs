using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

//added for 09/07/22 class
[System.Serializable]
public class TypedEvent : UnityEvent<object> { }

public class AdvancedEventManager : MonoBehaviour
{

    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, TypedEvent> typedEventDictionary;

    private static AdvancedEventManager eventManager;

    public static AdvancedEventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(AdvancedEventManager)) as AdvancedEventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            typedEventDictionary = new Dictionary<string, TypedEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            instance.typedEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventManager == null) return;
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, object data)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}