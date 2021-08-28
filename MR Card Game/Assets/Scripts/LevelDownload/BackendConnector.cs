using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using i5.Toolkit.Core.Utilities;

// using Microsoft.MixedReality.Toolkit.Utilities;

public class UnityConnector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // // The method used to access the list of all level folders that were uploaded
    // public static async Task<string[]> GetLevels()
    // {
    //     Response resp = await Rest.GetAsync(Manager.Instance.BackendAPIBaseURL + "saves", null, -1, null, true);
    //     Manager.Instance.CheckStatusCode(resp.ResponseCode);

    //     // Check if the operation was a success
    //     if (!resp.Successful)
    //     {
    //         Debug.LogError(resp.ResponseCode + ": " + resp.ResponseBody);
    //         string[] newArray = new string[0];
    //         return newArray;

    //     } else {

    //         // Get the array of levels
    //         string[] levels = JsonArrayUtility.FromJson<string>(resp.ResponseBody);
    //         return levels;
    //     }
    // }

    // // Method used to load the data from the back end, of the file saveName in the level levelName
    // public static async Task<string> Load(string levelName, string saveName)
    // {
    //     // Request the loading and get the response message
    //     Response resp = await Rest.GetAsync(Manager.Instance.BackendAPIBaseURL + "saveData/" + saveName, null, -1, null, true);
    //     Manager.Instance.CheckStatusCode(resp.ResponseCode);

    //     // Check if the process was successful
    //     if (resp.Successful)
    //     {
    //         // Initialize an empty string
    //         string emptyString = "";

    //         // Return an empty string
    //         return emptyString;

    //     } else {

    //         // Return the string of the json file
    //         return new string(resp.ResponseCode, resp.ResponseBody);
    //     }
    // }
}
