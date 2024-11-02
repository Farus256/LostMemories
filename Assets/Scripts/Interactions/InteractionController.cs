using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    [Header("Настройки сидения")]
    public KeyCode interactKey = KeyCode.E; // Клавиша взаимодействия
    public float interactDistance = 3f; // Максимальная дистанция для взаимодействия
    public Transform playerTransform; // Трансформ игрока

    protected PlayerController m_PlayerController; // Ссылка на PlayerController для отключения движения

    protected Camera m_PlayerCamera;

    protected virtual void Start()
    {
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
        // Проверяем, находится ли игрок на нужной дистанции от стула
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return distance <= interactDistance;
    }

    protected bool IsLookingAtObject()
    {
        // Создаем Ray из центра экрана и проверяем, что игрок смотрит на стул
        Ray ray = m_PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return hit.transform == transform;
        }
        return false;
    }
}
