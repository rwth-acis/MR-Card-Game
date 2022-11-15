using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using UnityEngine.Networking;


static class Questions
{
    // The shuffled question array
    public static string[] questionArray;

    // The index of the current question in the question array
    public static int currentQuestionIndex;

    // The last index of the array
    public static int lastQuestionIndex;

    // The number of models
    public static int numberOfModels;

    // The number of models that have finished to load
    public static int numberOfModelsLoaded;

    // The path to the local folder on the device
    public static string pathToLevel;

    // The number of questions that need to be answered correctly before the action can take place
    public static int numberOfQuestionsNeededToAnswer;

    // The image target that started the questions
    public static GameObject questionRequestingImageTarget;

    public static bool currentlyAnsweringQuestion;
}

public class ActivateQuestions : MonoBehaviour
{
    // The instance of this script
    public static ActivateQuestions instance;

    // Define the "menus"
    public GameObject viewModel;
    public GameObject viewMultipleChoiceQuestion;
    public GameObject viewInputQuestion;
    public GameObject gameOverlay;
    // Define the button on which the number of questions that need to be answered is written
    public Button numberOfQuestionsThatNeedToBeAnsweredDisplay;

    [Header("Input Question")]

    // Define the text objects, text field and buttons of the input question interface
    public TMP_Text questionNameInput;
    public Button feedbackInput;

    [Tooltip("The panel contains all components for input questions")]
    public GameObject inputWithoutImagePanel;

    public TMP_Text questionTextWithoutImageInput;
    public Button confirmAnswerWithoutImageInput;
    public Button closeQuestionWithoutImageInput;
    public TMP_InputField answerFieldWithoutImageInput;

    // Define the text elements of the correct answer preview of the input question interface
    public GameObject previewCorrectAnswerInput;
    public TMP_Text yourWrongAnswerWithoutImageInput;
    public TMP_Text theCorrectAnswerWithoutImageInput;

    [Header("Multiple Choice Question")]

    // Define the text objects and button of the multiple choice question interface
    public TMP_Text questionNameMC;

    public Button feedbackMC;

    [Tooltip("The panel contains all components for multiple choice questions")]
    public GameObject multipleChoiceWithoutImagePanel;
    public Button confirmAnswerWithoutImageMC;
    public Button closeQuestionWithoutImageMC;

    // Define the text objects and buttons of multiple choice questions
    public TMP_Text questionWithoutImageTextMC;
    public Button answer1WithoutImageMC;
    public Button answer2WithoutImageMC;
    public Button answer3WithoutImageMC;
    public Button answer4WithoutImageMC;
    public Button answer5WithoutImageMC;

    // Define two questions for correct and incorrect answers
    public Color correctColor;
    public Color incorrectColor;
    public Color typingColor;
    public Color selectedColor;
    public Color deselectedColor;
    public Color correctButtonColor;
    public Color incorrectButtonColor;

    // The begin of the url to the .obj object downloaded in the created examples
    public string urlBegin;

    /// <summary>
    /// the view model (so view activate question) menu as a static object
    /// </summary>
    public static GameObject ViewModel
    {
        get { return instance.viewModel; }
    }

    /// <summary>
    /// the view multiple choice question menu as a static object
    /// </summary>
    public static GameObject ViewMultipleChoiceQuestion
    {
        get { return instance.viewMultipleChoiceQuestion; }
    }

    /// <summary>
    /// the view input question menu as a static object
    /// </summary>
    public static GameObject ViewInputQuestion
    {
        get { return instance.viewInputQuestion; }
    }

    /// <summary>
    /// the additional field 2 as a static object
    /// </summary>
    public static Button NumberOfQuestionsThatNeedToBeAnsweredDisplay
    {
        get { return instance.numberOfQuestionsThatNeedToBeAnsweredDisplay; }
    }

    /// <summary>
    /// The JSON Serialization for the input questions
    /// </summary>
    [Serializable]
    public class InputQuestion
    {
        public string exerciseType;
        public string name;
        public string question;
        public string answer;
        public string withImage;
        public string imageName;
    }

    /// <summary>
    /// The JSON Serialization for the multiple choice questions
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestion
    {
        public string exerciseType;
        public string name;
        public string question;
        public int numberOfAnswers;
        public string answer1;
        public string answer2;
        public string answer3;
        public string answer4;
        public string answer5;
        public bool answer1Correct;
        public bool answer2Correct;
        public bool answer3Correct;
        public bool answer4Correct;
        public bool answer5Correct;
        public bool withImage;
        public string imageName;
    }

    // The JSON Serialization for the Models
    [Serializable]
    public class Model
    {
        public string modelName;
        public string modelUrl;
        public int numberOfQuestionsUsedIn;
    }

    // The JSON Serialization for the log file
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
        instance = this;

        Questions.currentlyAnsweringQuestion = false;

        Questions.numberOfQuestionsNeededToAnswer = 0;

        // The number of models
        Questions.numberOfModels = 5;

        Questions.numberOfModelsLoaded = 0;

        // Since the path to level is set in browse level, it is the same in android and not android boot
        levelDirectoryPath = Questions.pathToLevel;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the number of questions that need to be answered is greater than 0
        if(Questions.numberOfQuestionsNeededToAnswer > 0 && Questions.currentlyAnsweringQuestion == false)
        {
            // Activate the models
            ActivateViewModels();

            // Set that the question is currently being answered
            Questions.currentlyAnsweringQuestion = true;
        }
    }

    /// <summary>
    /// Returns the greatest number of the three floats given
    /// </summary>
    public float ReturnGreatestFloat(float size1, float size2, float size3)
    {
        // Initialize a size variable
        float greater;

        // Check which one is greater from the two first sizes
        if (size1 >= size2)
        {
            greater = size1;
        } else {
            greater = size2;
        }

        // Check what is greater, the greatest of the two first sizes or the third size
        if(greater >= size3)
        {
            return greater;
        } else {
            return size3;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that activate the question interfaces depending on the current question
    //-------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Executed when a user presses the "view question" button in the model view
    /// </summary>
    public void ActivateQuestion()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Access the json string of the question file
        string json = File.ReadAllText(questionPath);

        // Disable the view model menu
        viewModel.SetActive(false);

        // Check if the path points at a multiple choice or input question
        if(json.Contains("input question") == true)
        {
            // Extract the object
            InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

            // The current question is an input question, open the right question UI
            viewInputQuestion.SetActive(true);

            // Display the question information in the right text objects
            questionNameInput.text = question.name;

            // TODO: With / Without picture
            inputWithoutImagePanel.SetActive(true);
            questionTextWithoutImageInput.text = question.question;

            // Activate the confirm button and deactivate the close button
            closeQuestionWithoutImageInput.gameObject.SetActive(false);
            confirmAnswerWithoutImageInput.gameObject.SetActive(true);

            // Activate the input field and make it interactable)
            answerFieldWithoutImageInput.gameObject.SetActive(true);
            answerFieldWithoutImageInput.interactable = true;

            // Disable the correct answer preview
            previewCorrectAnswerInput.SetActive(false);

            // Set the right typing color to the input field
            GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = typingColor;
            answerFieldWithoutImageInput.text = "";
            
        } else {

            MultipleChoiceQuestion question = JsonUtility.FromJson<MultipleChoiceQuestion>(json);
            viewMultipleChoiceQuestion.SetActive(true);
            multipleChoiceWithoutImagePanel.SetActive(true);

            // Activate the confirm button and deactivate the close button
            closeQuestionWithoutImageMC.gameObject.SetActive(false);
            confirmAnswerWithoutImageMC.gameObject.SetActive(true);
            DeactivateButtonsWithoutImageMC();
            // Display the correct question name
            questionNameMC.text = question.name;
         
            //TODO: With/Without Image
            questionWithoutImageTextMC.text = question.question;
            // Check how many answers exist
            switch (question.numberOfAnswers)
            {
                case 2:
                    answer1WithoutImageMC.gameObject.SetActive(true);
                    answer2WithoutImageMC.gameObject.SetActive(true);
                    // Display the correct question and ansers
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer2;

                    // Make them interactable
                    answer1WithoutImageMC.interactable = true;
                    answer2WithoutImageMC.interactable = true;

                    // Set the right button color
                    answer1WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer2WithoutImageMC.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    break;
                case 3:
                    answer1WithoutImageMC.gameObject.SetActive(true);
                    answer2WithoutImageMC.gameObject.SetActive(true);
                    answer3WithoutImageMC.gameObject.SetActive(true);
                    // Display the correct question and ansers
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer3;

                    // Make them interactable
                    answer1WithoutImageMC.interactable = true;
                    answer2WithoutImageMC.interactable = true;
                    answer3WithoutImageMC.interactable = true;

                    // Set the right button color
                    answer1WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer2WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer3WithoutImageMC.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    break;
                case 4:
                    answer1WithoutImageMC.gameObject.SetActive(true);
                    answer2WithoutImageMC.gameObject.SetActive(true);
                    answer3WithoutImageMC.gameObject.SetActive(true);
                    answer4WithoutImageMC.gameObject.SetActive(true);
                    // Display the correct question and ansers
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer3;
                    answer4WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer4;

                    // Make them interactable
                    answer1WithoutImageMC.interactable = true;
                    answer2WithoutImageMC.interactable = true;
                    answer3WithoutImageMC.interactable = true;
                    answer4WithoutImageMC.interactable = true;

                    // Set the right button color
                    answer1WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer2WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer3WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer4WithoutImageMC.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer4WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    break;
                case 5:
                    answer1WithoutImageMC.gameObject.SetActive(true);
                    answer2WithoutImageMC.gameObject.SetActive(true);
                    answer3WithoutImageMC.gameObject.SetActive(true);
                    answer4WithoutImageMC.gameObject.SetActive(true);
                    answer5WithoutImageMC.gameObject.SetActive(true);
                    // Display the correct question and ansers
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer3;
                    answer4WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer4;
                    answer5WithoutImageMC.GetComponentInChildren<TMP_Text>().text = question.answer5;

                    // Make them interactable
                    answer1WithoutImageMC.interactable = true;
                    answer2WithoutImageMC.interactable = true;
                    answer3WithoutImageMC.interactable = true;
                    answer4WithoutImageMC.interactable = true;
                    answer5WithoutImageMC.interactable = true;

                    // Set the right button color
                    answer1WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer2WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer3WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer4WithoutImageMC.GetComponent<Image>().color = deselectedColor;
                    answer5WithoutImageMC.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer1WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer2WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer3WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer4WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer5WithoutImageMC.GetComponentInChildren<TMP_Text>().color = typingColor;
                    break;
                default:
                    Debug.LogError("A MC question is only allowed to have at most 5 choices.");
                    break;
            }       
        }
    }

    private void DeactivateButtonsWithoutImageMC()
    {
        answer1WithoutImageMC.gameObject.SetActive(false);
        answer2WithoutImageMC.gameObject.SetActive(false);
        answer3WithoutImageMC.gameObject.SetActive(false);
        answer4WithoutImageMC.gameObject.SetActive(false);
        answer5WithoutImageMC.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that give a visual feedback on the correct and incorrect answers and correct or incorrect given answers
    //-------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Displays the correct or incorrect answer when clicking on the selection confirmation button after solving an input question
    /// </summary>
    public void DisplayRestultInput()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Extract the question object
        string json = File.ReadAllText(questionPath);
        InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

        // Initialize the boolean that tells if the given answer was correct or not
        bool answeredCorrectly = true;

        // Check if the answer is correct
        if(answerFieldWithoutImageInput.text == question.answer)
        {
            // Case it is the correct answer, change the color of the text of the input field to green
            GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = correctColor;

            closeQuestionWithoutImageInput.gameObject.SetActive(true);
            confirmAnswerWithoutImageInput.gameObject.SetActive(false);
            answerFieldWithoutImageInput.interactable = false;
            

        } else {

            // Check if it was a spelling mistake (capital letter missing or too much at the begining)
            bool sameExceptCapitalLetter = CheckIfBothOrNoneStartWithCapitalLetter(answerFieldWithoutImageInput.text, question.answer);

            if(sameExceptCapitalLetter == true)
            {
                // Case it is the correct answer except for the first latter that had/missed capitalization, change the color of the text of the input field to green
                GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = correctColor;

                // Change the text so that it has the capitalization of the answer
                answerFieldWithoutImageInput.text = question.answer;

                closeQuestionWithoutImageInput.gameObject.SetActive(true);
                confirmAnswerWithoutImageInput.gameObject.SetActive(false);
                answerFieldWithoutImageInput.interactable = false;

            } else {
                // Case both words are different, open the correct answer preview
                previewCorrectAnswerInput.SetActive(true);

                // Disable the text input field
                answerFieldWithoutImageInput.gameObject.SetActive(false);

                // Set the text objects on the right string
                yourWrongAnswerWithoutImageInput.text = answerFieldWithoutImageInput.text;
                theCorrectAnswerWithoutImageInput.text = question.answer;

                closeQuestionWithoutImageInput.gameObject.SetActive(true);
                confirmAnswerWithoutImageInput.gameObject.SetActive(false);
                answeredCorrectly = false;
            }
        }

        // Check if the question was answered correctly or not and give visual feedback
        DisplayFeedbackButton(feedbackInput, answeredCorrectly);

        if(answeredCorrectly == true)
        {
            lastQuestionWasAnsweredCorrectly = true;

        } else {

            lastQuestionWasAnsweredCorrectly = false;
        }
    }

    /// <summary>
    /// Checks if both strings begin with or without capital letter, if not check if it is the same letter
    /// </summary>
    public bool CheckIfBothOrNoneStartWithCapitalLetter(string firstWord, string secondWord)
    {
        // Check if the first characters are digits or letters
        if(Char.IsDigit(firstWord, 0) == false && Char.IsDigit(secondWord, 0) == false)
        {
            // Case the first characters are letters
            // Check if the first character of both words is a capital letter
            bool firstCapital = Char.IsUpper(firstWord, 0);
            bool secondCapital = Char.IsUpper(secondWord, 0);

            // Compare the boolean
            if(firstCapital == secondCapital)
            {
                // Case both are small or both are capital, both words are different
                return false;

            } else {

                // One is written with a capital, the other is written small, check if it is the same word
                if(firstCapital == false)
                {
                    // Case it is the first word that is written small, add the capital letter and check the equality
                    string capitalizedFirstWord = WriteFirstLetterCapital(firstWord);

                    // Check if the capitalized word is identical to the other
                    if(capitalizedFirstWord == secondWord)
                    {
                        return true;

                    } else {
                        return false;
                    }

                } else {

                    // Case it is the second word that is written small, add the capital letter and check the equality
                    string capitalizedSecondWord = WriteFirstLetterCapital(secondWord);

                    // Check if the capitalized word is identical to the other
                    if(capitalizedSecondWord == firstWord)
                    {
                        return true;

                    } else {
                        return false;
                    }
                }
            }

        } else {

            // Case the first character is a digit, both words are different
            return false;
        }
    }

    /// <summary>
    /// Takes a word, and returns it with a capital letter as first letter
    /// </summary>
    public string WriteFirstLetterCapital(string word)
    {
        string firstLetter = word.Substring(0, 1);
        firstLetter = firstLetter.ToUpper();

        // Get the end of the word
        string ending = word.Substring(1);

        // Return the capital letter with the word ending
        return firstLetter + ending;

    }

    /// <summary>
    /// Displays the correct or incorrect answer when clicking on the selection confirmation button after solving an multiple choice question
    /// </summary>
    public void DisplayRestultWithoutImageMC()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Extract the question object
        string json = File.ReadAllText(questionPath);
        MultipleChoiceQuestion question = JsonUtility.FromJson<MultipleChoiceQuestion>(json);

        // Initialize the boolean variable that gives the information if all correct answers were selected and no incorrect answer was selected
        bool answeredCorrectly = true;

        // Go over all possible answers, and check if they were selected or not
        for(int index = 0; index < question.numberOfAnswers; index++)
        {
            // Get the button that correspond to the index
            Button currentButton = GetQuestionButtonFromIndex(index, false);

            // Check if the answer should be correct and was selected
            if(AnswerShouldBeCorrect(index, question) == true)
            {
                // if the user did not select the right answer
                if (currentButton.GetComponent<Image>().color != selectedColor)
                {
                    answeredCorrectly = false;
                }

                // Answer was correct, give the button a greenish tint
                currentButton.GetComponent<Image>().color = correctButtonColor;
                currentButton.GetComponentInChildren<TMP_Text>().color = correctColor;

            }
            else {

                if (currentButton.GetComponent<Image>().color == selectedColor)
                {
                    // Did select incorrect answer, question answered incorrectly
                    answeredCorrectly = false;
                }

                // Answer was incorrect, give the button a reddish tint
                currentButton.GetComponent<Image>().color = incorrectButtonColor;
                currentButton.GetComponentInChildren<TMP_Text>().color = incorrectColor;
            }

            // Make the button not interactable
            currentButton.interactable = false;
        }

        // Check if the question was answered correctly or not and give visual feedback
        DisplayFeedbackButton(feedbackMC, answeredCorrectly);

        // Activate the close button and deactivate the confirm button
        closeQuestionWithoutImageMC.gameObject.SetActive(true);
        confirmAnswerWithoutImageMC.gameObject.SetActive(false);

        // If the question was answered correctly, reduce the number of questions that need to be answered by one
        if(answeredCorrectly == true)
        {
            lastQuestionWasAnsweredCorrectly = true;

        } else {

            lastQuestionWasAnsweredCorrectly = false;
        }
    }

    /// <summary>
    /// Displays the correctly or incorrectly answered question
    /// </summary>
    public void DisplayFeedbackButton(Button button, bool answer)
    {
        // Enable the button
        button.gameObject.SetActive(true);

        // Check if the question was answered correctly or not
        if(answer == true)
        {
            button.GetComponentInChildren<TMP_Text>().text = "Your answer was correct!";
            button.GetComponentInChildren<TMP_Text>().color = correctColor;

        } else {

            button.GetComponentInChildren<TMP_Text>().text = "Your answer was incorrect.";
            button.GetComponentInChildren<TMP_Text>().color = incorrectColor;
        }
    }

    /// <summary>
    /// Returns if the answer with given index should be true or not
    /// </summary>
    public bool AnswerShouldBeCorrect(int index, MultipleChoiceQuestion question)
    {
        // Return the right boolean value depending on the index
        switch(index)
        {
            case 0:
                return question.answer1Correct;
            case 1:
                return question.answer2Correct;
            case 2:
                return question.answer3Correct;
            case 3:
                return question.answer4Correct;
            case 4:
                return question.answer5Correct;
        }

        // This case will never happen
        return question.answer5Correct;
    }

    /// <summary>
    /// Returns you the right button given the index and whether it is a question with picture;
    /// </summary>
    public Button GetQuestionButtonFromIndex(int index, bool withImage)
    {
        if (withImage)
        {
            return null;
        }
        else
        {
            switch (index)
            {
                case 0:
                    return answer1WithoutImageMC;
                case 1:
                    return answer2WithoutImageMC;
                case 2:
                    return answer3WithoutImageMC;
                case 3:
                    return answer4WithoutImageMC;
                case 4:
                    return answer5WithoutImageMC;
                default:
                    Debug.LogError("A MC question is only allowed to have 2, 3, or 4 choices");
                    return null;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that create the question array, shuffling the questions in the level directory, and that changes question after the current question was answered
    //-------------------------------------------------------------------------------------------------------------------------------------------
    
    // The path to the current level directory
    public string levelDirectoryPath;

    public Button startButton;

    private IEnumerator GetJsonFiles(string additionalPath)
    {
        string path = Application.streamingAssetsPath + additionalPath;
        UnityWebRequest uwr = new UnityWebRequest(path);
        
        yield return uwr.SendWebRequest();
    
        string newPath = Application.persistentDataPath + additionalPath;
        File.WriteAllBytes(newPath, uwr.downloadHandler.data);
    }

    
    public void ReadJson(string additionalPath)
    {
        string path = Application.persistentDataPath + additionalPath;
        StreamReader json = new StreamReader(path);
        string input = json.ReadToEnd();
        Log description = JsonUtility.FromJson<Log>(input);

        // Write the number of questions on the button
        startButton.GetComponentInChildren<TMP_Text>().text = description.numberOfQuestions.ToString();
    }


    // Method that creates the question array (not shuffled)
    public void InitializeQuestionArray()
    {
        string[] questions = Directory.GetFiles(Questions.pathToLevel, "Question*", SearchOption.TopDirectoryOnly);
        Questions.questionArray = questions;
        Questions.lastQuestionIndex = Questions.questionArray.Length - 1;
        Questions.currentQuestionIndex = 0;
    }

    // Initialize random number generator
    private readonly System.Random random = new System.Random();

    /// <summary>
    /// Shuffles an array
    /// </summary>
    public string[] ShuffleQuestionArray(string[] array)
    {
        // Get the length of the question array
        int length = array.Length;

        // Initialize the loop index
        int index = length - 1;

        // Shuffle the question array
        while(index >= 0)
        {
            // Get a random number
            int swapIndex = random.Next(0, index);

            // Copy entry at swapIndex to the entry index of the array
            string value = array[swapIndex];
            array[swapIndex] = array[index];
            array[index] = value;

            // Reduce the index by one
            index = index - 1;
        }

        // Return the shuffled array
        return array;
    }

    // Method that initializes everything when a level is started
    public void InitializeRound()
    {
        InitializeQuestionArray();
        Questions.questionArray = ShuffleQuestionArray(Questions.questionArray);
    }

    /// <summary>
    /// Get the array of files in a directory
    /// </summary>
    static string[] GetFilesArray(string path) 
    {
        string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        return files;
    }

    /// <summary>
    /// Activates the view model menu and enables the user to open the first question
    /// </summary>
    public void ActivateViewModels()
    {
        // After waiting, enable the view model menu
        viewModel.SetActive(true);
    }

    /// <summary>
    /// if the game overlay is active
    /// </summary>
    /// <returns></returns>
    public bool GameOverlayActivated()
    {
        return gameOverlay.activeSelf == true;
    }

    // Activates the view model menu and enables the user to open the first question
    IEnumerator ActivateGame()
    {
        // After waiting, enable the view model menu
        gameOverlay.SetActive(true);

        Debug.Log("The game overlay should have been enabled");

        yield return new WaitUntil(GameOverlayActivated);

        Debug.Log("Here, the game setup should be reset");

        GameSetup.ResetGameSetup();

        // Make sure all towers are released
        // Get the array of all tower objects
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag ("Tower");
 
        // Disable all towers
        foreach(GameObject tower in towerArray)
        {
            if(tower.activeSelf == true)
            { 
                ObjectPools.ReleaseTower(tower);
            }
        }

        // Get the array of all trap objects
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag ("Trap");
 
        // Disable all traps
        foreach(GameObject trap in trapArray)
        {
            // Check if the trap is active
            if(trap.activeSelf == true)
            {
                ObjectPools.ReleaseTrap(trap.GetComponent<Trap>());
            }
        }

        // Reset all spell cards so that they are not drawn
         GameObject[] spellArray = GameObject.FindGameObjectsWithTag ("Spell Card");

        foreach(GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCardController>().ResetSpellCard();
        }
    }

    private bool lastQuestionWasAnsweredCorrectly = false;

    /// <summary>
    /// Closes the current question, and changes the current question index
    /// </summary>
    public void GoToNextQuestion()
    {
        Questions.currentlyAnsweringQuestion = false;

        // Check if the last question was answered correctly
        if(lastQuestionWasAnsweredCorrectly == true)
        {
            // Reduce the number of questions that need to be answered by one
            Questions.numberOfQuestionsNeededToAnswer--;
            UpdateNumberOfQuestionsThatNeedToBeAnsweredDisplay();
        }

        // Check if the the current question index reached the end of the index
        if(Questions.currentQuestionIndex < Questions.lastQuestionIndex)
        {
            // Case the current question index is smaller than the last question index, increase the current question index by one
            Questions.currentQuestionIndex = Questions.currentQuestionIndex + 1;

        } else {

            // Case the current question index reached the end of the array, shuffle the array and set the current question index to 0
            Questions.questionArray = ShuffleQuestionArray(Questions.questionArray);
            Questions.currentQuestionIndex = 0;
        }

        // Disable the feedback buttons
        feedbackInput.gameObject.SetActive(false);
        feedbackMC.gameObject.SetActive(false);

        // Check if any question still need to be answered
        if(Questions.numberOfQuestionsNeededToAnswer > 0)
        {
           // DisplayModels();
        } else {
           // DisplayModels();
            viewModel.SetActive(false);
        }
    }

    public static void IncreaseNumberOfQuestionsThatNeedToBeAnswered(int number)
    {
        // Increase the number of questions that need to be answered by the number given
        Questions.numberOfQuestionsNeededToAnswer = Questions.numberOfQuestionsNeededToAnswer + number;

        UpdateNumberOfQuestionsThatNeedToBeAnsweredDisplay();
    }

    public static void UpdateNumberOfQuestionsThatNeedToBeAnsweredDisplay()
    {
        // Change the number displayed to the global variables
        NumberOfQuestionsThatNeedToBeAnsweredDisplay.GetComponentInChildren<TMP_Text>().text = Questions.numberOfQuestionsNeededToAnswer.ToString();
    }

    public static void ResetQuestionMenuWindows()
    {
        // Disable the view input question menu
        ViewInputQuestion.SetActive(false);

        // Disable the view multiple choice question menu
        ViewMultipleChoiceQuestion.SetActive(false);

        // Disable the view model (view activate question) menu
        ViewModel.SetActive(false);

        // Reset the number of questions needed to be answered
        Questions.numberOfQuestionsNeededToAnswer = 0;
    }
}
