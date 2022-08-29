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

    [SerializeField] private TextMeshProUGUI currentPageText;

    // Define the previous and next buttons
    [SerializeField] private Button previousPage;
    [SerializeField] private Button nextPage;

    // The directories array
    private string[] directoriesArray;

    //The current Quizname
    private string currentQuizname;

    // The number of the current page
    private int currentPage = 1;

    // The number of the current page
    private int numberOfPages;

    private void OnEnable()
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
        directoriesArray[directoriesArray.Length - 1] = directoriesArray[directoriesArray.Length - 1].Trim();

        UpdateButtons();

        for (int i = 0; i < 5; i++)
        {
            if (i > directoriesArray.Length - 1)
            {
                directories[i].GetComponentInChildren<TMP_Text>().SetText("-");
                directories[i].GetComponent<Button>().interactable = false;
                directories[i].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 0);
                directories[i].GetComponentsInChildren<Image>()[2].color = new Color(1, 1, 1, 0);

            } else
            {
                bool downloaded = File.Exists(Application.persistentDataPath + "/" + directoriesArray[i] + "/Description.json");
                directories[i].GetComponentInChildren<TMP_Text>().SetText(directoriesArray[i]);
                directories[i].GetComponent<Button>().interactable = true;
                if (downloaded)
                {
                    var colors = directories[i].GetComponent<Button>().colors;
                    colors.normalColor = new Color(1, 1, 1, 1);
                    colors.selectedColor = new Color(1, 1, 1, 1);
                    directories[i].GetComponent<Button>().colors = colors;
                    directories[i].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 0);
                    directories[i].GetComponentsInChildren<Image>()[2].color = new Color(1, 1, 1, 0.784f);
                }
                else
                {
                    var colors = directories[i].GetComponent<Button>().colors;
                    colors.normalColor = new Color(1, 1, 1, 0.392f);
                    colors.selectedColor = new Color(1, 1, 1, 0.682f);
                    directories[i].GetComponent<Button>().colors = colors;
                    directories[i].GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 0.784f);
                    directories[i].GetComponentsInChildren<Image>()[2].color = new Color(1, 1, 1, 0);
                }
            }
        }
    }

    public void UpdateButtons()
    {
        numberOfPages = System.Convert.ToInt32(System.Math.Ceiling((double)directoriesArray.Length / 5.0));

        // Change the current page text
        currentPageText.text = "Page " + currentPage + "/" + numberOfPages;

        // Enable / Disable previous button and change color
        previousPage.interactable = (currentPage != 1);

        // Enable / Disable next button and change color
        nextPage.interactable = (currentPage != numberOfPages);
    }

    // Method that is activated when pressing next (change the other directories)
    public void NextPage()
    {
        currentPage = currentPage + 1;
        UpdateButtons();
    }

    // Method that is activated when pressing previous (change the other directories)
    public void PreviousPage()
    {
        currentPage = currentPage - 1;
        UpdateButtons();
    }


    //Manage the Webrequest
    IEnumerator GetRequest(string uri, int typeOfRequest)
    {
        string requesturi = "https://raw.githubusercontent.com/rwth-acis/mr-card-game-quizzes/main/" + uri;

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
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
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
                                if(i < 10)
                                    StartCoroutine(GetRequest(currentQuizname + "/Question00"+ i +".json", 2));
                                else if (i < 100)
                                    StartCoroutine(GetRequest(currentQuizname + "/Question0" + i + ".json", 2));
                                else
                                    StartCoroutine(GetRequest(currentQuizname + "/Question" + i + ".json", 2));
                            }
                            UpdateDirectoriesArray(string.Join(",",directoriesArray));
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
