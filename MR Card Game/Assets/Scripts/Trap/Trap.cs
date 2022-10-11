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
    // The trap type
    [SerializeField]
    private TrapType trapType;

    // The hole slow factor
    [SerializeField]
    private float slowFactor;

    // The inverse factors
    private float slowRemoveFactor;

    // Method used to get the type of the trap
    public TrapType TrapType
    {
        get { return trapType; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the hole slow remove factor
        slowRemoveFactor = (1 / slowFactor);

        Debug.Log("Slow factor: " + slowFactor);
        Debug.Log("Slow remove factor: " + slowRemoveFactor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // The method used to detect that the image target entered the game board space
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.CompareTag("Enemy"))
        {
            // Slow the enemy by the slow factor
            other.GetComponent<Enemy>().enemySlowFactor = other.GetComponent<Enemy>().enemySlowFactor * slowFactor;

            // Check if the trap type is a swamp
            if(trapType == TrapType.Swamp)
            {
                // Make the enemy wet since this is a swamp
                other.GetComponent<Enemy>().IsWet = true;
            }
        }
    }

    // The method used to detect that the image target left the game board space
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.CompareTag("Enemy"))
        {
            // Remove the slow on the enemy with the slow remove factor
            other.GetComponent<Enemy>().enemySlowFactor = other.GetComponent<Enemy>().enemySlowFactor * slowRemoveFactor;

            // Check if the trap type is a swamp
            if(trapType == TrapType.Swamp)
            {
                // Remove the enemy wetness
                other.GetComponent<Enemy>().IsWet = false;
            }
        }
    }

    // The method activated when clicking on the hidden button on towers to upgrade them
    public void TryOpeningDeleteTrapMenu()
    {
        // Debug.Log("Upgrade tower button was pressed!");
        // Debug.Log("The number of questions that need to be answered is: " + Questions.numberOfQuestionsNeededToAnswer);
        // Debug.Log("The value of the game paused variable is: " + GameAdvancement.gamePaused);

        // Check that nothing is beeing build or upgraded
        if(GameAdvancement.gamePaused == false)
        {
            // Open the upgrade tower menu with the method of another script
            UpgradeTower.OpenDeleteTrapMenu(this);
        }
    }
}
