using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

public class SpawnSpellEffect : MonoBehaviour
{
    [Header("Spell Effect Prefabs")]
    [SerializeField]
    private GameObject spellEffectParent;
    [SerializeField]
    private GameObject arrowRain;
    [SerializeField]
    private GameObject meteorImpact;
    [SerializeField]
    private GameObject thunderStrike;
    [SerializeField]
    private GameObject spaceDistortion;

    private Vector3 arrowRainOriginalScale;
    private Vector3 meteorOriginalScale;
    private Vector3 thunderStrikeOriginalScale;
    private Vector3 spaceDistortionOriginalScale;

    /// <summary>
    /// The instance of this class so that the static prefabs can be accessed
    /// </summary>
    public static SpawnSpellEffect Instance;

    public static GameObject SpellEffectParent
    {
        get { return Instance.spellEffectParent; }
    }

    public static GameObject ArrowRain
    {
        get { return Instance.arrowRain; }
    }

    public static GameObject MeteorImpact
    {
        get { return Instance.meteorImpact; }
    }

    public static GameObject ThunderStrike
    {
        get { return Instance.thunderStrike; }
    }

    public static GameObject SpaceDistortion
    {
        get => Instance.spaceDistortion;
    }

    void Start()
    {
        Instance = this;
        arrowRainOriginalScale = arrowRain.transform.localScale;
        meteorOriginalScale = meteorImpact.transform.localScale;
        thunderStrikeOriginalScale = thunderStrike.transform.localScale;
        spaceDistortionOriginalScale = spaceDistortion.transform.localScale;
    }

    /// <summary>
    /// Get a spell effect from the pool with the given type
    /// </summary>
    public static GameObject SpawnSpellFromPool(SpellType spell)
    {
        int poolIndex;
        GameObject spellEffect;
        switch (spell)
        {
            case SpellType.ArrowRain:
                poolIndex = ObjectPools.GetSpellEffectPoolIndex(SpellType.ArrowRain);
                spellEffect = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(ArrowRain); });
                break;
            case SpellType.Meteor:
                poolIndex = ObjectPools.GetSpellEffectPoolIndex(SpellType.Meteor);
                spellEffect = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(MeteorImpact); });
                break;
            case SpellType.ThunderStrike:
                poolIndex = ObjectPools.GetSpellEffectPoolIndex(SpellType.ThunderStrike);
                spellEffect = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(ThunderStrike); });
                break;
            case SpellType.SpaceDistortion:
                poolIndex = ObjectPools.GetSpellEffectPoolIndex(SpellType.SpaceDistortion);
                spellEffect = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(SpaceDistortion); });
                break;
            default:
                spellEffect = new GameObject();
                break;
        }
        spellEffect.SetActive(true);
        float diameter = SpellCardManager.GetSpellRadiusWithType(spell) * 2;
        // For thunder strike, we use a specific scale, i.e. (0.03,0.03,0.03)
        Vector3 scale = spell == SpellType.ThunderStrike ? new Vector3(0.03f, 0.03f, 0.03f) : new Vector3(diameter, diameter, diameter);
        if(diameter > 0)
        {
            spellEffect.transform.localScale = GetSpellEffectOriginalScale(spell).x / 1 * scale;
        }
        else
        {
            spellEffect.transform.localScale = new Vector3(1, 1, 1);
        }
        return spellEffect;
    }

    public static Vector3 GetSpellEffectOriginalScale(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.ArrowRain:
                return Instance.arrowRainOriginalScale;
            case SpellType.Meteor:
                return Instance.meteorOriginalScale;
            case SpellType.ThunderStrike:
                return Instance.thunderStrikeOriginalScale;
            case SpellType.SpaceDistortion:
                return Instance.spaceDistortionOriginalScale;
            default:
                Debug.Log("The given spell has no visual effect");
                return Vector3.zero;
        }
    }
}
