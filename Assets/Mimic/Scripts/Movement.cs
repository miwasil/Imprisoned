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
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        //
        public NavMeshAgent agent;

        public Transform player;

        public LayerMask whatIsGround, whatIsPlayer;

        public float health;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

      
        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;


        private void Awake()
        {
            player = GameObject.Find("RigidBodyFPSController").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
        }
        
        

        void Update()
        {
            velocity = Vector3.Lerp(velocity,
                (walkPoint - transform.position).normalized * speed,
                velocityLerpCoef * Time.deltaTime);
            myMimic.velocity = velocity;
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
            
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) 
                Patroling();
            
            if (playerInSightRange && !playerInAttackRange) 
                ChasePlayer();
        }
        
        private void Patroling()
        {
            if (!walkPointSet) 
                SearchWalkPoint();

            if (walkPointSet)
            {
                
                agent.SetDestination(walkPoint);


                Vector3 distanceToWalkPoint = transform.position - walkPoint;

                //Walkpoint reached
                if (distanceToWalkPoint.magnitude < 1f)
                    walkPointSet = false;
            }
        }
        
        private void SearchWalkPoint()
        {
            NavMeshHit hit;
            Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
            randomDirection += transform.position;
    
            if (NavMesh.SamplePosition(randomDirection, out hit, walkPointRange, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                walkPointSet = true;
            }
        }

        
        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }
    }
}
