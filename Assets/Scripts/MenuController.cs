using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuCamera.enabled = true;
        cutsceneCamera.enabled = false;

        m_FlashlightAnimator = flashlight.GetComponent<Animator>();
        m_RadioAnimator = radio.GetComponent<Animator>();
        m_FlashlightController = flashlight.GetComponent<FlashlightController>();

        StartCoroutine(WakeUp(true));
    }

    private IEnumerator WakeUp(bool state)
    {
        yield return BlinkController.Blink(state, 3f);
    }

    private void Update()
    {
        Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            m_CurrentHitName = hit.collider.gameObject.name;
            if (hit.collider.CompareTag("MenuObj"))
            {
                Debug.Log($"Наведен на объект: {hit.collider.gameObject.name}");
                if (hit.collider.gameObject.name == "Radio")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Work in progress");
                    }
                }
                else if (hit.collider.gameObject.name == "Flashlight")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        m_FlashlightController.ToggleFlashLightOnce();
                        m_FlashlightAnimator.SetBool("isHover", false);
                        StartCoroutine(TransitionToGame());
                    }
                }
            }
        }

        switch (m_CurrentHitName)
        {
            case "Radio":
                m_RadioAnimator.SetBool("isHover", true);
                break;
            case "Flashlight":
                m_FlashlightAnimator.SetBool("isHover", true);
                break;
            default:
                m_FlashlightAnimator.SetBool("isHover", false);
                m_RadioAnimator.SetBool("isHover", false);
                break;
        }


    }
    private IEnumerator TransitionToGame()
    {
        yield return StartCoroutine(WakeUp(false));

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_FlashlightAnimator.enabled = false;
        m_RadioAnimator.enabled = false;

        bootstrapper.StartCutscene();
        menuCanvas.enabled = false;
        menuCamera.enabled = false;
        cutsceneCamera.enabled = true;
        m_FlashlightController.ToggleFlashLightOnce();
    }
}
