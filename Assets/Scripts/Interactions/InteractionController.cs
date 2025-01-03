using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E; // Клавиша взаимодействия
    public float interactDistance = 3f; // Максимальная дистанция для взаимодействия
    public Transform playerTransform; // Трансформ игрока

    protected PlayerController m_PlayerController; // Ссылка на PlayerController для отключения движения
    protected Camera m_PlayerCamera;

    private RectTransform crosshair;
    protected virtual void Start()
    {
        GameObject crosshairObject = GameObject.Find("Crosshair");
        if (crosshairObject != null)
        {
            crosshair = crosshairObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("Crosshair not found in the scene. Please add it to the Canvas.");
        }
        m_PlayerCamera = Camera.main;
        m_PlayerController = playerTransform.GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
        if (playerTransform && IsPlayerNear() && IsLookingAtObject() && Input.GetKeyDown(interactKey))
        {
            Interaction();
            Debug.Log("Interaction");
        }
    }
    protected abstract void Interaction();

    protected bool IsPlayerNear()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return distance <= interactDistance;
    }

    protected bool IsLookingAtObject()
    {
        Vector3 screenPoint = crosshair.position;
        Ray ray = m_PlayerCamera.ScreenPointToRay(screenPoint);

        float sphereRadius = 0.1f; // Радиус сферы (можно увеличить для более точного подбора)
        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactDistance))
        {
            return hit.transform == transform;
        }
        return false;
    }

}
