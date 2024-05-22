using UnityEngine;

public class DisableObjectOnLook : MonoBehaviour
{
    public GameObject targetObject;  // The object the player should look at
    public GameObject objectToDisable;  // The object to disable
    public Camera playerCamera;  // The player's camera to determine the look direction
    public float maxDistance = 100f;  // The maximum distance for the raycast

    private bool playerInside;  // Flag to check if player is inside the collider

    void Start()
    {
        playerInside = false;  // Initialize the flag
    }

    void Update()
    {
        if (playerInside)
        {
            CheckIfLookingAtTarget();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;  // Set the flag to true when player enters the collider
            Debug.Log("Player entered the collider.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;  // Set the flag to false when player exits the collider
            Debug.Log("Player left the collider.");
        }
    }

    void CheckIfLookingAtTarget()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);  // Create a ray from the camera's position in the forward direction
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject == targetObject)
            {
                objectToDisable.SetActive(false);  // Disable the object to disable if the raycast hits the target object
                Debug.Log("Object disabled because the player is looking at the target object.");
            }
            else
            {
                Debug.Log("Player is not looking at the target object.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }
}