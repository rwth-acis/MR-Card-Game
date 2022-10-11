using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{
    public Color selectedColor;
    public Color deselectedColor;

    /// <summary>
    /// Activated when pressing a button, so that an answer can be selected and deselected
    /// </summary>
    public void SelectOrDeselectButton()
    {
        // Get the current button name
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        Button currentButton = GameObject.Find(buttonName).GetComponent<Button>();

        // Deselect the button
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        if(currentButton.GetComponent<Image>().color == selectedColor)
        {
            currentButton.GetComponent<Image>().color = deselectedColor;
        } else {
            currentButton.GetComponent<Image>().color = selectedColor;
        }
    }
}
