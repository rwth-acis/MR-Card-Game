using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

public class SpawnTrap : MonoBehaviour
{
    // The instance, needed to access the static versions of the tower prefabs
    public static SpawnTrap Instance;

    [Header("Trap Prefabs")]
    [SerializeField]
    private GameObject hole;
    [SerializeField]
    private GameObject swamp;

    [Header("Trap Properties")]

    [Tooltip("Radius of the hole in meter")]
    [SerializeField]
    private float holeRadius;

    [Tooltip("Radius of the swamp in meter")]
    [SerializeField]
    private float swampRadius;

    [Tooltip("The original radius of both trap prefabs in meter")]
    [SerializeField]
    private float trapPrefabOrignialRadius = 2.5f;
    public static GameObject Hole
    {
        get { return Instance.hole; }
    }

    public static float HoleRadius
    {
        get { return Instance.holeRadius; }
    }

    public static GameObject Swamp
    {
        get { return Instance.swamp; }
    }

    public static float SwampRadius
    {
        get { return Instance.swampRadius; }
    }

    public static float TrapPrefabOriginalRadius
    {
        get => Instance.trapPrefabOrignialRadius;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Spawn a trap with the given parent and trap type
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject SpawnTrapFromPool(GameObject parent, TrapType trapType)
    {
        int poolIndex = ObjectPools.GetTrapPoolIndex(trapType);
        GameObject trap = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return GetTrapOfType(trapType); });
        trap.SetActive(true);
        trap.transform.parent = parent.transform;
        // Here, the prefab should have a scale of (1,1,1).
        float scale = GetTrapRadiusWithType(trapType) / TrapPrefabOriginalRadius;
        Debug.Log(scale);
        trap.transform.localScale = new Vector3(scale, scale, scale);
        trap.transform.localPosition = new Vector3(0, 0, 0);
        return trap;
    }

    /// <summary>
    /// Get the radius of the size with the given type
    /// </summary>
    public static float GetTrapRadiusWithType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Hole => HoleRadius,
            TrapType.Swamp => SwampRadius,
            _ => 0
        };
    }

    /// <summary>
    /// Get the trap with the given type
    /// </summary>
    public static GameObject GetTrapOfType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Hole => Instantiate(Hole),
            TrapType.Swamp => Instantiate(Swamp),
            _ => Instantiate(Hole)
        };
    }
}
