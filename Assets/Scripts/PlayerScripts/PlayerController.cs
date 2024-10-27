using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    [Header("Настройки движения")]
    public float speed = 5.0f;
    public float gravity = -9.81f;

    [Header("Настройки камеры")]
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float bobSpeed = 6f;
    [SerializeField] private float bobAmount = 0.06f;

    [Header("Настройки звука")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepsSound;
    [SerializeField] private float stepInterval = 0.5f;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_DefaultCameraPosition;

    private float m_Timer;
    private float m_StepTimer;
    private float m_VerticalRotation;

    private void Start()
    {
        m_DefaultCameraPosition = playerCamera.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;

        AudioSourceInitial();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;

        if (moveDirection.magnitude >= 0.1f)
        {
            moveDirection.Normalize();
        }

        Vector3 move = moveDirection * speed;

        characterController.Move(move * Time.deltaTime);

        if (characterController.isGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -3f;
        }

        m_Velocity.y += gravity * Time.deltaTime;

        characterController.Move(m_Velocity * Time.deltaTime);

        PlayWalkSound(move);
        CameraBob(x, z);
    }

    private void AudioSourceInitial()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = stepsSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void PlayWalkSound(Vector3 move)
    {
        if (move.magnitude > 0.1f)
        {
            m_StepTimer += Time.deltaTime;
            if (m_StepTimer >= stepInterval)
            {
                audioSource.PlayOneShot(stepsSound, audioSource.volume);
                m_StepTimer = 0f;
            }
        }
        else
        {
            m_StepTimer = 0f;
        }
    }

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        m_VerticalRotation -= mouseY;
        m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, -90f, 90f);

        playerCamera.transform.localEulerAngles = new Vector3(m_VerticalRotation, 0, 0);
    }


    private void CameraBob(float x, float z)
    {
        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (isMoving)
        {
            m_Timer += Time.deltaTime * bobSpeed;

            // Reset the timer periodically to prevent floating-point errors
            if (m_Timer > Mathf.PI * 2)
            {
                m_Timer -= Mathf.PI * 2;
            }

            // Calculate horizontal and vertical bobbing
            float horizontalBob = Mathf.Cos(m_Timer) * bobAmount * 0.5f;
            float verticalBob = Mathf.Sin(m_Timer * 2) * bobAmount;

            // Update the camera's position
            playerCamera.transform.localPosition = new Vector3(
                m_DefaultCameraPosition.x + horizontalBob,
                m_DefaultCameraPosition.y + verticalBob,
                m_DefaultCameraPosition.z
            );
        }
        else
        {
            // Smoothly reduce the timer to zero
            m_Timer = Mathf.MoveTowards(m_Timer, 0f, Time.deltaTime * bobSpeed);

            // Smoothly return the camera to its default position
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                m_DefaultCameraPosition,
                Time.deltaTime * bobSpeed * 2f
            );
        }
    }
}
