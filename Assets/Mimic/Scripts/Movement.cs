using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 2f;

        public float speed = 5f;
        private Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        private Mimic myMimic;

        public NavMeshAgent agent;
        public Transform player;
        public LayerMask whatIsGround, whatIsPlayer;

        public float health;

        //Patrolling
        private Vector3 walkPoint;
        private bool walkPointSet;
        public float walkPointRange;

        //States
        public float sightRange, attackRange;
        private bool playerInSightRange, playerInAttackRange;

        private void Awake()
        {
            player = GameObject.Find("hero").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            if (myMimic == null)
            {
                Debug.LogError("Mimic component not found on this GameObject.");
            }
        }

        private void Update()
        {
            // Update the enemy's velocity
            //velocity = Vector3.Lerp(velocity, (walkPoint - transform.position).normalized * speed, velocityLerpCoef * Time.deltaTime);
           
            //myMimic.velocity = velocity;
            

            // Adjust height based on the ground
            AdjustHeight();

            // Check player's position relative to the enemy
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
                Patroling();
            
            
            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
        }

        private void AdjustHeight()
        {
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
            {
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            }
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        private void Patroling()
        {
            velocity = Vector3.Lerp(velocity, (walkPoint - transform.position).normalized * speed, velocityLerpCoef * Time.deltaTime);
           
            myMimic.velocity = velocity;
            
            if (!walkPointSet)
                SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
                Vector3 distanceToWalkPoint = transform.position - walkPoint;

                // Walkpoint reached
                if (distanceToWalkPoint.magnitude < 2f)
                    walkPointSet = false;
            }
        }

        private void SearchWalkPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
            randomDirection += transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkPointRange, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                walkPointSet = true;
            }
        }

        private void ChasePlayer()
        {
            
            agent.SetDestination(player.position);
            velocity = Vector3.Lerp(velocity, (player.position - transform.position).normalized * speed, velocityLerpCoef * Time.deltaTime);
            myMimic.velocity = velocity;

        }
    }
}
