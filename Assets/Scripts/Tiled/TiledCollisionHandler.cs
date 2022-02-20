using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class TiledCollisionHandler : MonoBehaviour
{
    private GameObject originMapObject;
    private Map originMap;
    private GameObject destinationMapObject;
    private Map destinationMap;

    private string localizationTable;
    private string localizationTableKey;
    private string message;
    public string Message { get { return message; } }

    private GameManager gameManager;
    private SuperCustomProperties superCustomProperties;
    private MapMove mapMove;
    private InteractableSign interactableSign;

    void Start()
    {
        // Get the game manager
        gameManager = FindObjectOfType<GameManager>();
        // ... and the custom properties
        superCustomProperties = gameObject.GetComponent<SuperCustomProperties>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            // TODO: ADD NEW TAGS HERE
            switch (tag) {
                // some tricky shit to get switch/case to use non-constant values
                // CHANGE ROOM TRIGGER
                case var value when value == gameManager.SuperTiledRoomChangeTriggerTag:
                    RoomChangeTrigger(other);
                    break;
                // SIGN TRIGGER
                case var value when value == gameManager.SuperTiledSignTriggerTag:
                    GetLocalizedMessage();
                    gameManager.GetComponent<InteractableSign>().TriggerEnter2D(other, message);
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            // TODO: ADD NEW TAGS HERE
            switch (tag) {
                // some tricky shit to get switch/case to use non-constant values
                // CHANGE ROOM TRIGGER
                case var value when value == gameManager.SuperTiledRoomChangeTriggerTag:
                    break;
                // SIGN TRIGGER
                case var value when value == gameManager.SuperTiledSignTriggerTag:
                    gameManager.GetComponent<InteractableSign>().TriggerExit2D();
                    break;
                default:
                    break;
            }
        }
    }

    private void RoomChangeTrigger(Collider2D other)
    {
        superCustomProperties.TryGetCustomProperty(gameManager.SuperTiledOriginPropertyName, out CustomProperty originRoomProp);
        if (originRoomProp != null)
        {
            originMapObject = gameManager.FindObjectInSuperWorld(originRoomProp.m_Value);
            originMap = originMapObject.GetComponent<Map>();
        }
        superCustomProperties.TryGetCustomProperty(gameManager.SuperTiledDestinationPropertyName, out CustomProperty destinationRoomProp);
        if (destinationRoomProp != null)
        {
            destinationMapObject = gameManager.FindObjectInSuperWorld(destinationRoomProp.m_Value);
            destinationMap = destinationMapObject.GetComponent<Map>();
        }

        // This is a room change trigger
        if (originMap != null && destinationMap != null)
        {
            // Call room mover
            gameManager.MapMover.MoveToNewMap(other, originMap, destinationMap);
        }
    }

    private void GetLocalizedMessage()
    {
        superCustomProperties.TryGetCustomProperty(GameManager.localizationTablePropertyName, out CustomProperty localizationTable);
        superCustomProperties.TryGetCustomProperty(GameManager.localizationTableKeyPropertyName, out CustomProperty localizationTableKey);

        if (localizationTable != null && localizationTableKey != null) {
            LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult tableEntry = gameManager.GetComponent<Locale>().GetResult(localizationTable.m_Value, localizationTableKey.m_Value);
            try {
                message = tableEntry.Entry.LocalizedValue;
            } catch (Exception) {
                message = "";
            }
        }
    }
}
