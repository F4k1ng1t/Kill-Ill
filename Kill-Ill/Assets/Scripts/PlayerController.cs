using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    [SerializeField] private LayerMask npcLayer;


    //Interaction stuff
    [SerializeField] private GameObject canInteractText;
    private NPCPersona selectedNPC;
    private bool isInteracting = false;
    private bool _canInteract = false;
    private bool canInteract
    {
        set
        {
            _canInteract = value;
            canInteractText.SetActive(_canInteract);
        }

        get
        {
            return _canInteract;
        }
    }

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private bool isUIOpen = false;
    [SerializeField] private GameObject UI;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleRaycastCheck();
        HandleInteraction();
    }

    void HandleMovement()
    {
        if (isUIOpen)
            return;

        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // keeps player grounded

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        if (isUIOpen)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleRaycastCheck()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, npcLayer))
        {
            canInteract = true;
            selectedNPC = hit.transform.GetComponent<NPCPersona>();
            return;
        }
        canInteract = false;
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //this is temporary the interaction UI will pop up when you talk to an NPC. Although being able to pull up the notes at any time would be cool so I can have it be seperate - Ethan
        {
            if (!isInteracting)
            {
                if (selectedNPC && canInteract)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    isUIOpen = true;
                    isInteracting = true;
                    UI.SetActive(isUIOpen);
                    selectedNPC.Interact();
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isUIOpen = false;
                isInteracting = false;
                UI.SetActive(isUIOpen);
                selectedNPC.Leave();
            }
        }
    }
}
