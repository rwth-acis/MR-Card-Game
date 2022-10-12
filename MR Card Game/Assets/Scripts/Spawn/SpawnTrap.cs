using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;
using Unity.Android.Types;

public class SpawnTrap : MonoBehaviour
{
    // The instance, needed to access the static versions of the tower prefabs
    public static SpawnTrap instance;

    [Header("Trap Prefabs")]
    [SerializeField]
    private Trap hole;
    [SerializeField]
    private Trap swamp;

    [Header("Trap Properties")]

    [Tooltip("Radius of the hole in cm")]
    [SerializeField]
    private float holeSize;

    [Tooltip("Radius of the swamp in cm")]
    [SerializeField]
    private float swampSize;

    public static Trap Hole
    {
        get { return instance.hole; }
    }

    public static float HoleSize
    {
        get { return instance.holeSize; }
    }

    public static Trap Swamp
    {
        get { return instance.swamp; }
    }

    public static float SwampSize
    {
        get { return instance.swampSize; }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Spawn a trap with the given parent and trap type
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static Trap SpawnTrapFromPool(TrapType trapType, GameObject parent)
    {
        int poolIndex = ObjectPools.GetTrapPoolIndex(trapType);
        Trap trap = ObjectPool<Trap>.RequestResource(poolIndex, () => { return GetTrapOfType(trapType); });
        trap.gameObject.SetActive(true);
        trap.gameObject.transform.parent = parent.transform;
        trap.transform.localScale = new Vector3(Board.greatestBoardDimension * GetTrapSizeWithType(trapType) * 3, Board.greatestBoardDimension * 3, Board.greatestBoardDimension * GetTrapSizeWithType(trapType) * 3);
        trap.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        return trap;
    }

    /// <summary>
    /// Get the radius of the size with the given type
    /// </summary>
    public static float GetTrapSizeWithType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Hole => HoleSize,
            TrapType.Swamp => SwampSize,
            _ => 0
        };
    }

    /// <summary>
    /// Get the trap with the given type
    /// </summary>
    public static Trap GetTrapOfType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Hole => Hole,
            TrapType.Swamp => Swamp,
            _ => Hole
        };
    }
}
