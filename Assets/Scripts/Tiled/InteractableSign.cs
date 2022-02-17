using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSign : MonoBehaviour
{
    private GameManager gameManager;
    private TextMeshProUGUI dialogBoxText;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        dialogBoxText = gameManager.DialogBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameManager.DialogBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (gameManager.DialogBox.activeInHierarchy == true) {
                gameManager.DialogBox.SetActive(false);
            }
        }
    }

    public void TriggerEnter2D(Collider2D other, string message)
    {
        if (other.CompareTag("Player")) {
            dialogBoxText.text = message;
            gameManager.DialogBox.SetActive(true);
        }
    }

    public void TriggerExit2D()
    {
        StartCoroutine(DisableDialogBox());
    }

    private IEnumerator DisableDialogBox()
    {
        if (gameManager.DialogBox.activeInHierarchy == true) {
            yield return new WaitForSeconds(gameManager.AutoHideOnLeaveDelay);
            gameManager.DialogBox.SetActive(false);
        }
    }
}
