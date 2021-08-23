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

    // The method used to access to the arrow rain spell card image as a static object
    public static Sprite getArrowRainSpellCardImage
    {
        get { return instance.arrowRainSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite thunderStrikeSpellCardImage;

    // The method used to access to the thunder strike spell card image as a static object
    public static Sprite getThunderStrikeSpellCardImage
    {
        get { return instance.thunderStrikeSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite obliterationSpellCardImage;

    // The method used to access to the obliteration spell card image as a static object
    public static Sprite getObliterationSpellCardImage
    {
        get { return instance.obliterationSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite stopTimeSpellCardImage;

    // The method used to access to the stop time spell card image as a static object
    public static Sprite getStopTimeSpellCardImage
    {
        get { return instance.stopTimeSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite slowTimeSpellCardImage;

    // The method used to access to the slow time spell card image as a static object
    public static Sprite getSlowTimeSpellCardImage
    {
        get { return instance.slowTimeSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite spaceDistortionSpellCardImage;

    // The method used to access to the space distortion spell card image as a static object
    public static Sprite getSpaceDistortionSpellCardImage
    {
        get { return instance.spaceDistortionSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite rainSpellCardImage;

    // The method used to access to the rain spell card image as a static object
    public static Sprite getRainSpellCardImage
    {
        get { return instance.rainSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite healSpellCardImage;

    // The method used to access to the heal spell card image as a static object
    public static Sprite getHealSpellCardImage
    {
        get { return instance.healSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite armorSpellCardImage;

    // The method used to access to the armor spell card image as a static object
    public static Sprite getArmorSpellCardImage
    {
        get { return instance.armorSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite drawSpellCardImage;

    // The method used to access to the draw spell card image as a static object
    public static Sprite getDrawSpellCardImage
    {
        get { return instance.drawSpellCardImage; }
    }

    // The meteor image
    [SerializeField]
    private Sprite teleportSpellCardImage;

    // The method used to access to the teleport spell card image as a static object
    public static Sprite getTeleportSpellCardImage
    {
        get { return instance.teleportSpellCardImage; }
    }

    //------------------------------------------------------------------------------------------------------------------
    // Define all images on the canvas of the image targets
    //------------------------------------------------------------------------------------------------------------------

    // The spell appearance of the spell image target 1
    [SerializeField]
    private GameObject spellAppearanceImageTarget1;

    // The method used to access to the spell appearance of the image target 1 as a static object
    public static GameObject getSpellAppearanceImageTarget1
    {
        get { return instance.spellAppearanceImageTarget1; }
    }

    // The spell appearance of the spell image target 2
    [SerializeField]
    private GameObject spellAppearanceImageTarget2;

    // The method used to access to the spell appearance of the image target 2 as a static object
    public static GameObject getSpellAppearanceImageTarget2
    {
        get { return instance.spellAppearanceImageTarget2; }
    }

    // The spell appearance of the spell image target 3
    [SerializeField]
    private GameObject spellAppearanceImageTarget3;

    // The method used to access to the spell appearance of the image target 3 as a static object
    public static GameObject getSpellAppearanceImageTarget3
    {
        get { return instance.spellAppearanceImageTarget3; }
    }

    // The spell appearance of the spell image target 4
    [SerializeField]
    private GameObject spellAppearanceImageTarget4;

    // The method used to access to the spell appearance of the image target 4 as a static object
    public static GameObject getSpellAppearanceImageTarget4
    {
        get { return instance.spellAppearanceImageTarget4; }
    }

    // The spell appearance of the spell image target 5
    [SerializeField]
    private GameObject spellAppearanceImageTarget5;

    // The method used to access to the spell appearance of the image target 5 as a static object
    public static GameObject getSpellAppearanceImageTarget5
    {
        get { return instance.spellAppearanceImageTarget5; }
    }

    // The spell appearance of the spell image target 6
    [SerializeField]
    private GameObject spellAppearanceImageTarget6;

    // The method used to access to the spell appearance of the image target 6 as a static object
    public static GameObject getSpellAppearanceImageTarget6
    {
        get { return instance.spellAppearanceImageTarget6; }
    }

    // The spell appearance of the spell image target
    [SerializeField]
    private GameObject spellAppearanceImageTarget7;

    // The method used to access to the spell appearance of the image target 7 as a static object
    public static GameObject getSpellAppearanceImageTarget7
    {
        get { return instance.spellAppearanceImageTarget7; }
    }

    //------------------------------------------------------------------------------------------------------------------
    // Define the width and height of the images on the image targets
    //------------------------------------------------------------------------------------------------------------------

    // // The width of the image targets
    // [SerializeField]
    // private float imageTargetWidth;

    // // The height of the image targets
    // [SerializeField]
    // private float imageTargetHeight;

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
    public static void DisplaySpell(GameObject imageTarget, string type)
    {
        // Get the right image to change depending on the current image target
        GameObject imageToChange = GetRightImageComponent(imageTarget);

        // Set the image active
        imageToChange.GetComponent<Image>().enabled = true;

        // Get the right image to change depending on the current image target
        Sprite spellImage = GetRightImage(type);

        // Set the right sprite
        imageToChange.GetComponent<Image>().sprite = spellImage;
    }

    // // Method used to display the meteor image on spell cards
    // public static void DisplayMeteor(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getMeteorSpellCardImage;
    // }

    // // Method used to display the arrow rain image on spell cards
    // public static void DisplayArrowRain(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getArrowRainSpellCardImage;
    // }

    // // Method used to display the thunder strike image on spell cards
    // public static void DisplayThunderStrike(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getThunderStrikeSpellCardImage;
    // }

    // // Method used to display the obliteration image on spell cards
    // public static void DisplayObliteration(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getObliterationSpellCardImage;
    // }

    // // Method used to display the stop time image on spell cards
    // public static void DisplayStopTime(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getStopTimeSpellCardImage;
    // }

    // // Method used to display the slow time image on spell cards
    // public static void DisplaySlowTime(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getSlowTimeSpellCardImage;
    // }

    // // Method used to display the space distortion image on spell cards
    // public static void DisplaySpaceDistortion(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getSpaceDistortionSpellCardImage;
    // }

    // // Method used to display the rain image on spell cards
    // public static void DisplayRain(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getRainSpellCardImage;
    // }

    // // Method used to display the heal image on spell cards
    // public static void DisplayHeal(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getHealSpellCardImage;
    // }

    // // Method used to display the armor image on spell cards
    // public static void DisplayArmor(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getArmorSpellCardImage;
    // }

    // // Method used to display the draw image on spell cards
    // public static void DisplayDraw(GameObject imageTarget)
    // {
    //     // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getDrawSpellCardImage;
    // }

    // // Method used to display the teleport image on spell cards
    // public static void DisplayTeleport(GameObject imageTarget)
    // {
    //      // Get the right image to change depending on the current image target
    //     GameObject imageToChange = GetRightImageComponent(imageTarget);

    //     // Set the image active
    //     imageToChange.GetComponent<Image>().enabled = true;

    //     // Set the right sprite
    //     imageToChange.GetComponent<Image>().sprite = getTeleportSpellCardImage;
    // }

    // Method used to get the right image component from the name of the image target
    public static GameObject GetRightImageComponent(GameObject imageTarget)
    {
        // Check what the image target name is, and return the right image object
        switch(imageTarget.name)
        {
            case "LocationSpell1":
                return getSpellAppearanceImageTarget1;
            break;

            case "LocationSpell2":
                return getSpellAppearanceImageTarget2;
            break;

            case "LocationSpell3":
                return getSpellAppearanceImageTarget3;
            break;

            case "LocationSpell4":
                return getSpellAppearanceImageTarget4;
            break;

            case "LocationSpell5":
                return getSpellAppearanceImageTarget5;
            break;

            case "LocationSpell6":
                return getSpellAppearanceImageTarget6;
            break;

            case "LocationSpell7":
                return getSpellAppearanceImageTarget7;
            break;
        }

        return getSpellAppearanceImageTarget7;
    }

    // Method used to get the right spell image from the spell type
    public static Sprite GetRightImage(string type)
    {
        // Check what the image target name is, and return the right image object
        switch(type)
        {
            case "Meteor":
                return getMeteorSpellCardImage;
            break;

            case "Arrow rain":
                return getArrowRainSpellCardImage;
            break;

            case "Thunder strike":
                return getThunderStrikeSpellCardImage;
            break;

            case "Armor":
                return getArmorSpellCardImage;
            break;

            case "Heal":
                return getHealSpellCardImage;
            break;

            case "Obliteration":
                return getObliterationSpellCardImage;
            break;

            case "Draw":
                return getDrawSpellCardImage;
            break;

            case "Teleport":
                return getTeleportSpellCardImage;
            break;

            case "Space distortion":
                return getSpaceDistortionSpellCardImage;
            break;

            case "Slow time":
                return getSlowTimeSpellCardImage;
            break;

            case "Stop time":
                return getStopTimeSpellCardImage;
            break;

            case "Rain":
                return getRainSpellCardImage;
            break;
        }

        return getMeteorSpellCardImage;
    }
}
