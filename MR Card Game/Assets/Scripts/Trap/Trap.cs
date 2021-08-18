using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // The method used to detect that the image target entered the game board space
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            // Make the 
        }
    }

    // The method used to detect that the image target left the game board space
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            //
        }
    }
}
