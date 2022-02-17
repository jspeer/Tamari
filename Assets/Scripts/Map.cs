using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;

public class Map : MonoBehaviour
{
    private string mapName;
    public string MapName { get { return mapName; } }
    public bool NeedText { get { return !String.IsNullOrEmpty(mapName); } }
    public Vector2 WorldPosition { get { return new Vector3(
                                    transform.localPosition.x * transform.lossyScale.x,
                                    transform.localPosition.y * transform.lossyScale.y,
                                    transform.localPosition.z * transform.lossyScale.z
            ); } }
    public Vector2 MinOffset { get { return new Vector3(0f, -GetComponent<SuperMap>().m_Height, 0f ); } }
    public Vector2 MaxOffset { get { return new Vector3(GetComponent<SuperMap>().m_Width, 0f, 0f ); } }

    public Vector4 PositionWithOffset;

    private GameManager gameManager;
    private SuperMap superMap;
    private SuperCustomProperties superCustomProperties;
    private Vector2 size;
    public Vector2 Size { get { return size; } }
    private Vector2 tileSize;
    public Vector2 TileSize { get { return tileSize; } }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Get the supermap from supertiled2unity
        superMap = GetComponent<SuperMap>();
        // populate the tilesize and room size
        tileSize = new Vector2(superMap.m_TileWidth, superMap.m_TileHeight);
        size = new Vector2(superMap.m_Width, superMap.m_Height);

        // Assign the map name from the property
        superCustomProperties = gameObject.GetComponent<SuperCustomProperties>();
        superCustomProperties.TryGetCustomProperty(gameManager.SuperTiledMapNamePropertyName, out CustomProperty mapNameProp);
        if (mapNameProp != null) {
            mapName = mapNameProp.m_Value;
        }

        // Hacky way I had to deal with presenting this combination of information to MapMove.CalculatePlayerOffset()
        // Was too tired to think of a better way
        PositionWithOffset = new Vector4(
            WorldPosition.x,
            WorldPosition.y,
            WorldPosition.x+MaxOffset.x,
            WorldPosition.y+MinOffset.y
        );
    }
}
