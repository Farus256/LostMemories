using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 2.0f;
    public Camera playerCamera;
    public float bobSpeed = 14f;
    public float bobAmount = 0.05f;
    public AudioClip stepsSound;
    public AudioSource audioSource;

    private float defaultYPos = 0;
    private float timer = 0;
    private float initialVolume = 0.3f;
    private bool isFadingOut = false; // Флаг для контроля процесса затухания звука
    private float stepSoundInterval = 0.5f; // Минимальное время между звуками шагов
    private float lastStepSoundTime = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultYPos = playerCamera.transform.localPosition.y;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = stepsSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        initialVolume = audioSource.volume;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        CameraBob();
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        isFadingOut = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * fadeTime * Time.deltaTime * 5f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = initialVolume;
        isFadingOut = false;
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move;

        if (move.magnitude > 0.005f && !audioSource.isPlaying && !isFadingOut)
        {
            if (Time.time - lastStepSoundTime >= stepSoundInterval)
            {
                audioSource.volume = initialVolume;
                audioSource.Play();
                lastStepSoundTime = Time.time;
            }

        }
        else if (move.magnitude <= 0.005f && audioSource.isPlaying && !isFadingOut)
        {
            StartCoroutine(FadeOut(audioSource, 0.5f));
        }
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        Vector3 currentRotation = playerCamera.transform.localEulerAngles;
        float newRotationX = currentRotation.x - mouseY;

        // Преобразуем угол в 360 градусов в более удобный для расчетов формат (-180 до 180)
        if (newRotationX > 180f) newRotationX -= 360f;

        // Ограничиваем вращение, чтобы камера не "переворачивалась" вверх тормашками
        newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);

        playerCamera.transform.localEulerAngles = new Vector3(newRotationX, currentRotation.y, currentRotation.z);
    }


    void CameraBob()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * bobAmount,
                playerCamera.transform.localPosition.z
            );
        }
        else
        {
            if (timer != 0)
            {
                timer += Time.deltaTime * bobSpeed;
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    Mathf.Lerp(playerCamera.transform.localPosition.y, defaultYPos, Time.deltaTime * bobSpeed),
                    playerCamera.transform.localPosition.z
                );

                if (Mathf.Abs(playerCamera.transform.localPosition.y - defaultYPos) < 0.001f)
                {
                    timer = 0;
                    playerCamera.transform.localPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        defaultYPos,
                        playerCamera.transform.localPosition.z
                    );
                }
            }
        }
    }
}