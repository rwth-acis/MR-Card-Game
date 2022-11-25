using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    // LateUpdate is called after the regular update
    void LateUpdate()
    {
        transform.LookAt(Board.camera);
        transform.forward = -transform.forward;
    }
}
