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


    // The arror rain image
    [SerializeField]
    private Sprite arrowRainSpellCardImage;

    // The thunder strike image
    [SerializeField]
    private Sprite thunderStrikeSpellCardImage;

    // The obliteration image
    [SerializeField]
    private Sprite obliterationSpellCardImage;

    // The stop time image
    [SerializeField]
    private Sprite stopTimeSpellCardImage;

    // The slow time image
    [SerializeField]
    private Sprite slowTimeSpellCardImage;

    // The space distortion image
    [SerializeField]
    private Sprite spaceDistortionSpellCardImage;

    // The rain image
    [SerializeField]
    private Sprite rainSpellCardImage;

    // The heal image
    [SerializeField]
    private Sprite healSpellCardImage;

    // The armor image
    [SerializeField]
    private Sprite armorSpellCardImage;

    // The draw spell image
    [SerializeField]
    private Sprite drawSpellCardImage;

    // The teleport image
    [SerializeField]
    private Sprite teleportSpellCardImage;

    // The method used to access to the meteor spell card image as a static object
    public static Sprite MeteorSpellCardImage
    {
        get { return instance.meteorSpellCardImage; }
    }
    // The method used to access to the arrow rain spell card image as a static object
    public static Sprite ArrowRainSpellCardImage
    {
        get { return instance.arrowRainSpellCardImage; }
    }

    // The method used to access to the thunder strike spell card image as a static object
    public static Sprite ThunderStrikeSpellCardImage
    {
        get { return instance.thunderStrikeSpellCardImage; }
    }

    // The method used to access to the obliteration spell card image as a static object
    public static Sprite ObliterationSpellCardImage
    {
        get { return instance.obliterationSpellCardImage; }
    }

    // The method used to access to the stop time spell card image as a static object
    public static Sprite StopTimeSpellCardImage
    {
        get { return instance.stopTimeSpellCardImage; }
    }

    // The method used to access to the slow time spell card image as a static object
    public static Sprite SlowTimeSpellCardImage
    {
        get { return instance.slowTimeSpellCardImage; }
    }

    // The method used to access to the space distortion spell card image as a static object
    public static Sprite SpaceDistortionSpellCardImage
    {
        get { return instance.spaceDistortionSpellCardImage; }
    }

    // The method used to access to the rain spell card image as a static object
    public static Sprite RainSpellCardImage
    {
        get { return instance.rainSpellCardImage; }
    }

    // The method used to access to the heal spell card image as a static object
    public static Sprite HealSpellCardImage
    {
        get { return instance.healSpellCardImage; }
    }

    // The method used to access to the armor spell card image as a static object
    public static Sprite ArmorSpellCardImage
    {
        get { return instance.armorSpellCardImage; }
    }

    // The method used to access to the draw spell card image as a static object
    public static Sprite DrawSpellCardImage
    {
        get { return instance.drawSpellCardImage; }
    }

    // The method used to access to the teleport spell card image as a static object
    public static Sprite TeleportSpellCardImage
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
    public static GameObject GetSpellAppearanceImageTarget1
    {
        get { return instance.spellAppearanceImageTarget1; }
    }

    // The spell appearance of the spell image target 2
    [SerializeField]
    private GameObject spellAppearanceImageTarget2;

    // The method used to access to the spell appearance of the image target 2 as a static object
    public static GameObject GetSpellAppearanceImageTarget2
    {
        get { return instance.spellAppearanceImageTarget2; }
    }

    // The spell appearance of the spell image target 3
    [SerializeField]
    private GameObject spellAppearanceImageTarget3;

    // The method used to access to the spell appearance of the image target 3 as a static object
    public static GameObject GetSpellAppearanceImageTarget3
    {
        get { return instance.spellAppearanceImageTarget3; }
    }

    // The spell appearance of the spell image target 4
    [SerializeField]
    private GameObject spellAppearanceImageTarget4;

    // The method used to access to the spell appearance of the image target 4 as a static object
    public static GameObject GetSpellAppearanceImageTarget4
    {
        get { return instance.spellAppearanceImageTarget4; }
    }

    // The spell appearance of the spell image target 5
    [SerializeField]
    private GameObject spellAppearanceImageTarget5;

    // The method used to access to the spell appearance of the image target 5 as a static object
    public static GameObject GetSpellAppearanceImageTarget5
    {
        get { return instance.spellAppearanceImageTarget5; }
    }

    // The spell appearance of the spell image target 6
    [SerializeField]
    private GameObject spellAppearanceImageTarget6;

    // The method used to access to the spell appearance of the image target 6 as a static object
    public static GameObject GetSpellAppearanceImageTarget6
    {
        get { return instance.spellAppearanceImageTarget6; }
    }

    // The spell appearance of the spell image target
    [SerializeField]
    private GameObject spellAppearanceImageTarget7;

    // The method used to access to the spell appearance of the image target 7 as a static object
    public static GameObject GetSpellAppearanceImageTarget7
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
    public static void DisplaySpell(GameObject imageTarget, SpellType type)
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
            case "ImageTargetSpell1":
                return GetSpellAppearanceImageTarget1;

            case "ImageTargetSpell2":
                return GetSpellAppearanceImageTarget2;

            case "ImageTargetSpell3":
                return GetSpellAppearanceImageTarget3;

            case "ImageTargetSpell4":
                return GetSpellAppearanceImageTarget4;

            case "ImageTargetSpell5":
                return GetSpellAppearanceImageTarget5;

            case "ImageTargetSpell6":
                return GetSpellAppearanceImageTarget6;
            case "ImageTargetSpell7":
                return GetSpellAppearanceImageTarget7;
        }

        return GetSpellAppearanceImageTarget7;
    }

    // Method used to get the right spell image from the spell type
    public static Sprite GetRightImage(SpellType type)
    {
        // Check what the image target name is, and return the right image object
        return type switch
        {
            SpellType.Meteor => MeteorSpellCardImage,
            SpellType.ArrowRain => ArrowRainSpellCardImage,
            SpellType.ThunderStrike => ThunderStrikeSpellCardImage,
            SpellType.Armor => ArmorSpellCardImage,
            SpellType.Healing => HealSpellCardImage,
            SpellType.Obliteration => ObliterationSpellCardImage,
            SpellType.Draw => DrawSpellCardImage,
            SpellType.Teleport => TeleportSpellCardImage,
            SpellType.SpaceDistortion => SpaceDistortionSpellCardImage,
            SpellType.SlowTime => SlowTimeSpellCardImage,
            SpellType.StopTime => StopTimeSpellCardImage,
            SpellType.Rain => RainSpellCardImage,
            _ => MeteorSpellCardImage,
        };
    }
}
