using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BrowsLevels : MonoBehaviour
{
    // The path to the root directory where all levels are saved locally
    private string rootDirectoryPath;

    // The current path
    private string currentPath;

    // The number of directories in the folder
    private int numberOfDirectories;

    // The directories array
    private string[] directoriesArray;

    // The current browsing depth
    private int depth;

    // The number of pages that can be displayed in this direcory
    private int numberOfPages;

    // The number of the current page
    private int currentPage;

    // The flag that must be risen if a user want to click on a directory, to ensure the user doesn't go more than one directory per click
    private bool flagVariable;

    // Here menus and buttons are defined
    public GameObject mainMenu;
    public GameObject browsDirectoriesMenu;

    // Define the page x / y text of the brows directories menu
    public TextMeshProUGUI currentPageText;

    // Define the model preview buttons
    public Button directory1;
    public Button directory2;
    public Button directory3;
    public Button directory4;
    public Button directory5;

    // Define the previous an next buttons
    public Button previousPage;
    public Button nextPage;

    // Define the return button
    public Button returnOneUp;

    // Define the launch level button
    public Button selectLevel;

    [SerializeField]
    private Sprite[] switchSprites;
    private Image switchImage;

    // Start is called before the first frame update
    void Start()
    {
        // First I initialize the Global paths
        string scriptPath = GetCurrentFilePath();
        rootDirectoryPath = GetPathToRootDirectory(scriptPath);
        currentPath = rootDirectoryPath;
        depth = 1;

        // Then I actualize in a function the directories, page numbers, heading
        ActualizeGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(currentPath);
    }

    // Helper method to get the path to this script file
    string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string fileName = null)
    {
        return fileName;
    }

    // Method that returns you the path to this script
    private string GetCurrentFilePath()
    {
        string scriptPath = GetCurrentFileName();
        return scriptPath;
        
    }

    // Method that returns you the path to the root directory of the directory structure saved in the back end
    private string GetPathToRootDirectory(string scriptPath)
    {
        string rootPath = Path.GetFullPath(Path.Combine(scriptPath, @"..\..\..\..\..\"));
        string rootDirectoryPath = Path.GetFullPath(Path.Combine(rootPath, @"Backend\Directories\"));
        return rootDirectoryPath;
    }

    // Method that returns the array of directories in the current directory
    public string[] GetDirectoriesArray() 
    {
        string[] dirs = Directory.GetDirectories(currentPath, "*", SearchOption.TopDirectoryOnly);
        return dirs;
    }

    // Method that returns the array of files in the given path to a directory
    public string[] GetFilesArray()
    {
        string[] files = Directory.GetFiles(currentPath, "Question*");

        // Check if the description file exists
        if (File.Exists(currentPath + "Description.json")) 
        {

            // Case it exists
            string[] description = Directory.GetFiles(currentPath, "Description.json");

            // Get the length of the files array
            int length = 0;
            foreach(string file in files)
            {
                length = length + 1;
            }
            length = length + 1;

            // Create a new array that can contain all files
            string[] array = new string[length];

            // Copy the description in the first slot
            array[0] = description[0];
            int index = 1;

            // Append all elements in the files array to the description array
            foreach(string file in files)
            {
                array[index] = file;
                index = index + 1;
            }

            // Return the array that contains the description and the questions
            return array;

        } else {

            // Case the description file does not exist
            return files;
        }
    }

    // Method that returns the number of directories in the current directory
    public int GetNumberOfDirectories(string[] dirs) 
    {
       int number = 0;
       foreach (string dir in dirs) {
           number = number + 1;
       }
       return number;
    }

    // Method that returns the number of directories in the current directory
    public int GetNumberOfFiles(string[] files) 
    {
       int number = 0;
       foreach (string file in files) {
           number = number + 1;
       }
       return number;
    }

    // Disabling or enabling of the buttons
    public void DisableOrEnableButtons()
    {
        // Define the disabled color gradient
        VertexGradient disabledTextGradient;
        disabledTextGradient.bottomLeft = new Color32(99, 101, 102, 150);
        disabledTextGradient.bottomRight = new Color32(99, 101, 102, 150);
        disabledTextGradient.topLeft = new Color32(99, 101, 102, 255);
        disabledTextGradient.topRight = new Color32 (99, 101, 102, 255);

        // Define the enabled color gradient
        VertexGradient enabledTextGradient;
        enabledTextGradient.bottomLeft = new Color32(0, 84, 159, 255);
        enabledTextGradient.bottomRight = new Color32(0, 84, 159, 255);
        enabledTextGradient.topLeft = new Color32(64, 127, 183, 255);
        enabledTextGradient.topRight = new Color32 (64, 127, 183, 255);

        // Enable / Disable previous button and change color
        TMP_Text textPrevious = previousPage.GetComponentInChildren<TMP_Text>();
        if(currentPage == 1)
        {
            previousPage.interactable = false;
            textPrevious.GetComponent<TMP_Text>().colorGradient = disabledTextGradient;
        } else {
            previousPage.interactable = true;
            textPrevious.GetComponent<TMP_Text>().colorGradient = enabledTextGradient;
        }

        // Enable / Disable next button and change color
        TMP_Text textNext = nextPage.GetComponentInChildren<TMP_Text>();
        if(currentPage != numberOfPages)
        {
            nextPage.interactable = true;
            textNext.GetComponent<TMP_Text>().colorGradient = enabledTextGradient;
        } else {
            nextPage.interactable = false;
            textNext.GetComponent<TMP_Text>().colorGradient = disabledTextGradient;
        }

        // Enable / disable the return button
        switchImage = returnOneUp.image;
        if(currentPath != rootDirectoryPath)
        {
            //returnButtonOn.interactable = true;
            returnOneUp.interactable = true;
            switchImage.sprite = switchSprites[1];
        } else {
            //returnButtonOff.interactable = false;
            returnOneUp.interactable = false;
            switchImage.sprite = switchSprites[0];
        }
    }

    // Method that creates the buttons depending of the directory we are currently in
    public void RenameButtons(string path)
    {

        // Case there are no directories to be displayed
        if(numberOfDirectories == 0)
        {
            // // If select directory mode is on, then enable the select button
            // if(Menus.directorySelection == true)
            // {
            //     selectButton.interactable = true;
            // }

            // // Get the array of all files
            // Globals.fileArray = GetFilesArray();
            // int numberOfFiles = GetNumberOfFiles(Globals.fileArray);

            // // Case there are files in the folder
            // if(numberOfFiles != 0)
            // {
            //     // Disable the create directory button
            //     createDirectory.interactable = false;

            //     // Set the flag
            //     Globals.theseAreFiles = true;
            //     // First rename the buttons that should have button names, check that they are enabled
            //     // for that initialize the range of the for loop

            //     // Value at the begining of the for loop
            //     int initialIndex = (Globals.currentPage - 1) * 5;
            //     // counter for the assigning of a button
            //     int currentFileNumber = 1;
            //     // Value for the end of the for loop (for the renaming loop)
            //     int lastIndex = 0;

            //     if(numberOfFiles <= (Globals.currentPage) * 5)
            //     {
            //         lastIndex = numberOfFiles - 1;
            //     } else {
            //         lastIndex = Globals.currentPage * 5 - 1;
            //     }
            //     // Last index that would correspond to the fifth directory if the array was full enough (for the deleting names loop)
            //     int lastEmptyIndex = (Globals.currentPage) * 5 - 1;

            //     for(int currentIndex = initialIndex; currentIndex <= lastIndex; currentIndex = currentIndex + 1)
            //     {
            //         string file = Globals.fileArray[currentIndex];
            //         string lastFileName = Path.GetFileName(file);

            //         // Get the question name form the file
            //         string questionName = ExtractQuestionName(file);

            //         switch (currentFileNumber)
            //         {
            //             case 1:
            //                 Button directory1 = GameObject.Find("Directory1").GetComponent<Button>();
            //                 directory1.GetComponentInChildren<TMP_Text>().text = questionName;
            //                 directory1.interactable = true;
            //             break;
            //             case 2:
            //                 Button directory2 = GameObject.Find("Directory2").GetComponent<Button>();
            //                 directory2.GetComponentInChildren<TMP_Text>().text = questionName;
            //                 directory2.interactable = true;
            //             break;
            //             case 3:
            //                 Button directory3 = GameObject.Find("Directory3").GetComponent<Button>();
            //                 directory3.GetComponentInChildren<TMP_Text>().text = questionName;
            //                 directory3.interactable = true;
            //             break;
            //             case 4:
            //                 Button directory4 = GameObject.Find("Directory4").GetComponent<Button>();
            //                 directory4.GetComponentInChildren<TMP_Text>().text = questionName;
            //                 directory4.interactable = true;
            //             break;
            //             case 5:
            //                 Button directory5 = GameObject.Find("Directory5").GetComponent<Button>();
            //                 directory5.GetComponentInChildren<TMP_Text>().text = questionName;
            //                 directory5.interactable = true;
            //             break;
            //         }
            //         currentFileNumber = currentFileNumber + 1;
            //     }
            //     if(currentFileNumber != 5)
            //     {
            //         for(int counter = numberOfFiles; counter <= lastEmptyIndex; counter = counter + 1)
            //         {
            //             switch (currentFileNumber)
            //             {
            //                 case 2:
            //                     Button directory2 = GameObject.Find("Directory2").GetComponent<Button>();
            //                     directory2.GetComponentInChildren<TMP_Text>().text = "";
            //                     directory2.interactable = false;
            //                 break;
            //                 case 3:
            //                     Button directory3 = GameObject.Find("Directory3").GetComponent<Button>();
            //                     directory3.GetComponentInChildren<TMP_Text>().text = "";
            //                     directory3.interactable = false;
            //                 break;
            //                 case 4:
            //                     Button directory4 = GameObject.Find("Directory4").GetComponent<Button>();
            //                     directory4.GetComponentInChildren<TMP_Text>().text = "";
            //                     directory4.interactable = false;
            //                 break;
            //                 case 5:
            //                     Button directory5 = GameObject.Find("Directory5").GetComponent<Button>();
            //                     directory5.GetComponentInChildren<TMP_Text>().text = "";
            //                     directory5.interactable = false;
            //                 break;
            //             }
            //             currentFileNumber = currentFileNumber + 1;
            //         }
            //     }

            // Case there are no files in the folder
            // } else {

            // directory1.GetComponentInChildren<TMP_Text>().text = "";
            // directory1.interactable = false;

            // directory2.GetComponentInChildren<TMP_Text>().text = "";
            // directory2.interactable = false;

            // directory3.GetComponentInChildren<TMP_Text>().text = "";
            // directory3.interactable = false;

            // directory4.GetComponentInChildren<TMP_Text>().text = "";
            // directory4.interactable = false;

            // directory5.GetComponentInChildren<TMP_Text>().text = "";
            // directory5.interactable = false;
            // }

        // Case there is at least one directory, then display the numbers 5*x + 1 to 5*x + 5 (x is number of the page)
        } else {

            // First rename the buttons that should have button names, check that they are enabled
            // for that initialize the range of the for loop

            // Value at the begining of the for loop
            int initialIndex = (currentPage - 1) * 5;
            // counter for the assigning of a button
            int currentDirectoryNumber = 1;
            // Value for the end of the for loop (for the renaming loop)
            int lastIndex = 0;
            if(numberOfDirectories <= (currentPage) * 5)
            {
                lastIndex = numberOfDirectories - 1;
            } else {
                lastIndex = currentPage * 5 - 1;
            }
            // Last index that would correspond to the fifth directory if the array was full enough (for the deleting names loop)
            int lastEmptyIndex = (currentPage) * 5 - 1;

            for(int currentIndex = initialIndex; currentIndex <= lastIndex; currentIndex = currentIndex + 1)
            {
            //each (string dir in Globals.directoriesArray) {
                string dir = directoriesArray[currentIndex];
                string lastFolderName = Path.GetFileName(dir);
                switch (currentDirectoryNumber)
                {
                    case 1:
                        directory1.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                        directory1.interactable = true;
                    break;
                    case 2:
                        directory2.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                        directory2.interactable = true;
                    break;
                    case 3:
                        directory3.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                        directory3.interactable = true;
                    break;
                    case 4:
                        directory4.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                        directory4.interactable = true;
                    break;
                    case 5:
                        directory5.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                        directory5.interactable = true;
                    break;
                }
                currentDirectoryNumber = currentDirectoryNumber + 1;
            }
            if(currentDirectoryNumber != 5)
            {
                for(int counter = numberOfDirectories; counter <= lastEmptyIndex; counter = counter + 1)
                {
                    switch (currentDirectoryNumber)
                    {
                        case 2:
                            directory2.GetComponentInChildren<TMP_Text>().text = "";
                            directory2.interactable = false;
                        break;
                        case 3:
                            directory3.GetComponentInChildren<TMP_Text>().text = "";
                            directory3.interactable = false;
                        break;
                        case 4:
                            directory4.GetComponentInChildren<TMP_Text>().text = "";
                            directory4.interactable = false;
                        break;
                        case 5:
                            directory5.GetComponentInChildren<TMP_Text>().text = "";
                            directory5.interactable = false;
                        break;
                    }
                    currentDirectoryNumber = currentDirectoryNumber + 1;
                }
            }
        }
    }

    // Method that is activated when pressing next (change the other directories)
    public void NextPage(){
        currentPage = currentPage + 1;
        DisableOrEnableButtons();
        RenameButtons(currentPath);
        GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing previous (change the other directories)
    public void PreviousPage(){
        currentPage = currentPage - 1;
        DisableOrEnableButtons();
        RenameButtons(currentPath);
        GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing the return arrow (get to the parent directory)
    public void ReturnOneUp()
    {
        // First we need to actualize the current path
        currentPath = Path.GetFullPath(Path.Combine(currentPath, @"..\"));
        
        // Then we can actualize everything
        ActualizeGlobals();
        DisableOrEnableButtons();
        RenameButtons(currentPath);
    }

    IEnumerator FreezeCoroutine()
    {
        // Print the time of when the function is first called.
        flagVariable = false;

        // Yield on a new YieldInstruction that waits for 0.5 seconds.
        yield return new WaitForSeconds(0.500F);

        //After we have waited 5 seconds print the time again.
        flagVariable = true;
    }

    // Method used to navigate in directories, when clicking on one visible directory
    public void NavigateDirectories()
    {
        // get the name of the button that was pressed and the button
        string name = EventSystem.current.currentSelectedGameObject.name;
        Button button = GameObject.Find(name).GetComponent<Button>();
        string fileName = button.GetComponentInChildren<TMP_Text>().text;

        if(flagVariable == true)
        {
            // Increase the browsing depth
            depth = depth + 1;

            // Get the name of the directory selected
            string directory = button.GetComponentInChildren<TMP_Text>().text;

            // Actualize the path
            currentPath = currentPath + directory + @"\";

            // Actualize the other globals (directories array, number, page number, etc)
            ActualizeGlobals();
            DisableOrEnableButtons();
            RenameButtons(currentPath);
            StartCoroutine(FreezeCoroutine());
        }

        // // Case theses are files, and it was not the description that was clicked
        // if(flagVariable == true && theseAreFiles == true && fileName != "Description")
        // {

        //     // Activate the creator menu
        //     creatorMenu.SetActive(true);

        //     // Set the path to save to that directory
        //     savePathField.text = Globals.currentPath;
        //     Globals.selectedPath = Globals.currentPath;

        //     // Get the button and the index
        //     string questionName = button.GetComponentInChildren<TMP_Text>().text;
        //     int buttonIndex = GetIndexFromButtonName(name);

        //     Debug.Log("Current button index : " + buttonIndex);

        //     // Get the number of the index of the file
        //     int fileIndex = (Globals.currentPage - 1) * 5 + buttonIndex;

        //     // Get the right path to the file
        //     string filePath = Globals.fileArray[fileIndex];
        //     Debug.Log("path to file: " + filePath);
        //     Debug.Log("name of the file: " + questionName);

        //     // Set the file name, so that it can be loaded back with the same name
        //     Globals.filePath = filePath;
        //     Globals.fileName = Path.GetFileName(filePath);

        //     // Load the selected question in the temp save file
        //     File.Copy(filePath, Path.Combine(Globals.tempSavePath, Path.GetFileName(filePath)));

        //     // Add the name of the exercise to the preview and enable it
        //     previewQuestion1.GetComponentInChildren<TMP_Text>().text = questionName;
        //     previewQuestion1.interactable = true;
        //     Debug.Log("PreviewQuestion1 button is now interactable.");

        //     // Get the json string
        //     string json = File.ReadAllText(filePath);

        //     // Find our what kind of question it is
        //     if(json.Contains("input question") == true)
        //     {
        //         // Extract the object
        //         InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

        //         // Load all model files
        //         if(question.numberOfModels >= 1)
        //         {
        //             // Load the first model file
        //             File.Copy(Path.Combine(Globals.selectedPath, question.model1Name), Path.Combine(Globals.tempSavePath, question.model1Name));
        //         }
        //         if(question.numberOfModels >= 2)
        //         {
        //             // Load the second model file
        //             File.Copy(Path.Combine(Globals.selectedPath, question.model2Name), Path.Combine(Globals.tempSavePath, question.model2Name));
        //         }
        //         if(question.numberOfModels >= 3)
        //         {
        //             // Load the third model file
        //             File.Copy(Path.Combine(Globals.selectedPath, question.model3Name), Path.Combine(Globals.tempSavePath, question.model3Name));
        //         }
        //         if(question.numberOfModels >= 4)
        //         {
        //             // Load the fourth model file
        //             File.Copy(Path.Combine(Globals.selectedPath, question.model4Name), Path.Combine(Globals.tempSavePath, question.model4Name));
        //         }
        //         if(question.numberOfModels == 5)
        //         {
        //             // Load the fifth model file
        //             File.Copy(Path.Combine(Globals.selectedPath, question.model5Name), Path.Combine(Globals.tempSavePath, question.model5Name));
        //         }

        //         // Get the models array
        //         string[] modelArray = GetModelsArray(Globals.tempSavePath);

        //         int index = 0;

        //         // Set the models
        //         foreach(string model in modelArray)
        //         {
        //             // Get the json string
        //             string jsonModel = File.ReadAllText(model);

        //             // Extract the object
        //             Model modelObject = JsonUtility.FromJson<Model>(jsonModel);

        //             // Get the right button
        //             Button previewButton = GetRightModelPreviewButton(index);

        //             // Set the name of the button correctly
        //             previewButton.GetComponentInChildren<TMP_Text>().text = modelObject.modelName;

        //             // Make it interactable
        //             previewButton.interactable  = true;

        //             // Increase index by one
        //             index = index + 1;
        //         }

        //         // Set the rest of the buttons correctly
        //         for(int rest = index; rest < 5; rest = rest + 1)
        //         {
        //             // Get the right button
        //             Button previewButton = GetRightModelPreviewButton(rest);
        //             if(rest == index)
        //             {
        //                 // Set the name of the button correctly
        //                 previewButton.GetComponentInChildren<TMP_Text>().text = "+";
        //                 previewButton.interactable  = true;
        //             } else {
        //                 // Set the name of the button correctly
        //                 previewButton.GetComponentInChildren<TMP_Text>().text = "";
        //                 previewButton.interactable  = false;
        //             }
        //         }
        //     }

        //     // Here change the button from "save" to "change"
        //     saveButton.gameObject.SetActive(false);
        //     changeButton.gameObject.SetActive(true);

        //     // Disable the add button and brows directories button so that no additional question can be created and the path can't be changed.
        //     addButton.interactable = false;
        //     browsDirectoriesButton.interactable = false;

        //     // Set the flag that a file is currently beeing changed (since it changes the index of the file in the buttons of preview in the creator menu)
        //     Globals.currentlyChangingFile = true;

        //     // Reset the menu so that next time the user accesses the selection is back at the root directory
        //     resetBrowsDirectories();

        //     // Deactivate the brows directories menu
        //     browsDirectoriesMenu.SetActive(false);
        // }

        // // Case theses are files, and it was the description that was clicked
        // if(Globals.flagVariable == true && Globals.theseAreFiles == true && fileName == "Description")
        // {
        //     // Activate the window to enter a description
        //     ActivateEnterDescriptionWindow();

        //     // Activate the change button and deactivate the ok button
        //     changeDescription.gameObject.SetActive(true);
        //     okDescription.gameObject.SetActive(false);
        //     backDescription.gameObject.SetActive(true);
        //     cancelDescription.gameObject.SetActive(false);

        //     // Load the log file
        //     string json = File.ReadAllText(Globals.currentPath + "Description.json");
        //     Log descriptionLog = JsonUtility.FromJson<Log>(json);

        //     // Paste the existing information in the text fields
        //     descriptionHeading.text = descriptionLog.heading;
        //     descriptionText.text = descriptionLog.description;
        // }
        // Debug.Log("Current path: " + Globals.currentPath);
    }

    // Method that returns the array of models (json files) in the given path
    static string[] GetModelsArray(string path) 
    {
        Debug.Log("The model array was created");
        string[] questions = Directory.GetFiles(path, "Model*", SearchOption.TopDirectoryOnly);
        return questions;
    }

    // // Method that returns you the right model preview button given the index
    // public Button GetRightModelPreviewButton(int index)
    // {
    //     switch(index)
    //     {
    //         case 0:
    //             return previewModel1;
    //         break;
    //         case 1:
    //             return previewModel2;
    //         break;
    //         case 2:
    //             return previewModel3;
    //         break;
    //         case 3:
    //             return previewModel4;
    //         break;
    //         case 4:
    //             return previewModel5;
    //         break;
    //         default:
    //             return previewModel5;
    //         break;
    //     }
    // }

    // Get the index that the button gives
    public int GetIndexFromButtonName(string buttonName)
    {
        // First get the index inside the page
        int indexOnPage = 0;

        switch(buttonName)
        {
            case "Directory1":
                indexOnPage = 0;
            break;
            case "Directory2":
                indexOnPage = 1;
            break;
            case "Directory3":
                indexOnPage = 2;
            break;
            case "Directory4":
                indexOnPage = 3;
            break;
            case "Directory5":
                indexOnPage = 4;
            break;
        }
        return indexOnPage;
    }

    // Method that actualizes the global variables (when going deeper or shallower in directory structures)
    public void ActualizeGlobals()
    {
        // First I actualize the directories array and number
        directoriesArray = GetDirectoriesArray();
        numberOfDirectories = GetNumberOfDirectories(directoriesArray);

        // // Case there are no directories
        // if(numberOfDirectories == 0)
        // {
        //     Globals.filesArray = GetFilesArray();
        //     Globals.numberOfFiles = GetNumberOfFiles(Globals.filesArray);
        //     if(Globals.numberOfFiles != 0)
        //     {
        //         // Then I actualize the number of pages and rename the heading accordingly
        //         Globals.currentPage = 1;
        //         double value = (double)Globals.numberOfFiles/(double)5;
        //         Globals.numberOfPages = System.Convert.ToInt32(System.Math.Ceiling(value));
        //         GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + Globals.currentPage + "/" + Globals.numberOfPages;

        //     } else {
        //         // Case neither files not directories
        //         Globals.currentPage = 1;
        //         Globals.numberOfPages = 1;
        //         GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + Globals.currentPage + "/" + Globals.numberOfPages;
        //     }

        // // Case directories
        // } else {


        // Actualize the page heading
        currentPage = 1;
        double value = (double)numberOfDirectories/(double)5;
        numberOfPages = System.Convert.ToInt32(System.Math.Ceiling(value));
        currentPageText.text = "Page " + currentPage + "/" + numberOfPages;


        // }
    }

    // Method for the back button 
    // It should change the menu to the previous menu (main menu or creator)
    // The distinction is done with the fact that the "select" button is enabled or not
    public void Back()
    {
        // Then display the main menu
        mainMenu.SetActive(true);

        // First reset the globals so that everything is reset the next time the user enters the menu
        resetBrowsDirectories();

        // Disable the menu
        browsDirectoriesMenu.SetActive(false);

        // Disable the select level button after exiting
        selectLevel.gameObject.SetActive(false);
    }

    // Method that resets the brows directories menu
    public void resetBrowsDirectories()
    {
        currentPath = rootDirectoryPath;
        depth = 1;

        // Then I actualize in a function the directories, page numbers, heading
        ActualizeGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(currentPath);
    }
}
