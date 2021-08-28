using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class DownloadLevel : MonoBehaviour
{
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

            // // Get the array of all levels
            // string[] levelArray = BackendConnector.GetLevels();

            // // Check if the level name exist there
            // if(levelArray.Contains(levelNameInputField.text) == true)
            // {
            //     // Enable the name already exists error message
            //     errorDoesNotExist.gameObject.SetActive(true);

            //     // Make sure the error message is disabled
            //     errorDownloadFailed.gameObject.SetActive(false);

            // } else{

                // // Download the level and get a truthvalue of if it worked
                // bool downloadWorked = DownloadLevelMethod();

                // // Check if the upload did not work
                // if(downloadWorked == false)
                // {
                //     // Enable the error message
                //     errorDownloadFailed.gameObject.SetActive(true);

                // } else {
                //     // Make sure the error message is disabled
                //     errorDownLoadFailed.gameObject.SetActive(false);
                // }
            // }
        }
    }

    // // The method used to upload a level
    // public bool DownloadLevelMethod()
    // {
    //     // Read the level name / code in the input field
    //     string levelName = levelNameInputField.text;

    //     // Get the array of all levels
    //     string[] levelArray = BackendConnector.GetLevels();

    //     // Get the array of files
    //     string[] filePaths = Directory.GetFiles(Globals.currentPath);

    //     // Initialize the successful flag
    //     bool successful = true;

    //     // Download each file in the level
    //     foreach(string fileName in levelArray)
    //     {
    //         // Download that file at the right place
    //         bool success = BackendConnector.Load(levelName, fileName, file);

    //         // Check if it was a success
    //         if(success == false)
    //         {
    //             // If it was not a success, set the successful flag to false
    //             successful = false;
    //         }
    //     }

    //     // Check if the process was unsuccessful
    //     if(successful == false)
    //     {
    //         // Delete everything TODO
    //     }

    //     // Return the successful flag
    //     return successful;
    //     }
    // }
}
