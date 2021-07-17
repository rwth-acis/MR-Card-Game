using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class of the castle game object
static class GameAdvancement
{
    // The maximum and current health point of the castle
    public static int castleMaxHP;
    public static int castlecurrentHP;

    // The current armor points of the castle
    public static int castleCurrentAP;

    // The amount of currency the player has right now
    public static int currencyPoints;

    // The current wave
    public static int currentWave;
}


public class NewBehaviourScript : MonoBehaviour
{
    // The health points the castle has. Can be changed in the inspector.
    [SerializeField]
    private int castleHP;

    // The currency the player should have at the beginning. Can be changed in the inspector.
    [SerializeField]
    private int beginingCurrency;

    // Start is called before the first frame update
    void Start()
    {
        // Set the castle max and current hp to the hp given in the inspector
        GameAdvancement.castleMaxHP = castleHP;
        GameAdvancement.castlecurrentHP = castleHP;

        // Set the currency points to the amount the player should have in the begining
        GameAdvancement.currencyPoints = beginingCurrency;

        // Set the wave counter to wave 1
        GameAdvancement.currentWave = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Display currency points
    }
}
