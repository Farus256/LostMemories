using UnityEngine;

public class ChairController : InteractionController
{
    [Header("Настройки сидения")]
    public Transform sitPosition; // Позиция для сидения на стуле
    public float sitTransitionSpeed = 2f; // Скорость перехода к позиции сидения
    private bool m_IsSitting = false; // Флаг для проверки состояния сидения

    private Vector3 m_PlayerPositionDefault;
    private Quaternion m_PlayerRotationDefault;


    protected override void Update()
    {
        if (m_IsSitting && Input.GetKeyDown(interactKey) && IsPlayerNear())
        {
            StandUp();
        }
        else if (playerTransform && IsPlayerNear() && IsLookingAtObject() && Input.GetKeyDown(interactKey))
        {
            Interaction();
        }
    }

    protected override void Interaction()
    {
        if (!m_IsSitting)
        {
            m_PlayerPositionDefault = playerTransform.position;
            m_PlayerRotationDefault = playerTransform.rotation;
            SitDown();
        }
        else
        {
            StandUp();
        }
    }

    private void SitDown()
    {
        // Запускаем корутину для плавного перемещения к позиции сидения
        StartCoroutine(MoveToPosition(sitPosition.position, sitPosition.rotation));
        m_IsSitting = true;
        m_PlayerController.SetMovementEnabled(false); // Отключаем движение
    }

    private void StandUp()
    {
        // Запускаем корутину для плавного возврата к исходной позиции
        StartCoroutine(MoveToPosition(m_PlayerPositionDefault, m_PlayerRotationDefault));
        m_IsSitting = false;
        m_PlayerController.SetMovementEnabled(true); // Включаем движение
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        float elapsedTime = 0f;

        Vector3 startingPosition = playerTransform.position;


        while (elapsedTime < 1f)
        {
            playerTransform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime * sitTransitionSpeed);


            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = targetPosition;

    }
}
