using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    // Define the base path
    [SerializeField]
    private string basePath = "saves";

    // Define the backendAddress
    [SerializeField]
    private string backendAddress = "http://localhost";

    // Define the freed port
    [SerializeField]
    private int port = 8080;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method used to access the full backend address
    public string FullBackendAddress
    {
        get { return backendAddress + ":" + port; }
    }

    // Method used to access the full backend address followed by the base path
    public string BackendAPIBaseURL
    {
        get
        {
            return FullBackendAddress + "/" + basePath + "/";
        }
    }
}
