using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform doorTransform; // Трансформ двери
    public float openAngle = 90.0f; // Угол открытия двери
    public float openSpeed = 5.0f; // Скорость открытия двери
    public float interactDistance = 5.0f; // Максимальное расстояние, на котором игрок может взаимодействовать с дверью

    private bool isOpen = false;
    private float initialAngle;
    private bool openTowardsPlayer = true; // Открытие в сторону игрока

    private void Start()
    {
        if (doorTransform == null)
            doorTransform = GetComponent(typeof(Transform)) as Transform;
        initialAngle = doorTransform.eulerAngles.y;
    }

    private void Update()
    {
        // Проверяем, смотрит ли игрок на дверь
        if (IsLookingAtDoor() && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Переключить состояние открытия/закрытия
            openTowardsPlayer = ShouldOpenTowardsPlayer(); // Определить направление открытия двери
        }

        // Плавное открытие или закрытие двери
        if (isOpen)
        {
            float targetAngle = initialAngle + (openTowardsPlayer ? openAngle : -openAngle);
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * openSpeed);
        }
        else
        {
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, Quaternion.Euler(0, initialAngle, 0), Time.deltaTime * openSpeed);
        }
    }

    // Проверка, смотрит ли игрок на дверь
    private bool IsLookingAtDoor()
    {
        RaycastHit hit;
        Vector3 forward = Camera.main.transform.forward;
        Vector3 origin = Camera.main.transform.position;

        if (Physics.Raycast(origin, forward, out hit, interactDistance))
        {
            if (hit.transform == doorTransform) // Проверяем, что объект, на который мы смотрим, это дверь
            {
                Debug.Log("Player is looking at the door");
                return true;
            }
        }
        return false;
    }

    // Определение, должна ли дверь открываться на игрока
    private bool ShouldOpenTowardsPlayer()
    {
        Vector3 doorToPlayer = Camera.main.transform.position - doorTransform.position;
        float angle = Vector3.SignedAngle(doorTransform.forward, doorToPlayer, Vector3.up);
        return angle < 0; // Если угол меньше 0, открываем в одну сторону, иначе в другую
    }
}
