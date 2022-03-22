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
using UnityEngine.Networking;

public class DownloadLevels : MonoBehaviour
{
    // Define the model preview buttons
    [SerializeField] private Button[] directories = new Button[5];

    //DownloadLevels menu button
    [SerializeField] private Button downloadLevelsMenuButton;

    // The directories array
    private string[] directoriesArray;

    //The current Quizname
    private string currentQuizname;

    // The number of the current page
    private int currentPage;

    private void Start()
    {
        StartCoroutine(GetRequest("quiznames.txt",0));
    }

    // Update is called once per frame
    void Update()
    {
        //Disable download levels menu button if there is not internet connection
        downloadLevelsMenuButton.interactable = (Application.internetReachability != NetworkReachability.NotReachable);
    }

    //Download specific quiz
    private void DownloadQuiz(string quizname)
    {
        if(!File.Exists(Application.persistentDataPath + "/" + quizname + "/description.json"))
        {
            //Download the description file
            Directory.CreateDirectory(Application.persistentDataPath + "/" + quizname);
            StartCoroutine(GetRequest(quizname + "/Description.json", 1));
        }
    }

    public void DownloadButton(TMP_Text textchild)
    {
        currentQuizname = textchild.text;
        DownloadQuiz(currentQuizname);
    }

    //Update the directoriesArray
    private void UpdateDirectoriesArray(string quiznames)
    {
        directoriesArray = quiznames.Split(',');
        
        for(int i = 0; i < 5; i++)
        {
            if (i > directoriesArray.Length - 1)
            {
                directories[i].GetComponentInChildren<TMP_Text>().SetText("-");
                directories[i].GetComponent<Button>().interactable = false;
            } else
            {
                directories[i].GetComponentInChildren<TMP_Text>().SetText(directoriesArray[i]);
                directories[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    //Manage the Webrequest
    IEnumerator GetRequest(string uri, int typeOfRequest)
    {
        string requesturi = "https://raw.githubusercontent.com/JulianStaab/mr-card-game-quizzes/main/" + uri;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(requesturi))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = requesturi.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    switch(typeOfRequest)
                    {
                        case 0:
                            UpdateDirectoriesArray(webRequest.downloadHandler.text);
                            break;
                        case 1:
                            string result = webRequest.downloadHandler.text;
                            File.WriteAllText(Application.persistentDataPath + "/" + uri, result);
                            string[] resultArray = result.Split(':');
                            resultArray = resultArray[1].Split(',');
                            result = resultArray[0];
                            for(int i = 0; i < int.Parse(result);i++)
                            {
                                StartCoroutine(GetRequest(currentQuizname + "/Question00"+ i +".json", 2));
                            }
                            break;
                        case 2:
                            File.WriteAllText(Application.persistentDataPath + "/" + uri, webRequest.downloadHandler.text);
                            break;
                    }
                    break;
            }
        }
    }
}
