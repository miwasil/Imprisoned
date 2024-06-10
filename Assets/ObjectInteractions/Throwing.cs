using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Throwing : MonoBehaviour
{
    [Header("References")] 
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public GameObject reloadZoneObject; // Dodane pole do przypisania obiektu ReloadZone
    public TextMeshProUGUI jarsText; // Referencja do komponentu TextMeshPro

    [Header("Settings")] 
    public int totalThrows = 7; // Ustawienie początkowej liczby rzutów na 7
    public float throwCooldown;

    [Header("Throwing")] 
    public KeyCode farThrowKey = KeyCode.Mouse0; // Lewy przycisk myszy
    public KeyCode nearThrowKey = KeyCode.Mouse1; // Prawy przycisk myszy
    public float farThrowForce;
    public float nearThrowForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
        UpdateJarsText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(farThrowKey) && readyToThrow && totalThrows > 0)
        {
            Throw(farThrowForce);
        }
        else if (Input.GetKeyDown(nearThrowKey) && readyToThrow && totalThrows > 0)
        {
            Throw(nearThrowForce);
        }
    }

    private void Throw(float throwForce)
    {
        readyToThrow = false;
        
        // initiate gameobject
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        
        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        
        totalThrows--;
        UpdateJarsText(); // Aktualizacja tekstu
        
        // cooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzenie kolizji z przypisanym obiektem ReloadZone
        if (other.gameObject == reloadZoneObject)
        {
            ResetThrows();
        }
    }

    private void ResetThrows()
    {
        totalThrows = 7; // Resetowanie liczby rzutów do 7
        UpdateJarsText(); // Aktualizacja tekstu
        Debug.Log("Throws reloaded!");
    }

    private void UpdateJarsText()
    {
        //jarsText.text = totalThrows + "/7"; // Aktualizacja tekstu w TextMeshPro
    }
}
