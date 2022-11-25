using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class where the trap that opened the delete trap menu is saved
public static class TrapDeleter
{
    public static Trap currentlyOpenedTrapWindow;
}

public class Trap : MonoBehaviour
{
    [SerializeField]
    private TrapType trapType;

    [SerializeField]
    private float slowFactor;

    // The inverse factors
    private float slowRemoveFactor;

    public TrapType TrapType
    {
        get { return trapType; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the hole slow remove factor
        slowRemoveFactor = (1 / slowFactor);
    }

    /// <summary>
    /// Detect that the image target entered the game board space
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            // Slow the enemy by the slow factor
            other.GetComponent<Enemy>().enemySlowFactor = other.GetComponent<Enemy>().enemySlowFactor * slowFactor;
            if(trapType == TrapType.Swamp)
            {
                other.GetComponent<Enemy>().IsWet = true;
            }
        }
    }

    /// <summary>
    /// Detect that the image target left the game board space
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //remove the slow effect
            other.GetComponent<Enemy>().enemySlowFactor = other.GetComponent<Enemy>().enemySlowFactor * slowRemoveFactor;
            if(trapType == TrapType.Swamp)
            {
                other.GetComponent<Enemy>().IsWet = false;
            }
        }
    }

    /// <summary>
    /// Activated when clicking on the hidden button on towers to upgrade them
    /// </summary>
    public void TryOpeningDeleteTrapMenu()
    {
        // Check that nothing is beeing build or upgraded
        if(GameAdvancement.gamePaused == false)
        {
            UpgradeTower.OpenDeleteTrapMenu(this);
        }
    }
}
