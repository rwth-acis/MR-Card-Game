using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager managerInstance;

    [SerializeField]
    private string basePath = "template";

    public static string getBasePath
    {
        get { return managerInstance.basePath; }
    }

    [SerializeField]
    private string backendAddress = "http://192.168.1.39";

    public static string getBackendAddress
    {
        get { return managerInstance.backendAddress; }
    }

    [SerializeField]
    private int port = 8080;

    public static int GetPort
    {
        get { return managerInstance.port; }
    }

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = this;
    }

    /// <summary>
    /// Access the full backend address
    /// </summary>
    public static string FullBackendAddress
    {
        get { return getBackendAddress + ":" + GetPort; }
    }

    /// <summary>
    /// Access the full backend address followed by the base path
    /// </summary>
    public static string BackendAPIBaseURL
    {
        get
        {
            return FullBackendAddress + "/" + getBasePath + "/";
        }
    }
}
