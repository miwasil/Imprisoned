using System;
using System.Collections;
using System.Collections.Generic;
using MimicSpace;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public GameObject explosionPrefab;
    private Rigidbody rb;
    private bool targetHit;
    public float explosionLifetime = 2.0f;
    public float damage = 10.0f; // Damage value

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (targetHit)
            return;
        else
            targetHit = true;

        rb.isKinematic = true;
        transform.SetParent(other.transform);

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Apply damage if the target is an enemy
        if (other.CompareTag("Enemy"))
        {
            Movement enemyMovement = other.GetComponent<Movement>();
            if (enemyMovement != null)
            {
                enemyMovement.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
        Destroy(explosion, explosionLifetime);
    }
}