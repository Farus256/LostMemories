using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    public Camera menuCamera;
    public Camera cutsceneCamera;
    public Bootstrapper bootstrapper;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuCamera.enabled = true;
        cutsceneCamera.enabled = false;
        StartCoroutine(WakeUp(true));
    }

    private IEnumerator WakeUp(bool state)
    {
        yield return BlinkController.Blink(state, 1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("MenuObj"))
            {

                if (hit.collider.gameObject.name == "Radio")
                {
                    Debug.Log("Work in progress");
                }
                else if (hit.collider.gameObject.name == "Flashlight")
                {
                    StartCoroutine(TransitionToGame());
                }
            }
        }
    }
    private IEnumerator TransitionToGame()
    {
        // Закрытие глаз
        yield return StartCoroutine(WakeUp(false));

        // Скрыть курсор и переключить камеру
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        bootstrapper.StartGame();
        menuCamera.enabled = false;
        cutsceneCamera.enabled = true;
    }
}
