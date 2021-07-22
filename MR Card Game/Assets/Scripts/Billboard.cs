using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    // Define the camera
    public Transform cam;

    // LateUpdate is called after the regular update
    void LateUpdate()
    {
        transform.LookAt(transform.position + Board.camera.forward);
    }
}
