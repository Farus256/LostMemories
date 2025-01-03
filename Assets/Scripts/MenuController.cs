using System.Collections;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    public Camera menuCamera;
    public Camera cutsceneCamera;
    public Bootstrapper bootstrapper;

    public GameObject flashlight;
    public GameObject radio;

    public Canvas menuCanvas;
    
    public BlinkController blinkController;

    private Animator m_FlashlightAnimator;
    private Animator m_RadioAnimator;

    private FlashlightController m_FlashlightController;

    private string m_CurrentHitName;

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

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        menuCamera.enabled = true;
        cutsceneCamera.enabled = false;

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
            m_CurrentHitName = null;
        }
    }

    private void HandleHoverAnimations()
    {
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
        blinkController.StartBlink(false, 4f);
        yield return new WaitForSeconds(4f);
        if (bootstrapper != null)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (m_FlashlightAnimator != null)
                m_FlashlightAnimator.enabled = false;

            if (m_RadioAnimator != null)
                m_RadioAnimator.enabled = false;

            bootstrapper.StartCutscene();

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
        blinkController.StartBlink(true, 4f);
        yield return new WaitForSeconds(4f);
        yield return null;
    }
}
