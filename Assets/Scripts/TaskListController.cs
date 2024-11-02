using UnityEngine;
using DG.Tweening;

public class TaskListController : MonoBehaviour
{
    public KeyCode taskListKey = KeyCode.Tab; // Клавиша для открытия/закрытия блокнота
    public Transform notebookObject; // Ссылка на объект блокнота
    public Transform cameraTransform; // Трансформ камеры игрока
    public Transform notebookStartPosition; // Начальная позиция блокнота (вне поля зрения)
    public Transform notebookTargetPosition; // Целевая позиция блокнота (в поле зрения)
    public float cameraDownAngle = 45f; // Угол, на который опускается камера
    public float animationDuration = 1.5f; // Длительность анимации
    public MonoBehaviour cameraController; // Ссылка на скрипт управления камерой

    private bool isTaskListOpen = false;
    private Quaternion initialCameraRotation; // Исходное положение камеры
    private Renderer notebookRenderer; // Рендерер блокнота

    private void Start()
    {
        initialCameraRotation = cameraTransform.localRotation;
        notebookObject.localPosition = notebookStartPosition.localPosition;

        // Получаем Renderer блокнота и отключаем рендер
        notebookRenderer = notebookObject.GetComponent<Renderer>();
        notebookRenderer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(taskListKey))
        {
            isTaskListOpen = !isTaskListOpen;
            if (isTaskListOpen)
            {
                ShowTaskList();
            }
            else
            {
                HideTaskList();
            }
        }
    }

    private void ShowTaskList()
    {
        // Включаем рендер перед началом анимации
        notebookRenderer.enabled = true;
        initialCameraRotation = cameraTransform.localRotation;

        // Отключаем управление камерой
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // Опускаем камеру с анимацией
        cameraTransform.DOLocalRotateQuaternion(Quaternion.Euler(cameraDownAngle, 0f, 0f), animationDuration);

        // Анимация перемещения блокнота к целевой позиции
        notebookObject.DOLocalMove(notebookTargetPosition.localPosition, animationDuration);
    }

    private void HideTaskList()
    {
        // Включаем управление камерой
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // Поднимаем камеру в исходное положение с анимацией
        cameraTransform.DOLocalRotateQuaternion(Quaternion.Euler(initialCameraRotation.x, 0f, 0f), animationDuration);


        // Анимация перемещения блокнота в начальную позицию с отключением рендера по завершении
        notebookObject.DOLocalMove(notebookStartPosition.localPosition, animationDuration)
            .OnComplete(() => notebookRenderer.enabled = false); // Отключаем рендер после завершения анимации
    }

    private void Action()
    {
        throw new System.NotImplementedException();
    }
}
