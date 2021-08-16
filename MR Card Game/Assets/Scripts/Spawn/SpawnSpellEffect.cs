using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

public class SpawnSpellEffect : MonoBehaviour
{
    // The instance of this class so that the static prefabs can be accessed
    public static SpawnSpellEffect instance;

    // The game object under which the spell effects will be put in
    [SerializeField]
    private GameObject spellEffectParent;

    // The method used to access to the spell effect parent object as a static object
    public static GameObject getSpellEffectParent
    {
        get { return instance.spellEffectParent; }
    }




    // The prefab for the arrow rain
    [SerializeField]
    private GameObject arrowRain;

    // The method used to access to the arrow rain prefab as a static object
    public static GameObject getArrowRain
    {
        get { return instance.arrowRain; }
    }

    // The prefab for the arrow rain
    [SerializeField]
    private GameObject meteorImpact;

    // The method used to access to the meteor impact prefab as a static object
    public static GameObject getMeteorImpact
    {
        get { return instance.meteorImpact; }
    }

    // The prefab for the arrow rain
    [SerializeField]
    private GameObject lightningStrike;

    // The method used to access to the lightning strike prefab as a static object
    public static GameObject getLightningStrike
    {
        get { return instance.lightningStrike; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set instance to this class
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that spawns an arrow rain spell prefab
    public static GameObject SpawnAnArrowRain()
    {
        // Get the right object pool index for the arrow rain
        int poolIndex = ObjectPools.GetSpellEffectPoolIndex("Arrow Rain");

        // Get a new arrow rain from the right object pool
        GameObject arrowRain = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(getArrowRain);});

        // Set the arrow rain as active
        arrowRain.gameObject.SetActive(true);

        // Set the spell effect under the object that hold the script
        arrowRain.transform.parent = getSpellEffectParent.transform;

        // Scale the spell effect down
        arrowRain.transform.localScale = new Vector3(1, 1, 1);

        // Return the arrow rain object
        return arrowRain;
    }

    // Method that spawns a meteor impact spell prefab
    public static GameObject SpawnAMeteorImpact()
    {
        // Get the right object pool index for the meteor impact
        int poolIndex = ObjectPools.GetSpellEffectPoolIndex("Meteor Impact");

        // Get a new meteor impact from the right object pool
        GameObject meteorImpact = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(getMeteorImpact);});

        // Set the meteor impact as active
        meteorImpact.gameObject.SetActive(true);

        // Set the spell effect under the object that hold the script
        meteorImpact.transform.parent = getSpellEffectParent.transform;

        // Scale the spell effect down
        meteorImpact.transform.localScale = new Vector3(1, 1, 1);

        // Return the meteor impact object
        return meteorImpact;
    }

    // Method that spawns a lightning strike spell prefab
    public static GameObject SpawnAThunderStrike()
    {
        // Get the right object pool index for the lightning strike
        int poolIndex = ObjectPools.GetSpellEffectPoolIndex("Thunder Strike");

        // Get a new lightning strike from the right object pool
        GameObject lightningStrike = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(getLightningStrike);});

        // Set the lightning strike as active
        lightningStrike.gameObject.SetActive(true);

        // Set the spell effect under the object that hold the script
        lightningStrike.transform.parent = getSpellEffectParent.transform;

        // Scale the spell effect down
        lightningStrike.transform.localScale = new Vector3(1, 1, 1);

        // Return the lightning strike object
        return lightningStrike;
    }
}
