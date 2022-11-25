using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

static class Download
{
    /// <summary>
    /// If an upload is a success or not
    /// </summary>
    public static bool successful;

    /// <summary>
    /// The number of files that are needed to be downloaded in the level
    /// </summary>
    public static int numberOfFilesToDownload;
}

public class DownloadLevel : MonoBehaviour
{
    [Tooltip("add level window")]
    [SerializeField]
    private GameObject addLevelWindow;

    [Tooltip("level name input field")]
    [SerializeField]
    private TMP_InputField levelNameInputField;

    [Header("Error Messages")]
    [SerializeField]
    private TMP_Text errorDoesNotExist;
    [SerializeField]
    private TMP_Text errorEmptyName;
    [SerializeField]
    private TMP_Text errorSpecialCharacters;
    [SerializeField]
    private TMP_Text errorDownloadFailed;

    //----------------------------------------------------------------------------------------------------
    // The get and post requests
    //----------------------------------------------------------------------------------------------------

    // The get requestion coroutine to get levels
    IEnumerator CheckIfLevelExists(string path)
    {
        Debug.Log("The request was send with the uri: " + Manager.BackendAPIBaseURL + path);

        // Send the get request with the base url plus the given path
        UnityWebRequest uwr = UnityWebRequest.Get(Manager.BackendAPIBaseURL + path);

        yield return uwr.SendWebRequest();

        // Check if there was a network error
        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);

        } else {

            Debug.Log("Received: " + uwr.downloadHandler.text);
            
            // Get the level array
            string[] levelArray = GetTheArray(uwr.downloadHandler.text);

            foreach(string level in levelArray)
            {
                Debug.Log("Level found: " + level);
            }

            // Check if the level name exist there
            if(IsContainedInArray(levelArray, levelNameInputField.text) == true)
            {
                errorDoesNotExist.gameObject.SetActive(true);

                // Make sure the error message is disabled
                errorDownloadFailed.gameObject.SetActive(false);

                Debug.Log("Level with that name was already contained");

            } else {
                StartCoroutine(AccessLevelFiles(levelNameInputField.text));
            }
        }
    }

    // The get requestion coroutine to get levels
    IEnumerator AccessLevelFiles(string path)
    {
        Debug.Log("The request was send with the uri: " + Manager.BackendAPIBaseURL + path);

        // Send the get request with the base url plus the given path
        UnityWebRequest uwr = UnityWebRequest.Get(Manager.BackendAPIBaseURL + path);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);

        } else {

            Debug.Log("Received: " + uwr.downloadHandler.text);
            
            // Get the level files array
            string[] levelfilesArray = GetTheArray(uwr.downloadHandler.text);

            // Set the number of files to download to the length of the level files array
            Download.numberOfFilesToDownload = levelfilesArray.Length;

            foreach(string levelFile in levelfilesArray)
            {
                Debug.Log("Level file found: " + levelFile);
            }

            // Check if the level name exist there
            if(IsContainedInArray(levelfilesArray, levelNameInputField.text) == true)
            {
                errorDoesNotExist.gameObject.SetActive(true);
                // Make sure the error message is disabled
                errorDownloadFailed.gameObject.SetActive(false);

                Debug.Log("Level with that name was already contained");

            } else {
                bool downloadSucessful = DownloadLevelMethod(levelfilesArray);
                while (Download.numberOfFilesToDownload != 0) 
                {
                    yield return new WaitForSeconds(0.5f);
                }
                if(downloadSucessful == false)
                {
                    errorDownloadFailed.gameObject.SetActive(true);

                } else {

                    // Make sure the error message is disabled
                    errorDownloadFailed.gameObject.SetActive(false);
                    CloseWindow();
                }
            }
        }
    }

    // The get request coroutine
    IEnumerator GetRequest(string path, string fileName)
    {
        Debug.Log("The request was send with the uri: " + Manager.BackendAPIBaseURL + path);

        // Send the unity web request
        UnityWebRequest uwr = UnityWebRequest.Get(Manager.BackendAPIBaseURL + path);

        yield return uwr.SendWebRequest();

        // Check if there was an error
        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);

            // Set the flag that the download is successful to false
            Download.successful = false;
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            string realFileName = fileName + ".json";

            // Save the string retrieved in a file of the right name
            File.WriteAllText(Path.Combine(Globals.currentPath, realFileName), uwr.downloadHandler.text);
        }

        Download.numberOfFilesToDownload = Download.numberOfFilesToDownload - 1;
    }

    IEnumerator SendPingCoroutine()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(Manager.BackendAPIBaseURL + "write");

        // Wait for the response to come
        yield return uwr.SendWebRequest();
    }

    //----------------------------------------------------------------------------------------------------
    // The methods that interact with the server
    //----------------------------------------------------------------------------------------------------

    // Method that sends a ping at the server
    public void SendPing()
    {
        StartCoroutine(SendPingCoroutine());
    }

    /// <summary>
    /// Try downloading a level
    /// </summary>
    public void TryDownloadingLevel()
    {
        // Check if the input field is empty
        if(levelNameInputField.text == "")
        {
            // Display the empty name error message
            errorEmptyName.gameObject.SetActive(true);
            errorSpecialCharacters.gameObject.SetActive(false);
            errorDownloadFailed.gameObject.SetActive(false);
            errorDoesNotExist.gameObject.SetActive(false);

        } else if(Regex.IsMatch(levelNameInputField.text, @"^[a-zA-Z0-9]+$") == false)
        {
            // Activate the special character error message and deactivate the others
            errorEmptyName.gameObject.SetActive(false);
            errorSpecialCharacters.gameObject.SetActive(true);
            errorDownloadFailed.gameObject.SetActive(false);
            errorDoesNotExist.gameObject.SetActive(false);

        } else {
            
            // Deactivate the error messages
            errorEmptyName.gameObject.SetActive(false);
            errorSpecialCharacters.gameObject.SetActive(false);

            StartCoroutine(CheckIfLevelExists(levelNameInputField.text));
        }
    }

    /// <summary>
    /// Download a level
    /// </summary>
    public bool DownloadLevelMethod(string[] levelFiles)
    {
        string levelName = levelNameInputField.text;
        // Make sure the download successful flag is set to true
        Download.successful = true;

        // Download each file in the level
        foreach (string fileName in levelFiles)
        {
            StartCoroutine(GetRequest(levelName + "/" + fileName, fileName));
        }

        if(Download.successful == false)
        {
            Debug.Log("The download was unsuccessful");

            System.IO.DirectoryInfo directory = new DirectoryInfo(Globals.currentPath);

            // Go through all files in the current path
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete(); 
            }
        }
        return Download.successful;
    }

    public void CloseWindow()
    {
        // Close the menu and return to the main menu
        addLevelWindow.gameObject.SetActive(false);

        // Reset the add level menu by setting the flag to true
        Globals.resetAddLevelMenu = true;
    }

    //---------------------------------------------------------------------------------------------------------------------
    // Helper Methods
    //---------------------------------------------------------------------------------------------------------------------

    // The JSON Serialization for the input questions
    [Serializable]
    public class LevelDirectories
    {
        public string[] array;
    }

    /// <summary>
    /// Extract an array out of the string passed by the get request
    /// </summary>
    /// <returns>the level names array</returns>
    public string[] GetTheArray(string data)
    {
        // Check if there are no levels
        if(data != "null")
        {
            // Extract the LevelDirectories object
            LevelDirectories levelDirectories = JsonUtility.FromJson<LevelDirectories>(data);

            // Initialize an array of the same length as the array
            string[] levelNames = new string[levelDirectories.array.Length];
            int index = 0;

            // Extract the directory names (currently complete paths)
            foreach(string level in levelDirectories.array)
            {
                // Get the name of the file and save it in the level names array
                levelNames[index] = Path.GetFileName(levelDirectories.array[index]);
                index++;
            }
            return levelNames;

        } else {

            string[] levelNames = new string[1];
            levelNames[0] = "";
            return levelNames;
        }
    }

    /// <summary>
    /// If a string is contained in an array of strings
    /// </summary>
    public bool IsContainedInArray(string[] array, string name)
    {
        foreach (string modelName in array)
        {
            if(modelName == name)
            {
                return true;
            }
        }
        return false;
    }
}