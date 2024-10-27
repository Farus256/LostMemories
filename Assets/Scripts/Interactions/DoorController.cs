using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Настройки двери")]
    public Animator doorAnimator; // Ссылка на компонент Animator двери
    public KeyCode interactKey = KeyCode.E; // Клавиша взаимодействия
    public float interactDistance = 3f; // Максимальная дистанция взаимодействия
    public bool isPlayerNear = false; // Флаг, находится ли игрок рядом
    public Transform playerTransform; // Ссылка на трансформ игрока


    private bool doorOpen = false;

    private void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
            if (doorAnimator == null)
            {
                Debug.LogError("DoorController: Отсутствует компонент Animator на двери.");
            }
        }
    }

    private void Update()
    {
        // Проверяем дистанцию до игрока
        if (playerTransform)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= interactDistance)
            {
                isPlayerNear = true;

                if (Input.GetKeyDown(interactKey))
                {
                    if(!doorOpen){
                        OpenDoor(true);
                        doorOpen = true;
                    }
                    else { OpenDoor(false); doorOpen = false; }
                }
            }
            else
            {
                isPlayerNear = false;
            }
        }
    }

    private void OpenDoor(bool state)
    {
        if (doorAnimator)
        {
            doorAnimator.SetBool("isOpen", state);
        }
    }
}
