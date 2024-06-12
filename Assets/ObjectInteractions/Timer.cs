using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Timertext;  // Reference to the UI Text component
    public float timer;    // To keep track of the time spent in the room
    private bool playerInRoom;  // To check if the player is in the room

    void Start()
    {
        timer = 0f;  // Initialize timer
        playerInRoom = false;  // Player starts outside the room
        UpdateTimerText();  // Initialize the timer text
    }

    void Update()
    {
        if (!playerInRoom)
        {
            timer += Time.deltaTime; 
            UpdateTimerText();  // Update the UI text
        }
        else
        {
            timer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;  // Reset the timer
            playerInRoom = true;  // Set playerInRoom to true
            UpdateTimerText();  // Update the UI text
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = false;  // Set playerInRoom to false
            UpdateTimerText();  // Update the UI text
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        //Timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}