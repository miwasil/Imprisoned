using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    public Vector2 initialCameraRotation; // Pole do ustawienia początkowej rotacji kamery

    Vector2 velocity;
    Vector2 frameVelocity;
    Vector2 oldMouseDelta;
    
    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        oldMouseDelta = new Vector2(0,0);
        // Ustawienie początkowej rotacji kamery i postaci na podstawie bieżącej rotacji bohatera
        Vector3 characterEulerAngles = character.localEulerAngles;
        velocity = new Vector2(characterEulerAngles.y, -characterEulerAngles.x);

        // Jeżeli initialCameraRotation ma być używane jako nadpisanie domyślnej rotacji, można je dodać do velocity
        velocity += initialCameraRotation;
        
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetCameraRotation(new Vector2(0, 180));
        }

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        oldMouseDelta = mouseDelta;
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    void SetCameraRotation(Vector2 newRotation)
    {
        velocity.x = newRotation.y; // Rotacja wokół osi Y (obrót poziomy)
        velocity.y = -newRotation.x; // Rotacja wokół osi X (obrót pionowy)

        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
    }
}
