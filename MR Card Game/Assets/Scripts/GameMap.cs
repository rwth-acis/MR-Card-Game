using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Waypoints
{
    public static Transform[] mapWaypoints;

    public static GameObject enemySpawn;
}

namespace map
{
    public class GameMap : MonoBehaviour
    {
        [Tooltip(" Array of waypoints placed on the path of the enemies")]
        [SerializeField]
        private Transform[] mapWaypoints;

        [SerializeField]
        private GameObject enemySpawn;

        void Start()
        {
            Debug.Log(gameObject.GetComponent<Collider>().bounds.size);
            SetMapWaypoints();
            Debug.Log("The map waypoints and enemy spawn were set.");
        }

        private void SetMapWaypoints()
        {
            Waypoints.mapWaypoints = mapWaypoints;
            Waypoints.enemySpawn = enemySpawn;
        }
    }
}
