using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapMove : MonoBehaviour
{
    private Vector2 playerChange;
    private GameObject dialogBox;
    private TextMeshProUGUI mapTitleText;
    private float mapTitleTextDisplayTime;

    private GameManager parent;
    private Map originMap;
    private Map destinationMap;
    private Vector2 playerDirectionOffset;
    private Vector2 minCameraChange;
    private Vector2 maxCameraChange;
    private CameraMovement cameraMovement;

    private void Awake()
    {
        cameraMovement = Camera.main.GetComponent<CameraMovement>();

        parent = GetComponentInParent<GameManager>();
        playerChange = parent.PlayerChangeAmount;
        dialogBox = parent.DialogBox;
        mapTitleText = parent.MapTitleText;
        mapTitleTextDisplayTime = parent.MapTitleTextDisplayTime;
    }

    private void Start()
    {
        mapTitleText.enabled = false;
    }

    public void MoveToNewMap(Collider2D other, Map originMap, Map destinationMap)
    {
        this.originMap = originMap;
        this.destinationMap = destinationMap;

        CalculatePlayerOffset();

        if (dialogBox != null && dialogBox.activeInHierarchy)
        {
            dialogBox.SetActive(false);
        }

        // Move player and camera
        if (other.CompareTag("Player"))
        {
            // move player and camera
            MoveCamera();
            MovePlayer(other);
            // should we write the text label?
            if (destinationMap.NeedText)
            {
                StopAllCoroutines();
                StartCoroutine(DisplayMapName());
            }
        }
    }

    private void CalculatePlayerOffset()
    {
        // Calculate offset player direction
        // See Map.cs for definition of these Vector4s
        Vector4 o_MapPosOffset = originMap.PositionWithOffset;
        Vector4 d_MapPosOffset = destinationMap.PositionWithOffset;

        // Returns a Vector2 of 0, 1 or -1 in X and Y
        playerDirectionOffset = new Vector2(
            // Do they share a X axis?
            (o_MapPosOffset.x == d_MapPosOffset.z || o_MapPosOffset.z == d_MapPosOffset.x) ? playerDirectionOffset.x = Mathf.Sign(d_MapPosOffset.x - o_MapPosOffset.x) : 0,
            // Do they share a Y axis?
            (o_MapPosOffset.w == d_MapPosOffset.y || o_MapPosOffset.y == d_MapPosOffset.w) ? playerDirectionOffset.y = Mathf.Sign(d_MapPosOffset.w - o_MapPosOffset.w) : 0
        );
    }

    private void MoveCamera()
    {
        // sets the clamp variables for the camera
        CalculateNewCameraLimits(out minCameraChange, out maxCameraChange);
        cameraMovement.minPosition += minCameraChange;
        cameraMovement.maxPosition += maxCameraChange;
    }

    private void CalculateNewCameraLimits(out Vector2 min, out Vector2 max)
    {
        min = -(originMap.WorldPosition+originMap.MinOffset) + (destinationMap.WorldPosition+destinationMap.MinOffset);
        max = -(originMap.WorldPosition+originMap.MaxOffset) + (destinationMap.WorldPosition+destinationMap.MaxOffset);
    }

    private void MovePlayer(Collider2D other)
    {
        // Add player change vars (2D) to other's transform (3D)
        Vector3 playerPositionChange = new Vector3(playerChange.x * playerDirectionOffset.x, playerChange.y * playerDirectionOffset.y, other.transform.position.z);
        other.transform.position += playerPositionChange;
    }

    // writing a text label with timers... oh so fancy!
    private IEnumerator DisplayMapName()
    {
        // if the place text is set properly, lets display some text!
        if (mapTitleText != null) {
            // slight delay
            yield return new WaitForSeconds(mapTitleTextDisplayTime/4);
            // set text and enable
            mapTitleText.text = destinationMap.MapName;
            mapTitleText.enabled = true;
            // another delay
            yield return new WaitForSeconds(mapTitleTextDisplayTime);
            // poof, gone
            mapTitleText.enabled = false;
        }
    }
}
