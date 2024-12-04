using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualDoor : MonoBehaviour
{
    public Vector3 startPosition = new Vector3();
    public Vector3 endPosition = new Vector3(); 


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y - 7, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
