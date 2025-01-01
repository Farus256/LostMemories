using System.Collections;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class MenuController : MonoBehaviour
{
    public Camera menuCamera;
    public Camera cutsceneCamera;
    public Bootstrapper bootstrapper;

    public GameObject flashlight;
    public GameObject radio;

    public Canvas menuCanvas;

    private Animator m_FlashlightAnimator;
    private Animator m_RadioAnimator;

    private FlashlightController m_FlashlightController;

    private string m_CurrentHitName;

    private void Start()
    {
        // Настройка курсора
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Включаем камеру меню
        menuCamera.enabled = true;
        cutsceneCamera.enabled = false;

        // Инициализация компонентов
        if (flashlight != null)
        {
            m_FlashlightAnimator = flashlight.GetComponent<Animator>();
            m_FlashlightController = flashlight.GetComponent<FlashlightController>();
        }

        if (radio != null)
        {
            m_RadioAnimator = radio.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        HandleRaycast();
        HandleHoverAnimations();
    }

    private void HandleRaycast()
    {
        // Проверяем, куда направлен луч из камеры меню
        Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            m_CurrentHitName = hit.collider.gameObject.name;

            if (hit.collider.CompareTag("MenuObj"))
            {
                Debug.Log($"Наведен на объект: {hit.collider.gameObject.name}");

                if (hit.collider.gameObject.name == "Radio" && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Work in progress");
                }
                else if (hit.collider.gameObject.name == "Flashlight" && Input.GetMouseButtonDown(0))
                {
                    if (m_FlashlightController != null)
                    {
                        m_FlashlightController.ToggleFlashLightOnce();
                    }
                    StartCoroutine(TransitionToGame());
                }
            }
        }
        else
        {
            m_CurrentHitName = null; // Сброс имени объекта, если ничего не выбрано
        }
    }

    private void HandleHoverAnimations()
    {
        // Управляем анимациями наведения
        switch (m_CurrentHitName)
        {
            case "Radio":
                if (m_RadioAnimator != null)
                    m_RadioAnimator.SetBool("isHover", true);
                break;

            case "Flashlight":
                if (m_FlashlightAnimator != null)
                    m_FlashlightAnimator.SetBool("isHover", true);
                break;

            default:
                if (m_FlashlightAnimator != null)
                    m_FlashlightAnimator.SetBool("isHover", false);

                if (m_RadioAnimator != null)
                    m_RadioAnimator.SetBool("isHover", false);
                break;
        }
    }

    private IEnumerator TransitionToGame()
    {
        // Анимация перехода в игру
        if (bootstrapper != null)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (m_FlashlightAnimator != null)
                m_FlashlightAnimator.enabled = false;

            if (m_RadioAnimator != null)
                m_RadioAnimator.enabled = false;

            bootstrapper.StartCutscene();

            // Отключаем элементы меню
            if (menuCanvas != null)
                menuCanvas.enabled = false;

            menuCamera.enabled = false;
            cutsceneCamera.enabled = true;

            if (m_FlashlightController != null)
                m_FlashlightController.ToggleFlashLightOnce();
        }
        else
        {
            Debug.LogError("Bootstrapper is not assigned!");
        }

        yield return null; // Задержка для завершения кадра
    }
}
