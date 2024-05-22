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

    void Start()
    {
        TextOfWin.SetActive(false);
        FindKey.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        TextOfWin.SetActive(false);
        FindKey.SetActive(false);
    }

}