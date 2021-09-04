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
    // The flag that states if an upload is a success or not
    public static bool successful;

    public static int numberOfFilesToDownload;
}

public class DownloadLevel : MonoBehaviour
{
    // Define the add level window
    [SerializeField]
    private GameObject addLevelWindow;

    // Define the level name input field
    [SerializeField]
    private TMP_InputField levelNameInputField;

    // Define the error messages
    [SerializeField]
    private TMP_Text errorDoesNotExist;
    [SerializeField]
    private TMP_Text errorEmptyName;
    [SerializeField]
    private TMP_Text errorSpecialCharacters;
    [SerializeField]
    private TMP_Text errorDownloadFailed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //----------------------------------------------------------------------------------------------------
    // The get and post requests
    //----------------------------------------------------------------------------------------------------

    // The get requestion coroutine to get levels
    IEnumerator CheckIfLevelExists(string path)
    {
        Debug.Log("The request was send with the uri: " + Manager.BackendAPIBaseURL + path);

        // Send the get request with the base url plus the given path
        UnityWebRequest uwr = UnityWebRequest.Get(Manager.BackendAPIBaseURL + path);

        // Wait for the answer to come
        yield return uwr.SendWebRequest();

        // Check if there was a network error
        if (uwr.isNetworkError)
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
            if(isContained(levelArray, levelNameInputField.text) == true)
            {
                // Enable the name already exists error message
                errorDoesNotExist.gameObject.SetActive(true);

                // Make sure the error message is disabled
                errorDownloadFailed.gameObject.SetActive(false);

                Debug.Log("Level with that name was already contained");

            } else {

                // Access the level files and then download it
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

        // Wait for the answer to come
        yield return uwr.SendWebRequest();

        // Check if there was a network error
        if (uwr.isNetworkError)
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
            if(isContained(levelfilesArray, levelNameInputField.text) == true)
            {
                // Enable the name already exists error message
                errorDoesNotExist.gameObject.SetActive(true);

                // Make sure the error message is disabled
                errorDownloadFailed.gameObject.SetActive(false);

                Debug.Log("Level with that name was already contained");

            } else {

                // Upload the level and get a truthvalue of if it worked
                bool downloadWorked = DownloadLevelMethod(levelfilesArray);

                // Wait for all files to be downloaded
                while (Download.numberOfFilesToDownload != 0) 
                {
                    yield return new WaitForSeconds(0.5f);
                }

                // Check if the upload did not work
                if(downloadWorked == false)
                {
                    // Enable the error message
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

        // Wait for the response to come
        yield return uwr.SendWebRequest();

        // Check if there was an error
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);

            // Set the flag that the download is successful to false
            Download.successful = false;
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            // byte[] byteArray = (byte[])uwr.downloadHandler.text;

            // // Convert the byte array to string
            // string data = System.Text.Encoding.UTF8.GetString(uwr.downloadHandler.data);

            // Save the string retrieved in a file of the right name
            File.WriteAllText(Globals.currentPath + fileName + ".json", uwr.downloadHandler.text);
        }

        Download.numberOfFilesToDownload = Download.numberOfFilesToDownload - 1;
    }

    //----------------------------------------------------------------------------------------------------
    // The methods that interact with the server
    //----------------------------------------------------------------------------------------------------

    // Method used to try downloading a level
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

    // The method used to upload a level
    public bool DownloadLevelMethod(string[] levelFiles)
    {
        // Read the level name / code in the input field
        string levelName = levelNameInputField.text;

        // Download each file in the level
        foreach(string fileName in levelFiles)
        {
            // Download that file at the right place
            StartCoroutine(GetRequest(levelName + "/" + fileName, fileName));
        }

        // Make sure the download successful flag is set to true
        Download.successful = true;

        // Check if the process was unsuccessful
        if(Download.successful == false)
        {
            // Delete everything TODO
            Debug.Log("The download was unsuccessful");
        }

        // Return the successful flag
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

    // Method used to extract an array out of the string passed by the get request
    public string[] GetTheArray(string data)
    {
        // Extract the LevelDirectories object
        LevelDirectories levelDirectories = JsonUtility.FromJson<LevelDirectories>(data);

        // Initialize an array of the same length as the array
        string[] levelNames = new string[levelDirectories.array.Length];

        // Initialize the current index
        int index = 0;

        // Extract the directory names (currently complete paths)
        foreach(string level in levelDirectories.array)
        {
            // Get the name of the file and save it in the level names array
            levelNames[index] = Path.GetFileName(levelDirectories.array[index]);

            // Increase the index by one
            index = index + 1;
        }

        // Return the level names array
        return levelNames;
    }

    // Method that checks if a string is contained in an array of strings
    public bool isContained(string[] array, string name)
    {
        // Go through all strings of the array
        foreach (string modelName in array)
        {
            // Check if the current name and the given name are the same
            if(modelName == name)
            {
                return true;
            }
        }

        return false;
    }
}