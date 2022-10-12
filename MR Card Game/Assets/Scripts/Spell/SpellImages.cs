using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellImages : MonoBehaviour
{
    #region Serializable Fields
    [Header("Spell Images")]
    [SerializeField]
    private Sprite meteorSpellCardImage;

    [SerializeField]
    private Sprite arrowRainSpellCardImage;

    [SerializeField]
    private Sprite thunderStrikeSpellCardImage;

    [SerializeField]
    private Sprite obliterationSpellCardImage;

    [SerializeField]
    private Sprite stopTimeSpellCardImage;

    [SerializeField]
    private Sprite slowTimeSpellCardImage;

    [SerializeField]
    private Sprite spaceDistortionSpellCardImage;

    [SerializeField]
    private Sprite rainSpellCardImage;

    [SerializeField]
    private Sprite healingSpellCardImage;

    [SerializeField]
    private Sprite armorSpellCardImage;

    [SerializeField]
    private Sprite drawSpellCardImage;

    [SerializeField]
    private Sprite teleportSpellCardImage;

    [Header("Spell Appearance Objects")]

    [SerializeField]
    private GameObject meteorAppearance;

    [SerializeField]
    private GameObject arrorRainAppearance;

    [SerializeField]
    private GameObject thunderStrikeAppearance;

    [SerializeField]
    private GameObject obliterationAppearance;

    [SerializeField]
    private GameObject stopTimeAppearance;

    [SerializeField]
    private GameObject slowTimeAppearance;

    [SerializeField]
    private GameObject spaceDistortionAppearance;

    [SerializeField]
    private GameObject rainAppearance;

    [SerializeField]
    private GameObject healingAppearance;

    [SerializeField]
    private GameObject armorAppearance;

    [SerializeField]
    private GameObject drawAppearance;

    [SerializeField]
    private GameObject teleportAppearance;
    #endregion

    #region Static Properties
    public static Sprite MeteorSpellCardImage
    {
        get { return Instance.meteorSpellCardImage; }
    }

    public static Sprite ArrowRainSpellCardImage
    {
        get { return Instance.arrowRainSpellCardImage; }
    }

    public static Sprite ThunderStrikeSpellCardImage
    {
        get { return Instance.thunderStrikeSpellCardImage; }
    }

    public static Sprite ObliterationSpellCardImage
    {
        get { return Instance.obliterationSpellCardImage; }
    }

    public static Sprite StopTimeSpellCardImage
    {
        get { return Instance.stopTimeSpellCardImage; }
    }

    public static Sprite SlowTimeSpellCardImage
    {
        get { return Instance.slowTimeSpellCardImage; }
    }

    public static Sprite SpaceDistortionSpellCardImage
    {
        get { return Instance.spaceDistortionSpellCardImage; }
    }

    public static Sprite RainSpellCardImage
    {
        get { return Instance.rainSpellCardImage; }
    }

    public static Sprite HealSpellCardImage
    {
        get { return Instance.healingSpellCardImage; }
    }

    public static Sprite ArmorSpellCardImage
    {
        get { return Instance.armorSpellCardImage; }
    }

    public static Sprite DrawSpellCardImage
    {
        get { return Instance.drawSpellCardImage; }
    }

    public static Sprite TeleportSpellCardImage
    {
        get { return Instance.teleportSpellCardImage; }
    }

    public static GameObject MeteorAppearance
    {
        get => Instance.meteorAppearance;
    }

    public static GameObject ArrowRainAppearance
    {
        get => Instance.arrorRainAppearance;
    }

    public static GameObject ThunderStrikeAppearance
    {
        get => Instance.thunderStrikeAppearance;
    }
    public static GameObject ObliterationAppearance
    {
        get => Instance.obliterationAppearance;
    }

    public static GameObject StopTimeAppearance
    {
        get => Instance.stopTimeAppearance;
    }
    public static GameObject SlowTimeAppearance
    {
        get => Instance.slowTimeAppearance;
    }

    public static GameObject SpaceDistortionAppearance
    {
        get => Instance.spaceDistortionAppearance;
    }
    public static GameObject RainAppearance
    {
        get => Instance.rainAppearance;
    }
    public static GameObject HealingAppearance
    {
        get => Instance.healingAppearance;
    }
    public static GameObject ArmorAppearance
    {
        get => Instance.armorAppearance;
    }

    public static GameObject DrawAppearance
    {
        get => Instance.drawAppearance;
    }
    public static GameObject TeleportAppearance
    {
        get => Instance.teleportAppearance;
    }
    #endregion

    public static SpellImages Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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

    public static GameObject GetRightImageComponent(GameObject imageTarget)
    {
        // Check what the image target name is, and return the right image object
        return imageTarget.name switch
        {
            "Draw" => DrawAppearance,
            "Armor" => ArmorAppearance,
            "ArrorRain" => ArrowRainAppearance,
            "Healing" => HealingAppearance,
            "Meteor" => MeteorAppearance,
            "Obliteration" => ObliterationAppearance,
            "Rain" => RainAppearance,
            "SlowTime" => SlowTimeAppearance,
            "SpaceDistortion" => SpaceDistortionAppearance,
            "StopTime" => StopTimeAppearance,
            "ThunderStrike" => ThunderStrikeAppearance,
            "Teleport" => TeleportAppearance,
            _ => DrawAppearance
        };
    }

    /// <summary>
    /// Get the right spell image from the spell type
    /// </summary>
    public static Sprite GetRightImage(SpellType type)
    {
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
