using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float bobSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;

    [Header("Настройки звука")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepsSound;
    [SerializeField] private float stepInterval = 0.5f;

    private Vector3 _velocity = Vector3.zero;

    private float _defaultYPos = 0;
    private float _timer = 0;
    private float _stepTimer;
    private float _verticalRotation = 0;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _defaultYPos = playerCamera.transform.localPosition.y;
        AudioSourceInitial();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;

        if (moveDirection.magnitude >= 0.1f)
        {
            moveDirection.Normalize();
        }

        Vector3 move = moveDirection * speed ;

        characterController.Move(move* Time.deltaTime);

        if (characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -3f;
        }
        _velocity.y += gravity * Time.deltaTime;
        characterController.Move(_velocity * Time.deltaTime);

        PlayWalkSound(move);
        CameraBob();
    }

    void AudioSourceInitial()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = stepsSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void PlayWalkSound(Vector3 move)
    {
        if ( move.magnitude > 0.1f)
        {
            _stepTimer += Time.deltaTime;
            if (_stepTimer >= stepInterval)
            {
                audioSource.PlayOneShot(stepsSound, audioSource.volume);
                _stepTimer = 0f;
            }
        }
        else
        {
            _stepTimer = 0f;
        }
    }
    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);

        playerCamera.transform.localEulerAngles = new Vector3(_verticalRotation, 0, 0);
    }


    void CameraBob()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            _timer += Time.deltaTime * bobSpeed;
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * bobAmount,
                playerCamera.transform.localPosition.z
            );
        }
        else
        {
            if (_timer != 0)
            {
                _timer += Time.deltaTime * bobSpeed;
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    Mathf.Lerp(playerCamera.transform.localPosition.y, _defaultYPos, Time.deltaTime * bobSpeed),
                    playerCamera.transform.localPosition.z
                );

                if (Mathf.Abs(playerCamera.transform.localPosition.y - _defaultYPos) < 0.001f)
                {
                    _timer = 0;
                    playerCamera.transform.localPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        _defaultYPos,
                        playerCamera.transform.localPosition.z
                    );
                }
            }
        }
    }
}