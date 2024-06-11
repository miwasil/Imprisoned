using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace

{
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 2f;

        private float speed = 5f;
        private Vector3 velocity = Vector3.zero;
        private float velocityLerpCoef = 4f;
        private Mimic myMimic;
        private FieldOfView fieldOfView;
        public NavMeshAgent agent;
        public Transform player;
        public LayerMask whatIsGround, whatIsPlayer;

        public float health = 100f;

        // Patrolling
        private Vector3 walkPoint;
        private bool walkPointSet;
        public float walkPointRange;

        // States
        public float sightRange, attackRange;
        private bool playerInSightRange, playerInAttackRange;

        // Retreating
        public Vector3 retreatPoint;
        public float retreatDistance = 20f;
        public float retreatHealthThreshold = 20f;
        private bool isRetreating = false;

        // Last seen player position
        private Vector3 lastSeenPlayerPosition;
        private bool isChasingLastSeenPosition = false;
        
        // Timer
        public RoomTimer roomTimer;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            fieldOfView = GetComponent<FieldOfView>();
        }

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            if (myMimic == null)
            {
                Debug.LogError("Mimic component not found on this GameObject.");
            }

            if (fieldOfView == null)
            {
                Debug.LogError("FieldOfView component not found on this GameObject.");
            }

            player = fieldOfView.player; // Ensure player reference is the same
        }

        private void Update()
        {
            // Adjust height based on the ground
            AdjustHeight();
            //speed += (roomTimer.timer/10);
            //agent.acceleration += (roomTimer.timer/10);
            //agent.angularSpeed += (roomTimer.timer/10);

            if (isRetreating)
            {
                Retreat();
            }
            else
            {
                if (fieldOfView.canSeePlayer)
                {
                    lastSeenPlayerPosition = player.position;
                    isChasingLastSeenPosition = true;
                    ChasePlayer();
                }
                else if (isChasingLastSeenPosition)
                {
                    GoToLastSeenPlayerPosition();
                }
                else
                {
                    Patroling();
                }
            }
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

        private void GoToLastSeenPlayerPosition()
        {
            agent.SetDestination(lastSeenPlayerPosition);
            velocity = Vector3.Lerp(velocity, (lastSeenPlayerPosition - transform.position).normalized * speed, velocityLerpCoef * Time.deltaTime);

            if (Vector3.Distance(transform.position, lastSeenPlayerPosition) < 2f)
            {
                isChasingLastSeenPosition = false;
            }
        }

        private void Retreat()
        {
            agent.SetDestination(retreatPoint);

            // Sprawdź, czy przeciwnik dotarł do punktu ucieczki
            if (Vector3.Distance(transform.position, retreatPoint) < 2f)
            {
                health = 100f; // Przywróć pełne zdrowie
                isRetreating = false; // Zresetuj stan ucieczki
                isChasingLastSeenPosition = false;
            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= retreatHealthThreshold && !isRetreating)
            {
                isRetreating = true;
                CalculateRetreatPoint();
                Retreat();
            }
        }

        private void CalculateRetreatPoint()
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            Vector3 proposedRetreatPoint = transform.position + directionAwayFromPlayer * retreatDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(proposedRetreatPoint, out hit, retreatDistance, NavMesh.AllAreas))
            {
                retreatPoint = hit.position;
            }
            else
            {
                // Jeśli nie uda się znaleźć punktu na NavMeshu, ustaw jakikolwiek punkt oddalony
                retreatPoint = proposedRetreatPoint;
            }
        }
    }
}
