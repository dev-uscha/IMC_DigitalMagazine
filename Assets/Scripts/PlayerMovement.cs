using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;                   // Bewegungsgeschwindigkeit
    public float mouseSensitivity = 2f;            // Maus-Sensitivität
    public float verticalRotationLimit = 80f;      // Begrenzung der Kamerarotation nach oben/unten
    public float jumpHeight = 2f;                  // Sprunghöhe
    public float interactDistance = 3f;            // Interaktionsabstand

    private float rotationX = 0f;
    private float ySpeed = 0f;                     // Geschwindigkeit in der Y-Achse (für Sprünge und Schwerkraft)

    private CharacterController characterController;

    void Start()
    {
        // Den CharacterController initialisieren
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Mausbewegung für die Kamerarotation (oben/unten und links/rechts)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);

        // Kamerarotation um die X-Achse (vertikal)
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Spielerrotation um die Y-Achse (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Bewegung (WASD oder Pfeiltasten)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Bewegungsrichtung (horizontal und vertikal)
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        // Springen (wenn der Spieler am Boden ist)
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            ySpeed = jumpHeight;
        }

        // Schwerkraft anwenden
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // Bewegung inklusive Schwerkraft
        moveDirection.y = ySpeed;

        // Bewege den Spieler
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Interaktion (bei Druck auf die 'E'-Taste)
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
    }

    void InteractWithObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Überprüfe, ob der Raycast ein Objekt trifft
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Wenn das Objekt ein interaktives Objekt ist, führe die Interaktion aus
            if (hit.transform != null)
            {
                Debug.Log("Interagiert mit: " + hit.transform.name);

                // Beispiel: Interaktion mit einem Objekt, das ein InteractiveObject-Skript hat
                if (hit.transform.CompareTag("InteraktivesObjekt"))
                {
                    hit.transform.GetComponent<InteractiveObject>().Interact();
                }
            }
        }
    }
}
