using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerIndicator : MonoBehaviour
{
    public bool OverlapWithTowerOrTrap { get; set; }

    private void Start()
    {
        OverlapWithTowerOrTrap = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Trap"))
        {
            OverlapWithTowerOrTrap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Trap"))
        {
            OverlapWithTowerOrTrap = false;
        }
    }
}
