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
using System.Linq;

public class BrowseLevels : MonoBehaviour
{
    // The path to the root directory where all levels are saved locally
    private string rootDirectoryPathBrowse;

    // The current path
    private string currentPathBrowse;

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
    private bool flagVariable = true;

    private bool androidBoot = true;

    private Button directoryToBeDeleted = null;

    // Here menus and buttons are defined
    public GameObject mainMenu;
    public GameObject browseDirectoriesMenu;
    public GameObject levelDescriptionMenu;
    public GameObject backGround;

    // Define the page x / y text of the brows directories menu
    public TextMeshProUGUI currentPageText;

    // Define the model preview buttons
    public Button directory1;
    public Button directory2;
    public Button directory3;
    public Button directory4;
    public Button directory5;

    // Define the previous and next buttons
    public Button previousPage;
    public Button nextPage;

    // Define the return button
    public Button returnOneUp;

    // Define the launch level button
    public Button selectLevel;

    public Button[] deleteButtons = new Button[5];

    [SerializeField] GameObject deletionWindow;

    // The sprites of the return one level up button
    [SerializeField]
    private Sprite[] switchSprites;
    private Image switchImage;

    // The two text fields of the level description menu
    public TMP_Text levelHeading;
    public TMP_Text levelDescription;

    // The log, which will become the Description.json file
    [Serializable]
    public class Log
    {
        public int numberOfQuestions; // The number of already existing questions in the folder so that the new ones can be renamed
        public int numberOfModels; // The number of already existing model files in the folder so that the new ones can be renamed
        public string heading; // Heading of the description, name that users can give
        public string description; // The description text of the content / concepts that are needed for solving the exercises
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check if it is an android boot
        if (androidBoot == true)
        {
            rootDirectoryPathBrowse = Application.persistentDataPath;

        }
        else
        {

            // First I initialize the Global paths
            string scriptPath = GetCurrentFilePath();
            rootDirectoryPathBrowse = GetPathToRootDirectory(scriptPath);
        }

        // currentPathBrowse = rootDirectoryPathBrowse;
        // depth = 1;

        // // Then I update in a function the directories, page numbers, heading
        // UpdateGlobals();

        // // Then I disable / enable the previous and next button based on the number of pages
        // DisableOrEnableButtons();

        // // Then I rename / delete the name of the predefined buttons and disable those that have no name
        // RenameButtons(currentPathBrowse);
    }

    public void EnterBrowseDirectory()
    {
        Debug.Log("The rootDirectoryPathBrowse is: " + rootDirectoryPathBrowse);
        // Set the current path to the root directory path
        currentPathBrowse = rootDirectoryPathBrowse;
        depth = 1;

        // Then I update in a function the directories, page numbers, heading
        UpdateGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(currentPathBrowse);
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
        string rootDirectoryPath = Path.GetFullPath(Path.Combine(rootPath, @"Backend"));
        return rootDirectoryPath;
    }

    // Method that returns the array of directories in the current directory
    public string[] GetDirectoriesArray()
    {
        string[] dirs = Directory.GetDirectories(currentPathBrowse, "*", SearchOption.TopDirectoryOnly)
            .Where(dir => !dir.EndsWith("il2cpp")).ToArray();
        return dirs;
    }

    // Method that returns the array of files in the given path to a directory
    public string[] GetFilesArray()
    {
        string[] files = Directory.GetFiles(currentPathBrowse, "Question*");

        // Check if the description file exists
        if (File.Exists(Path.Combine(currentPathBrowse, "Description.json")))
        {

            // Case it exists
            string[] description = Directory.GetFiles(currentPathBrowse, "Description.json");

            // Get the length of the files array
            int length = 0;
            foreach (string file in files)
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
            foreach (string file in files)
            {
                array[index] = file;
                index = index + 1;
            }

            // Return the array that contains the description and the questions
            return array;

        }
        else
        {

            // Case the description file does not exist
            return files;
        }
    }

    // Method that returns the number of directories in the current directory
    public int GetNumberOfDirectories(string[] dirs)
    {
        int number = 0;
        foreach (string dir in dirs)
        {
            number = number + 1;
        }
        return number;
    }

    // Method that returns the number of directories in the current directory
    public int GetNumberOfFiles(string[] files)
    {
        int number = 0;
        foreach (string file in files)
        {
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
        disabledTextGradient.topRight = new Color32(99, 101, 102, 255);

        // Define the enabled color gradient
        VertexGradient enabledTextGradient;
        enabledTextGradient.bottomLeft = new Color32(0, 84, 159, 255);
        enabledTextGradient.bottomRight = new Color32(0, 84, 159, 255);
        enabledTextGradient.topLeft = new Color32(64, 127, 183, 255);
        enabledTextGradient.topRight = new Color32(64, 127, 183, 255);

        // Enable / Disable previous button and change color
        TMP_Text textPrevious = previousPage.GetComponentInChildren<TMP_Text>();
        if (currentPage == 1)
        {
            previousPage.interactable = false;
            textPrevious.GetComponent<TMP_Text>().colorGradient = disabledTextGradient;
        }
        else
        {
            previousPage.interactable = true;
            textPrevious.GetComponent<TMP_Text>().colorGradient = enabledTextGradient;
        }

        // Enable / Disable next button and change color
        TMP_Text textNext = nextPage.GetComponentInChildren<TMP_Text>();
        if (currentPage != numberOfPages)
        {
            nextPage.interactable = true;
            textNext.GetComponent<TMP_Text>().colorGradient = enabledTextGradient;
        }
        else
        {
            nextPage.interactable = false;
            textNext.GetComponent<TMP_Text>().colorGradient = disabledTextGradient;
        }

        Debug.Log("The current path is: " + currentPathBrowse);
        Debug.Log("The root directory path is: " + rootDirectoryPathBrowse);

        // Enable / disable the return button
        switchImage = returnOneUp.image;
        if (currentPathBrowse != rootDirectoryPathBrowse)
        {
            //returnButtonOn.interactable = true;
            returnOneUp.interactable = true;
            switchImage.sprite = switchSprites[1];

        }
        else
        {

            //returnButtonOff.interactable = false;
            returnOneUp.interactable = false;
            switchImage.sprite = switchSprites[0];
        }
    }

    // Method that creates the buttons depending of the directory we are currently in
    public void RenameButtons(string path)
    {

        // Case there are no directories to be displayed
        if (numberOfDirectories == 0 && File.Exists(Path.Combine(currentPathBrowse, "Description.json")))
        {
            // If there are no directories, then display the level information with name, description, etc. as well as a launch level button
            // Enable the level description menu
            levelDescriptionMenu.SetActive(true);

            // Disable the brows directories menu
            browseDirectoriesMenu.SetActive(false);

            // Set the level description and heading correctly
            SetUpLevelDescription();

            // Set all delete buttons to disabled
            foreach (Button deleteButton in deleteButtons)
            {
                deleteButton.gameObject.SetActive(false);
            }

            // Case there is at least one directory, then display the numbers 5*x + 1 to 5*x + 5 (x is number of the page)
        }
        else
        {

            // First rename the buttons that should have button names, check that they are enabled
            // for that initialize the range of the for loop

            // Value at the begining of the for loop
            int initialIndex = (currentPage - 1) * 5;

            // counter for the assigning of a button
            int currentDirectoryNumber = 1;

            // Last index that would correspond to the fifth directory if the array was full enough (for the deleting names loop)
            int lastEmptyIndex = (currentPage) * 5 - 1;

            // Check that the number of directories is unequal to 0
            if (numberOfDirectories != 0)
            {
                // Value for the end of the for loop (for the renaming loop)
                int lastIndex = 0;
                if (numberOfDirectories <= (currentPage) * 5)
                {
                    lastIndex = numberOfDirectories - 1;
                }
                else
                {
                    lastIndex = currentPage * 5 - 1;
                }

                for (int currentIndex = initialIndex; currentIndex <= lastIndex; currentIndex++)
                {
                    // Get the directory path
                    string dir = directoriesArray[currentIndex];

                    // Get the name
                    string lastFolderName = Path.GetFileName(dir);

                    //Activate the delete button
                    deleteButtons[currentDirectoryNumber - 1].gameObject.SetActive(true);
                    // Print the directory name on the right button
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

                    // Increase the current directory number by one
                    currentDirectoryNumber++;
                }
            }

            // If there are no more directories, make sure the rest of the buttons are empty and not interactable
            if (currentDirectoryNumber <= 5)
            {
                for (int counter = numberOfDirectories; counter <= lastEmptyIndex; counter++)
                {
                    //Deactivate the delete button
                    deleteButtons[currentDirectoryNumber - 1].gameObject.SetActive(false);
                    switch (currentDirectoryNumber)
                    {
                        case 1:
                            directory1.GetComponentInChildren<TMP_Text>().text = "";
                            directory1.interactable = false;
                            break;

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
    public void NextPage()
    {
        currentPage++;
        DisableOrEnableButtons();
        RenameButtons(currentPathBrowse);
        GameObject.Find("HeadingTextBrowseDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing previous (change the other directories)
    public void PreviousPage()
    {
        currentPage--;
        DisableOrEnableButtons();
        RenameButtons(currentPathBrowse);
        GameObject.Find("HeadingTextBrowseDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing the return arrow (get to the parent directory)
    public void ReturnOneUp()
    {
        // // First we need to update the current path
        // currentPathBrowse = Path.GetFullPath(Path.Combine(currentPathBrowse, @"..\"));

        System.IO.DirectoryInfo parentDirectory = Directory.GetParent(currentPathBrowse);

        currentPathBrowse = parentDirectory.FullName;

        // Then we can update everything
        UpdateGlobals();
        DisableOrEnableButtons();
        RenameButtons(currentPathBrowse);
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
        Debug.Log("the current path is: " + currentPathBrowse);
        // get the name of the button that was pressed and the button
        string name = EventSystem.current.currentSelectedGameObject.name;
        Button button = GameObject.Find(name).GetComponent<Button>();
        string fileName = button.GetComponentInChildren<TMP_Text>().text;

        if (flagVariable == true)
        {
            // Increase the browsing depth
            depth++;

            // Get the name of the directory selected
            string directory = button.GetComponentInChildren<TMP_Text>().text;

            // Update the path
            currentPathBrowse = Path.Combine(currentPathBrowse, directory);

            // Update the other globals (directories array, number, page number, etc)
            UpdateGlobals();
            DisableOrEnableButtons();
            StartCoroutine(FreezeCoroutine());
            RenameButtons(currentPathBrowse);
        }
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

        switch (buttonName)
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

    // Method that updates the global variables (when going deeper or shallower in directory structures)
    public void UpdateGlobals()
    {
        // First I update the directories array and number
        directoriesArray = GetDirectoriesArray();
        numberOfDirectories = GetNumberOfDirectories(directoriesArray);
        //DisableDirectoryButton(numberOfDirectories);

        // Update the page heading
        currentPage = 1;
        double value = (double)numberOfDirectories / (double)5;
        numberOfPages = System.Convert.ToInt32(System.Math.Ceiling(value));

        // Check if the number of pages is 0
        if (numberOfPages == 0)
        {
            // If yes, set it to 1
            numberOfPages = 1;
        }

        // Change the current page text
        currentPageText.text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method for the back button 
    // It should change the menu to the previous menu (main menu or creator)
    // The distinction is done with the fact that the "select" button is enabled or not
    public void Back()
    {
        // Then display the main menu
        mainMenu.SetActive(true);

        // First reset the globals so that everything is reset the next time the user enters the menu
        ResetBrowseDirectories();

        // Disable the menu
        browseDirectoriesMenu.SetActive(false);
    }

    // Method that resets the brows directories menu
    public void ResetBrowseDirectories()
    {
        currentPathBrowse = rootDirectoryPathBrowse;
        depth = 1;

        // Then I update in a function the directories, page numbers, heading
        UpdateGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(currentPathBrowse);
    }

    // Method used to exit the level description and return to the brows directories
    public void LeaveLevelDescription()
    {
        // Set the current path to one layer up
        string currentPathBrowseLong = Path.GetFullPath(Path.Combine(currentPathBrowse, @"..\"));
        currentPathBrowse = currentPathBrowseLong.Remove(currentPathBrowseLong.Length - 1, 1);

        Debug.Log("The current path browse was changed to: " + currentPathBrowse);

        // Then we can update everything
        UpdateGlobals();
        DisableOrEnableButtons();
        RenameButtons(currentPathBrowse);

        // Enable the brows directories menu
        browseDirectoriesMenu.SetActive(true);

        // Disable the level description
        levelDescriptionMenu.SetActive(false);
    }

    [SerializeField]
    private Button feedbackButton;

    // Method that does the level description setup
    public void SetUpLevelDescription()
    {
        Debug.Log("Accessing the file: " + Path.Combine(currentPathBrowse, "Description.json"));
        // First access the description file
        string json = File.ReadAllText(Path.Combine(currentPathBrowse, "Description.json"));
        Log descriptionObject = JsonUtility.FromJson<Log>(json);

        // feedbackButton.GetComponentInChildren<TMP_Text>().text = json;

        // Check if the level heading is blank
        if (descriptionObject.heading == "")
        {
            // Give the level the name of the folder that it contains
            levelHeading.text = Path.GetFileName(Path.GetDirectoryName(currentPathBrowse));

            // NOT WORKING TODO!!!!

        }
        else
        {

            // Set the title to the heading of the description object
            levelHeading.text = descriptionObject.heading;
        }

        // Check if the level description is blank
        if (descriptionObject.description == "")
        {
            // State that the level creator did not give a description
            levelDescription.text = "The creator of theses questions did not give a description for this level.";

        }
        else
        {

            // Set the description to the description of the description object
            levelDescription.text = descriptionObject.description;
        }
    }

    // The start level menu
    [SerializeField]
    public GameObject startLevelMenu;

    // The start level button
    [SerializeField]
    public Button startLevelButton;

    // The initializing button
    [SerializeField]
    public Button initializingButton;

    // The method used to launch a level from the start level screen
    public void LaunchLevel()
    {
        // Enable the start level overlay
        startLevelMenu.SetActive(true);

        // Enable the start level button
        startLevelButton.gameObject.SetActive(true);

        // Make sure the initializing button is disabled
        initializingButton.gameObject.SetActive(false);

        // Set the path to the root directory of the level correctly
        Questions.pathToLevel = currentPathBrowse;

        Debug.Log("The path to the current level is set to: " + currentPathBrowse);

        // Disable the view level info menu
        levelDescriptionMenu.SetActive(false);

        // // At the end disable the background of all menus
        // backGround.SetActive(false);

        // Set the flag that a new level started so that all level information are reset
        LevelInfo.newLevelStarted = true;
    }

    public void DeleteLevel(int directorynumber)
    {
        switch (directorynumber)
        {
            case 1:
                directoryToBeDeleted = directory1;
                break;
            case 2:
                directoryToBeDeleted = directory2;
                break;
            case 3:
                directoryToBeDeleted = directory3;
                break;
            case 4:
                directoryToBeDeleted = directory4;
                break;
            case 5:
                directoryToBeDeleted = directory5;
                break;
            default:
                directoryToBeDeleted = null;
                break;
        }

        if (directoryToBeDeleted != null)
        {
            deletionWindow.SetActive(true);
            deletionWindow.GetComponentInChildren<TMP_Text>().SetText("Are you sure you want to delete the quiz: " + directoryToBeDeleted.GetComponentInChildren<TMP_Text>().text + "?");
        }
    }

    public void ConfirmDeletion()
    {
        string quizname = directoryToBeDeleted.GetComponentInChildren<TMP_Text>().text;
        Directory.Delete(Application.persistentDataPath + "/" + quizname, true);

        UpdateGlobals();
        DisableOrEnableButtons();
        RenameButtons(currentPathBrowse);
        deletionWindow.SetActive(false);
    }

    public void CancelDeletion()
    {
        directoryToBeDeleted = null;
        deletionWindow.SetActive(false);
    }

    private void DisableDirectoryButton(int numberOfDirectory)
    {
        switch (numberOfDirectories)
        {
            case 1:
                directory2.gameObject.SetActive(false);
                directory3.gameObject.SetActive(false);
                directory4.gameObject.SetActive(false);
                directory5.gameObject.SetActive(false);
                break;
            case 2:
                directory3.gameObject.SetActive(false);
                directory4.gameObject.SetActive(false);
                directory5.gameObject.SetActive(false);
                break;
            case 3:
                directory4.gameObject.SetActive(false);
                directory5.gameObject.SetActive(false);
                break;
            case 4:
                directory5.gameObject.SetActive(false);
                break;
        }
    }
}
