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

public class AddLevel : MonoBehaviour
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
    private bool flagVariable = true;

    // Here menus and buttons are defined
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject addLevelMenu;

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

    // Define the create directory button
    [SerializeField]
    private Button createDirectory;

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
        string rootDirectoryPath = Path.GetFullPath(Path.Combine(rootPath, @"Backend\"));
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

        // Initialize the disable directory variable
        bool enableDirectory = true;

        // Go through the directories of the first page
        for(int currentIndex = initialIndex; currentIndex <= lastIndex; currentIndex = currentIndex + 1)
        {
            // Get the directory path
            string dir = directoriesArray[currentIndex];

            // Check if there are files in the directory
            if (Directory.GetFiles(dir).Length > 0)
            {
                // Set enable directory to false
                enableDirectory = false;
                
            } else {

                // Set enable directory to true
                enableDirectory = true;
            }

            // Get the name
            string lastFolderName = Path.GetFileName(dir);

            // Print the directory name on the right button
            switch (currentDirectoryNumber)
            {
                case 1:
                    directory1.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                    directory1.interactable = enableDirectory;
                break;
                case 2:
                    directory2.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                    directory2.interactable = enableDirectory;
                break;
                case 3:
                    directory3.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                    directory3.interactable = enableDirectory;
                break;
                case 4:
                    directory4.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                    directory4.interactable = enableDirectory;
                break;
                case 5:
                    directory5.GetComponentInChildren<TMP_Text>().text = lastFolderName;
                    directory5.interactable = enableDirectory;
                break;
            }
            currentDirectoryNumber = currentDirectoryNumber + 1;
        }

        // If there are no more directory, make sure the rest of the buttons are empty and not interactable
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
            // }
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
            StartCoroutine(FreezeCoroutine());
            RenameButtons(currentPath);
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
        numberOfPages = System.Convert.ToInt32(System.Math.Ceiling(value));
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
        currentPath = rootDirectoryPath;
        depth = 1;

        // Then I actualize in a function the directories, page numbers, heading
        ActualizeGlobals();

        // Then I disable / enable the previous and next button based on the number of pages
        DisableOrEnableButtons();

        // Then I rename / delete the name of the predefined buttons and disable those that have no name
        RenameButtons(currentPath);
    }

    //-----------------------------------------------------------------------------------------------------------
    // Creating directories
    //-----------------------------------------------------------------------------------------------------------

    // Define the input field, the error text and window so that they can get disabled / enabled when needed
    public TMP_InputField mainInputField; // The input field to create directories
    public TextMeshProUGUI errorText;
    public GameObject createDirectoryWindow;

    // Method used to create a directory
    public void AddDirectory(TMP_InputField input)
    {
        if (input.text.Length > 0) 
		{
            // Access to the text that was entered
            string directoryName = mainInputField.text;

            // Create new path that will exist after the directory has been created
            string newPath = currentPath + directoryName + @"\";

            // Create the new directory if it does not already exist
            if (!Directory.Exists(newPath))
            {
                // Create directory
                Directory.CreateDirectory(newPath);

                // Disable the window and enable the menu
                createDirectoryWindow.SetActive(false);

                // Save the page you were on
                int oldPageNumber = currentPage;

                // Since a new directory was created, it is needed to actualize it
                ActualizeGlobals();
                currentPage = oldPageNumber;
                DisableOrEnableButtons();
                RenameButtons(currentPath);

            } else {

                // Display error
                errorText.gameObject.SetActive(true);

            }
            // Reset the text after you used it
            mainInputField.text = "";
		}
    }
}
