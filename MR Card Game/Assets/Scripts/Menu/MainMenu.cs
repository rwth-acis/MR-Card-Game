using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Exit the application when clicking on the exit button
    public void ExitApplication()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
