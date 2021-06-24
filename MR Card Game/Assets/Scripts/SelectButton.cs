using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{
    public Color selectedColor;
    public Color deselectedColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that is activated when pressing a button, so that an answer can be selected and deselected
    public void SelectOrDeselectButton()
    {
        // Get the current button name
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        Button currentButton = GameObject.Find(buttonName).GetComponent<Button>();

        // Deselect the button
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        // If it was selected, deselect it. If it was not selected, select it.
        if(currentButton.GetComponent<Image>().color == selectedColor)
        {
            // Deselecte it
            currentButton.GetComponent<Image>().color = deselectedColor;
        } else {
            // Select it
            currentButton.GetComponent<Image>().color = selectedColor;
        }
    }
}
