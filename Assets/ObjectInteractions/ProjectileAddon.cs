using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public GameObject explosionPrefab;
    private Rigidbody rb;
    private bool targetHit;
    public float explosionLifetime = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;
        else
            targetHit = true;
        

        rb.isKinematic = true;
        transform.SetParent(collision.transform);
        
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);


        Destroy(gameObject);
        Destroy(explosion, explosionLifetime);
    }
}
