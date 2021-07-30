using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Method used to exit the application when clicking on the exit button
    public void ExitApplication()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
