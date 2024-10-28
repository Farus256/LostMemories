using UnityEngine;
using TMPro;

public class BasicInteraction : MonoBehaviour
{
    public TMP_Text interactionText; // Текстовый элемент UI для отображения сообщений
    public string defaultText = "Нажмите E для взаимодействия"; // Текст по умолчанию
    public string targetTag = "Interactive"; // Тег для объектов, с которыми можно взаимодействовать
    public MonoBehaviour targetScript; // Скрипт, который будет активирован при взаимодействии
    public float maxDistance = 1.5f; // Максимальная дистанция для взаимодействия
    private Transform player; // Трансформ игрока

    private void Start()
    {
        // Найдите объект игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Скрываем текст при запуске
        interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Выполняем проверку на наличие объекта впереди
        if (Physics.Raycast(ray, out hit))
        {
            // Проверяем, что Raycast попадает на этот объект и что расстояние до игрока меньше максимальной дистанции
            if (hit.transform == transform && hit.distance <= maxDistance)
            {
                // Отображаем текст при наведении
                interactionText.text = defaultText;
                interactionText.gameObject.SetActive(true);

                // Проверка нажатия клавиши E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (targetScript != null)
                    {
                        // Активируем другой скрипт
                        targetScript.enabled = true;
                    }
                }
            }
            else
            {
                // Скрываем текст если не наведено на объект или объект слишком далеко
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Скрываем текст если не наведено на объект
            interactionText.gameObject.SetActive(false);
        }
    }
}
