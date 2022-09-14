using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

public class SpawnSpellEffect : MonoBehaviour
{
    // The instance of this class so that the static prefabs can be accessed
    public static SpawnSpellEffect Instance;

    // The game object under which the spell effects will be put in
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

    // The method used to access to the spell effect parent object as a static object
    public static GameObject SpellEffectParent
    {
        get { return Instance.spellEffectParent; }
    }

    // The method used to access to the arrow rain prefab as a static object
    public static GameObject ArrowRain
    {
        get { return Instance.arrowRain; }
    }

    // The method used to access to the meteor impact prefab as a static object
    public static GameObject MeteorImpact
    {
        get { return Instance.meteorImpact; }
    }

    // The method used to access to the lightning strike prefab as a static object
    public static GameObject ThunderStrike
    {
        get { return Instance.thunderStrike; }
    }

    public static GameObject SpaceDistortion
    {
        get => Instance.spaceDistortion;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set instance to this class
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject SpawnSpell(SpellType spell)
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
        // Set the spell effect under the object that hold the script
        spellEffect.transform.parent = SpellEffectParent.transform;
        spellEffect.transform.localScale = new Vector3(1, 1, 1);
        return spellEffect;
    }

}
