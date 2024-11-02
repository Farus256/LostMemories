using UnityEngine;

public class DoorController : InteractionController
{
    [Header("Настройки двери")]
    public Transform hingePoint; // Точка вращения (петли двери)
    public float rotationAngle = 90f; // Угол, на который дверь должна открываться
    public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду

    private bool m_DoorOpen = false;
    private float m_CurrentRotation = 0f;

    protected override void Interaction()
    {
        Debug.Log("Interaction called");
        if (!m_DoorOpen)
        {
            OpenDoor(true);
            m_DoorOpen = true;
        }
        else
        {
            OpenDoor(false);
            m_DoorOpen = false;
        }
    }

    private void OpenDoor(bool state)
    {
        StartCoroutine(state ? RotateDoor(rotationAngle) : RotateDoor(0f));
    }

    private System.Collections.IEnumerator RotateDoor(float targetAngle)
    {
        while (Mathf.Abs(m_CurrentRotation - targetAngle) > 0.1f)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            // Определяем направление вращения
            if (m_CurrentRotation > targetAngle)
            {
                rotationThisFrame = -rotationThisFrame;
            }

            // Убедимся, что не вращаем больше, чем нужно
            if (Mathf.Abs(rotationThisFrame) > Mathf.Abs(m_CurrentRotation - targetAngle))
            {
                rotationThisFrame = targetAngle - m_CurrentRotation;
            }

            // Вращаем дверь
            transform.RotateAround(hingePoint.position, Vector3.up, rotationThisFrame);

            // Обновляем текущий угол
            m_CurrentRotation += rotationThisFrame;

            yield return null;
        }

        // Корректируем положение двери (эффект захлопывания двери)
        float finalRotation = targetAngle - m_CurrentRotation;
        transform.RotateAround(hingePoint.position, Vector3.up, finalRotation);
        m_CurrentRotation = targetAngle;
    }
}
