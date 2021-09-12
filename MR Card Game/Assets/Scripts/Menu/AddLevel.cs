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

static class Globals
{
    // Save the current path
    public static string currentPath;

    public static bool resetAddLevelMenu;

    // The flag that states if this is an android boot or not
    public static bool androidBoot;

    public static string rootDirectoryPath;
}

public class AddLevel : MonoBehaviour
{
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

    // Here menus and buttons are defined
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject addLevelMenu;
    [SerializeField]
    private GameObject levelDescriptionMenu;

    // Define the page x / y text of the brows directories menu
    [SerializeField]
    private TextMeshProUGUI currentPageText;

    // Define the model preview buttons
    [SerializeField]
    private Button directory1;
    [SerializeField]
    private Button directory2;
    [SerializeField]
    private Button directory3;
    [SerializeField]
    private Button directory4;
    [SerializeField]
    private Button directory5;

    // Define the create and select directory button
    [SerializeField]
    private Button createDirectoryButton;
    [SerializeField]
    private Button selectDirectory;

    // Define the previous an next buttons
    [SerializeField]
    private Button previousPage;
    [SerializeField]
    private Button nextPage;

    // Define the return button
    [SerializeField]
    public Button returnOneUp;

    // The sprites of the return one level up button
    [SerializeField]
    private Sprite[] spriteSwitch;
    private Image imageSwitch;

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
        Globals.resetAddLevelMenu = false;

        // Set androidBoot to true or false
        Globals.androidBoot = true;

        // Check if it is an android boot
        if(Globals.androidBoot == true)
        {
            Globals.rootDirectoryPath = Application.persistentDataPath;
            
        } else {
            // First I initialize the Global paths
            string scriptPath = GetCurrentFilePath();
            Globals.rootDirectoryPath = GetPathToRootDirectory(scriptPath);
        }

        // First I initialize the Global paths
        Globals.currentPath = Globals.rootDirectoryPath;
        depth = 1;

        // Then I actualize in a function the directories, page numbers, heading
        ActualizeGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(Globals.currentPath);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the menu should be reset
        if(Globals.resetAddLevelMenu == true)
        {
            // Set the reset flag to false
            Globals.resetAddLevelMenu = false;

            // Reset the menu
            Back();
        }
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
        string[] dirs = Directory.GetDirectories(Globals.currentPath, "*", SearchOption.TopDirectoryOnly);
        return dirs;
    }

    // Method that returns the array of files in the given path to a directory
    public string[] GetFilesArray()
    {
        string[] files = Directory.GetFiles(Globals.currentPath, "Question*");

        // Check if the description file exists
        if (File.Exists(Globals.currentPath + "Description.json")) 
        {

            // Case it exists
            string[] description = Directory.GetFiles(Globals.currentPath, "Description.json");

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

    [SerializeField]
    private Button deleteDirectoryButton;

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

        Debug.Log("The current path is: " + Globals.currentPath);
        Debug.Log("The root directory path is: " + Globals.rootDirectoryPath);

        // Enable / disable the return button
        imageSwitch = returnOneUp.image;
        if(Globals.currentPath != Globals.rootDirectoryPath)
        {
            //returnButtonOn.interactable = true;
            returnOneUp.interactable = true;
            imageSwitch.sprite = spriteSwitch[1];
        } else {
            //returnButtonOff.interactable = false;
            returnOneUp.interactable = false;
            imageSwitch.sprite = spriteSwitch[0];
        }

        // Enable / Disable select directory button, the select directory button is not enabled in the root directory and in directories containing directories
        TMP_Text textSelectDirectory = selectDirectory.GetComponentInChildren<TMP_Text>();
        if(Globals.currentPath != Globals.rootDirectoryPath && numberOfDirectories == 0 && GetNumberOfFiles(GetFilesArray()) == 0)
        {
            // Make the select directory button interactable and make it blue
            selectDirectory.interactable = true;
            textSelectDirectory.GetComponent<TMP_Text>().colorGradient = enabledTextGradient;

            // Enable the delete directory button
            deleteDirectoryButton.gameObject.SetActive(true);

        } else {

            // Make sure the select directory button is not interactable and make it grey
            selectDirectory.interactable = false;
            textSelectDirectory.GetComponent<TMP_Text>().colorGradient = disabledTextGradient;

            // Make sure the delete directory button is disabled
            deleteDirectoryButton.gameObject.SetActive(false);
        }
    }

    // Method that creates the buttons depending of the directory we are currently in
    public void RenameButtons(string path)
    {
        // Check if there are no new directories and a description file (== level inside)
        if(numberOfDirectories == 0 && File.Exists(Path.Combine(Globals.currentPath, "Description.json")))
        {
            // Enable the level description menu
            levelDescriptionMenu.SetActive(true);

            // Set the level description and heading correctly
            SetUpLevelDescription();

            // Disable the add level menu
            addLevelMenu.SetActive(false);

        // Case there is at least one directory, then display the numbers 5*x + 1 to 5*x + 5 (x is number of the page)
        } else {

            // First rename the buttons that should have button names, check that they are enabled
            // for that initialize the range of the for loop

            // Value at the begining of the for loop
            int initialIndex = (currentPage - 1) * 5;

            // counter for the assigning of a button
            int currentDirectoryNumber = 1;

            // Last index that would correspond to the fifth directory if the array was full enough (for the deleting names loop)
            int lastEmptyIndex = (currentPage) * 5 - 1;

            // Check that the number of directories is unequal to 0
            if(numberOfDirectories != 0)
            {
                // Value for the end of the for loop (for the renaming loop)
                int lastIndex = 0;
                if(numberOfDirectories <= (currentPage) * 5)
                {
                    lastIndex = numberOfDirectories - 1;
                } else {
                    lastIndex = currentPage * 5 - 1;
                }

                for(int currentIndex = initialIndex; currentIndex <= lastIndex; currentIndex = currentIndex + 1)
                {
                    // Get the directory path
                    string dir = directoriesArray[currentIndex];

                    // Get the name
                    string lastFolderName = Path.GetFileName(dir);

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
                    currentDirectoryNumber = currentDirectoryNumber + 1;
                }
            }

            // If there are no more directory, make sure the rest of the buttons are empty and not interactable
            if(currentDirectoryNumber != 5)
            {

                // // Check if there are no directories
                // if(currentDirectoryNumber == 1)
                // {
                //     // Enable the delete directory button
                //     deleteDirectoryButton.gameObject.SetActive(true);
                // }
                
                for(int counter = numberOfDirectories; counter <= lastEmptyIndex; counter = counter + 1)
                {
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
    public void NextPage(){
        currentPage = currentPage + 1;
        DisableOrEnableButtons();
        RenameButtons(Globals.currentPath);
        GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing previous (change the other directories)
    public void PreviousPage(){
        currentPage = currentPage - 1;
        DisableOrEnableButtons();
        RenameButtons(Globals.currentPath);
        GameObject.Find("HeadingTextBrowsDirectories").GetComponent<TMP_Text>().text = "Page " + currentPage + "/" + numberOfPages;
    }

    // Method that is activated when pressing the return arrow (get to the parent directory)
    public void ReturnOneUp()
    {
        // First we need to actualize the current path
        // Globals.currentPath = Path.GetFullPath(Path.Combine(Globals.currentPath, @"..\"));

        System.IO.DirectoryInfo parentDirectory = Directory.GetParent(Globals.currentPath);

        Globals.currentPath = parentDirectory.FullName;
        
        // Then we can actualize everything
        ActualizeGlobals();
        DisableOrEnableButtons();
        RenameButtons(Globals.currentPath);
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
            Globals.currentPath = Path.Combine(Globals.currentPath, directory);

            // Actualize the other globals (directories array, number, page number, etc)
            ActualizeGlobals();
            DisableOrEnableButtons();
            StartCoroutine(FreezeCoroutine());
            RenameButtons(Globals.currentPath);
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

        // Actualize the page heading
        currentPage = 1;
        double value = (double)numberOfDirectories/(double)5;

        // Get the number of pages
        int pageNumber = System.Convert.ToInt32(System.Math.Ceiling(value));

        // Check if the number of pages is 0
        if(pageNumber == 0)
        {
            // If yes, set it to 1
            numberOfPages = 1;

        } else {

            // If no, set the number of pages correctly
            numberOfPages = pageNumber;
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
        resetAddLevelMenu();

        // Disable the menu
        addLevelMenu.SetActive(false);
    }

    // Method that resets the brows directories menu
    public void resetAddLevelMenu()
    {
        Globals.currentPath = Globals.rootDirectoryPath;
        depth = 1;

        // Then I actualize in a function the directories, page numbers, heading
        ActualizeGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(Globals.currentPath);
    }

    //-----------------------------------------------------------------------------------------------------------
    // Displaying the level description window of the add level menu
    //-----------------------------------------------------------------------------------------------------------

    // Method that does the level description setup
    public void SetUpLevelDescription()
    {
        // First access the description file
        string json = File.ReadAllText(Path.Combine(Globals.currentPath, "Description.json"));
        Log descriptionObject = JsonUtility.FromJson<Log>(json);

        // Check if the level heading is blank
        if(descriptionObject.heading == "")
        {
            // Give the level the name of the folder that it contains
            levelHeading.text = Path.GetFileName(Path.GetDirectoryName(Globals.currentPath));

            // NOT WORKING TODO!!!!

        } else {

            // Set the title to the heading of the description object
            levelHeading.text = descriptionObject.heading;
        }

        // Check if the level description is blank
        if(descriptionObject.description == "")
        {
            // State that the level creator did not give a description
            levelDescription.text = "The creator of theses questions did not give a description for this level.";

        } else {

            // Set the description to the description of the description object
            levelDescription.text = descriptionObject.description;
        }
    }

    // Method used to exit the level description and return to the brows directories
    public void LeaveLevelDescription()
    {
        // Enable the brows directories menu
        addLevelMenu.SetActive(true);

        // Disable the level description
        levelDescriptionMenu.SetActive(false);

        // Set the current path to one layer up
        string currentPath= Path.GetFullPath(Path.Combine(Globals.currentPath, @"..\"));
        Globals.currentPath = currentPath.Remove(currentPath.Length - 1, 1); 

        // Then we can actualize everything
        ActualizeGlobals();
        DisableOrEnableButtons();
        RenameButtons(Globals.currentPath);
    }

    // Method used to delete a level by pressing the delete button in the level description window
    public void DeleteLevel()
    {
        // Get the array of all files in the current path directory
        string[] filePaths = Directory.GetFiles(Globals.currentPath);

        // Go through all files in the directory
        foreach (string filePath in filePaths)
        {
            // Delete file
            File.Delete(filePath); 
        }
        
        // Leave the level description
        LeaveLevelDescription();
    }

    //-----------------------------------------------------------------------------------------------------------
    // Creating directories
    //-----------------------------------------------------------------------------------------------------------

    // Define the input field, the error text and window so that they can get disabled / enabled when needed
    [SerializeField]
    private TMP_InputField mainInputField; // The input field to create directories

    [SerializeField]
    private TextMeshProUGUI errorText;

    [SerializeField]
    private GameObject createDirectoryWindow;

    // Method used to open the create directory menu
    public void OpenCreateDirectory()
    {
        Debug.Log("The create directory button was clicked!");

        // Disable the add level menu
        addLevelMenu.SetActive(false);

        Debug.Log("The status of the add level menu is: " + addLevelMenu.activeSelf);

        // Enable the create directory menu
        createDirectoryWindow.SetActive(true);

        Debug.Log("The status of the create directory menu is: " + createDirectoryWindow.activeSelf);
    }

    // Method used to create a directory
    public void AddDirectory(TMP_InputField input)
    {
        if (input.text.Length > 0) 
		{
            // Access to the text that was entered
            string directoryName = mainInputField.text;

            // Create new path that will exist after the directory has been created
            string newPath = Globals.currentPath + "/" + directoryName + "/";

            // Create the new directory if it does not already exist
            if (!Directory.Exists(newPath))
            {
                // Create directory
                Directory.CreateDirectory(newPath);

                // Disable the window
                createDirectoryWindow.SetActive(false);

                // Enable the menu
                addLevelMenu.SetActive(true);

                // Save the page you were on
                int oldPageNumber = currentPage;

                // Since a new directory was created, it is needed to actualize it
                ActualizeGlobals();
                currentPage = oldPageNumber;
                DisableOrEnableButtons();
                RenameButtons(Globals.currentPath);

            } else {

                // Display error
                errorText.gameObject.SetActive(true);

            }

            // Reset the text after you used it
            mainInputField.text = "";
		}
    }

    // Method used to delete a directory
    public void DeleteDirectory()
    {
        Debug.Log("Trying to delete the directory with path: " + Globals.currentPath);
        // Check if the directory exist
        if(Directory.Exists(Globals.currentPath))
        {
            Debug.Log("Deleting directory with path: " + Globals.currentPath);
            // Delete directory
            Directory.Delete(Globals.currentPath);

            // Return one up
            ReturnOneUp();
        }
    }

    // Method used to exit the level description and return to the brows directories
    public void LeaveCreateDirectory()
    {
        // Enable the brows directories menu
        addLevelMenu.SetActive(true);

        // Disable the level description
        createDirectoryWindow.SetActive(false);

        // // Set the current path to one layer up
        // Globals.currentPath = Path.GetFullPath(Path.Combine(Globals.currentPath, @"..\"));

        // Then we can actualize everything
        ActualizeGlobals();
        DisableOrEnableButtons();
        RenameButtons(Globals.currentPath);
    }

    //-----------------------------------------------------------------------------------------------------------
    // Adding level
    //-----------------------------------------------------------------------------------------------------------

    // Define the input field, the error text and menu of the enter code to add level menu
    [SerializeField]
    private TMP_InputField inputFieldEnterCode; // The input field to add a level

    [SerializeField]
    private TextMeshProUGUI errorTextCodeNotValid; // The error message text object

    [SerializeField]
    private GameObject enterCodeMenu; // The menu object

    // The method used to leave the enter code menu when pressing the cancel button
    public void OpenEnterCodeMenu()
    {
        // Enable the enter code menu
        enterCodeMenu.SetActive(true);

        // Disable the add level menu
        addLevelMenu.SetActive(false);
    }

    // The method used to leave the enter code menu when pressing the cancel button
    public void LeaveEnterCodeMenu()
    {
        // Reset the text field
        inputFieldEnterCode.text = "";

        // Disable the error message
        errorTextCodeNotValid.gameObject.SetActive(false);

        // Disable the enter code menu
        enterCodeMenu.SetActive(false);

        // Enable the add level menu
        addLevelMenu.SetActive(true);
    }
}
