using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    private readonly System.Random random = new System.Random();


    public void LoadGameSceneAsync()
    {
        SceneManager.sceneLoaded -= ActiveGameSceneOnLoad;
        SceneManager.sceneLoaded += ActiveGameSceneOnLoad;
        Scene persistentScene = SceneManager.GetSceneByName("i5 Persistent Scene");
        if (persistentScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("IntroScene");
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync("GameScene");
        }
    }

    private void ActiveGameSceneOnLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene == SceneManager.GetSceneByName("GameScene"))
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    /// <summary>
    /// Method that initializes everything when a level is started
    /// </summary>
    public void InitializeRound()
    {

        // Initialize the question array
        InitializeQuestionArray();

        // Shuffle the question array
        Questions.questionArray = ShuffleQuestionArray(Questions.questionArray);
    }

    /// <summary>
    /// Method that creates the question array (not shuffled)
    /// </summary>
    public void InitializeQuestionArray()
    {
        // Get the array of question files
        string[] questions = Directory.GetFiles(Questions.pathToLevel, "Question*.json", SearchOption.TopDirectoryOnly);

        // Set the Questions.questionArray right
        Questions.questionArray = questions;

        // Set the last question index
        Questions.lastQuestionIndex = Questions.questionArray.Length - 1;

        // Set the current question index to 0
        Questions.currentQuestionIndex = 0;
    }

    /// <summary>
    /// Method that shuffles an array
    /// </summary>
    public string[] ShuffleQuestionArray(string[] array)
    {
        // Get the length of the question array
        int length = array.Length;

        // Initialize the swap index
        int swapIndex = 0;

        // Initialize the loop index
        int index = length - 1;

        // Shuffle the question array
        while (index >= 0)
        {
            // Get a random number
            swapIndex = random.Next(0, index);

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
}

