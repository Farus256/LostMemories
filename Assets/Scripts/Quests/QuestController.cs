using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance;

    private string currentQuest = "Откройте гаражную дверь"; // Текущее задание

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GarageDoorController.OnDoorInteractionCompleted += CompleteQuest; // Подписка на событие
    }

    private void OnDisable()
    {
        GarageDoorController.OnDoorInteractionCompleted -= CompleteQuest; // Отписка от события
    }

    private void Start()
    {
        DisplayQuest();
    }

    public void DisplayQuest()
    {
        Debug.Log($"Текущее задание: {currentQuest}");
    }

    public void CompleteQuest(string completedQuest)
    {
        if (currentQuest == completedQuest)
        {
            Debug.Log($"Квест завершен: {completedQuest}");
            GiveNewQuest();
        }
    }

    private void GiveNewQuest()
    {
        currentQuest = "Исследуйте дом";
        DisplayQuest();
    }
}