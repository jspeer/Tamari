using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;

public class WorldMap : MonoBehaviour
{
    // SuperTiled2Unity SuperMap objects
    private SuperMap[] maps;

    private void Start()
    {
        // Get the map layers from the superworld
        SuperMap[] maps = GetComponentsInChildren<SuperMap>();

        foreach (SuperMap map in maps) {
            // Attach a Map to each of the maps
            map.gameObject.AddComponent<Map>();
            SuperObjectLayer[] collisionLayers = map.GetComponentsInChildren<SuperObjectLayer>();
            // Attach TiledCollisionHandler to each of the collision game layer objects
            foreach (SuperObjectLayer collisionLayer in collisionLayers) {
                SuperObject[] collisionObjects = collisionLayer.GetComponentsInChildren<SuperObject>();
                foreach (SuperObject collisionObject in collisionObjects) {
                    collisionObject.gameObject.AddComponent<TiledCollisionHandler>();
                }
            }
        }
    }
}
