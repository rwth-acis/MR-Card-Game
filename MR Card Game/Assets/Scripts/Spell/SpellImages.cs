using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellImages : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------
    // Define all spell card images
    //------------------------------------------------------------------------------------------------------------------

    // The instance of this class to access the static value of certain variables
    public static SpellImages instance;

    // The meteor image
    [SerializeField]
    private Sprite meteorSpellCardImage;

    // The method used to access to the meteor spell card image as a static object
    public static Sprite getMeteorSpellCardImage
    {
        get { return instance.meteorSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite arrowRainSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite thunderStrikeSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite obliterationSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite stopTimeSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite slowTimeSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite spaceDistortionSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite rainSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite healSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite armorSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite drawSpellCardImage;

    // The meteor image
    [SerializeField]
    private Sprite teleportSpellCardImage;

    //------------------------------------------------------------------------------------------------------------------
    // Define all images on the canvas of the image targets
    //------------------------------------------------------------------------------------------------------------------

    // The meteor image
    [SerializeField]
    private GameObject spellAppearanceImageTarget1;

    // The method used to access to the spell appearance of the image target 1 as a static object
    public static GameObject getSpellAppearanceImageTarget1
    {
        get { return instance.spellAppearanceImageTarget1; }
    }

    //------------------------------------------------------------------------------------------------------------------
    // Define the width and height of the images on the image targets
    //------------------------------------------------------------------------------------------------------------------

    // The width of the image targets
    [SerializeField]
    private float imageTargetWidth;

    // The height of the image targets
    [SerializeField]
    private float imageTargetHeight;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method used to display the meteor image on spell cards
    public static void DisplayMeteor(GameObject imageTarget)
    {
        GameObject imageToChange = GetRightImageComponent(imageTarget);
        // Get access to the image object on the canvas of the image target
        // GameObject imageToChange = imageTarget.GetComponentInChildren<Image>();

        if(imageToChange == null)
        {
            Debug.Log("No image component with that name was found");
        } else {
            Debug.Log("The name of the image to change object is: " + imageToChange.name);
        }

        // Set the image active
        imageToChange.GetComponent<Image>().enabled = true;

        // Set the right sprite
        imageToChange.GetComponent<Image>().sprite = getMeteorSpellCardImage;

        // // Scale the image down
        // imageToChange.rectTransform.sizeDelta = new Vector2(imageTargetWidth, imageTargetHeight);
    }

    public static GameObject GetRightImageComponent(GameObject imageTarget)
    {
        // Check what the image target name is, and return the right image object
        switch(imageTarget.name)
        {
            case "LocationSpell1":
                return getSpellAppearanceImageTarget1;
            break;

            case "LocationSpell2":
                return GameObject.Find("AppearanceSpell2");
            break;

            case "LocationSpell3":
                return GameObject.Find("AppearanceSpell3");
            break;

            case "LocationSpell4":
                return GameObject.Find("AppearanceSpell4");
            break;

            case "LocationSpell5":
                return GameObject.Find("AppearanceSpell5");
            break;

            case "LocationSpell6":
                return GameObject.Find("AppearanceSpell6");
            break;

            case "LocationSpell7":
                return GameObject.Find("AppearanceSpell7");
            break;
        }

        return GameObject.Find("AppearanceSpell1");
    }
}
