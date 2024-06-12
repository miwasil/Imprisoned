using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckObjectState : MonoBehaviour
{
    public GameObject targetObject; // Reference to the object to check
    public GameObject TextOfWin;
    public GameObject FindKey;
    public GameObject YouWin;
    public Image blackScreen;

    private bool playerInTrigger = false;

    void Start()
    {
        TextOfWin.SetActive(false);
        FindKey.SetActive(false);
        YouWin.SetActive(false);
        blackScreen.gameObject.SetActive(false); // Ensure the black screen is initially inactive
    }

    void Update()
    {
        if (playerInTrigger && targetObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TextOfWin.SetActive(false);
                ShowWinScreen();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (targetObject.activeInHierarchy)
            {
                TextOfWin.SetActive(true);
            }
            else
            {
                FindKey.SetActive(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            TextOfWin.SetActive(false);
            FindKey.SetActive(false);
        }
    }

    void ShowWinScreen()
    {
        blackScreen.gameObject.SetActive(true);
        YouWin.SetActive(true);
    }
}