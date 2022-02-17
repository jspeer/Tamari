using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Configuration Properties for transfering data from Tiles to Unity
    [Header("Overlay Configuration")]
    [SerializeField] private TextMeshProUGUI mapTitleText;
    public TextMeshProUGUI MapTitleText { get { return mapTitleText; } }
    [SerializeField] private float mapTitleTextDisplayTime = 2f;
    public float MapTitleTextDisplayTime { get { return mapTitleTextDisplayTime; } }
    [Header("Tiled Map Parameters")]
    [SerializeField] private string superTiledMapNamePropertyName;
    [SerializeField] private Vector2 playerChangeAmount;
    public Vector2 PlayerChangeAmount { get { return playerChangeAmount; } }
    public string SuperTiledMapNamePropertyName { get { return superTiledMapNamePropertyName; } }
    [Header("Room Transfer Triggers")]
    [SerializeField] private string originPropertyName;
    public string SuperTiledOriginPropertyName { get { return originPropertyName; } }
    [SerializeField] private string destinationPropertyName;
    public string SuperTiledDestinationPropertyName { get { return destinationPropertyName; } }
    [SerializeField] private string roomChangeTag;
    public string SuperTiledRoomChangeTriggerTag { get { return roomChangeTag; } }
    [Header("Interactable Objects Parameters")]
    [SerializeField] private GameObject dialogBox;
    public GameObject DialogBox { get { return dialogBox; } }
    [SerializeField] private float autoHideOnLeaveDelay = 2f;
    public float AutoHideOnLeaveDelay { get { return autoHideOnLeaveDelay; } }
    [Header("Interactable Signs")]
    [SerializeField] private string signTriggerTag;
    public string SuperTiledSignTriggerTag { get { return signTriggerTag; } }

    // SuperTiled2Unity World Object
    private SuperWorld worldObject;

    // MapMove instance
    private MapMove mapMover;
    public MapMove MapMover { get { return mapMover; } }

    // Static properties for localization
    public static string localizationTablePropertyName = "LocalizationTable";
    public static string localizationTableKeyPropertyName = "LocalizationTableKey";

    private void Awake()
    {
        // Find the SuperWorld
        worldObject = FindObjectOfType<SuperWorld>();
        mapMover = GetComponent<MapMove>();
    }

    private void Start()
    {
        // Attach World script to SuperWorld object
        worldObject.gameObject.AddComponent<WorldMap>();
    }

    public GameObject FindObjectInSuperWorld(string objectName)
    {
        objectName = $"{worldObject.name}/{objectName}";
        return GameObject.Find(objectName);
    }
}
