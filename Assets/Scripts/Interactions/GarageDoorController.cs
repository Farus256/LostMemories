using UnityEngine;


public class GarageDoorController : InteractionController
{
    public static event System.Action<string> OnDoorInteractionCompleted;
    
    public float moveDistance = 2f; // Дистанция, на которую двигается дверь
    public float duration = 2f; // Длительность анимации
    
    private Animator _animator;
    private Vector3 _targetPosition;
    private Transform _door;
    private bool _isMoving;
    
    public string questDescription = "Откройте гаражную дверь";
    protected override void Start()
    {
        base.Start();

        // Получаем Transform двери
        _door = GetComponent<Transform>();
        if (_door == null)
        {
            Debug.LogError("Door Transform not found!");
            return;
        }
        _animator = GetComponent<Animator>();
        // Устанавливаем целевую позицию
        _targetPosition = _door.position - new Vector3(0, moveDistance, 0);
    }

    protected override void Interaction()
    {
        Debug.Log("Door Interaction");
        // Анимация движения двери
        if (_door != null)
        {
            _animator.SetTrigger("Close");
            NotifyQuestCompletion();
        }
    }
    private void NotifyQuestCompletion()
    {
        OnDoorInteractionCompleted?.Invoke(questDescription); // Вызываем событие
    }
}