using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelCounter : MonoBehaviour
{

    public GameObject doorClient;
    private bool hasDoor = false; 

    public TMP_Text m_Text;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
       m_Text = GetComponent<TMP_Text>();
       count = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDoor)
        {
            doorClient.GetComponent<DoorController>().ChangeCount(count);
        }
       
    }

    public void ChangeLevel(int level) {

        count += level;
        m_Text.text = count.ToString();

        if (count >= 100) {
            m_Text.color = Color.red;
        }

    }

    public void FindDoor() {
        doorClient = GameObject.FindGameObjectWithTag("DoorClient");
        hasDoor = true;
    }

    public void ResetCount() {
        count = 0;
        m_Text.text = count.ToString();
        m_Text.color = Color.white;
    }

}
