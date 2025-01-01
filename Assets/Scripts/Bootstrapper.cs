using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bootstrapper : MonoBehaviour
{
    public MenuController menuController;
    [FormerlySerializedAs("introCutScene")] [FormerlySerializedAs("introCutscene")] public CutScene cutScene;

    public PlayerController player;
    public BlinkController blinkController; // Ссылка на BlinkController
    
    public bool skipCutscene;
    public Camera cutsceneCamera;
    public Camera menuCamera;
    public Camera mainCamera;

    private void Awake()
    {
        // Инициализируем BlinkController
        if (blinkController != null)
        {
            blinkController.InitializeBlink();
        }
        else
        {
            Debug.LogError("BlinkController is not assigned in the inspector!");
        }

        if (!skipCutscene)
            EnableMenu();
        
        StartGame();
    }

    private void EnableMenu()
    {
        menuController.enabled = true;
        cutScene.enabled = false;
        player.enabled = false;
    }

    public void StartCutscene()
    {
        cutScene.enabled = true;
        cutScene.PlayIntroCutscene(skipCutscene);
    }

    public void StartGame()
    {
        menuController.enabled = false;
        cutScene.enabled = false;
        cutsceneCamera.enabled = false;
        menuCamera.enabled = false;
        mainCamera.enabled = true;
        player.enabled = true;
        
        if (blinkController != null)
        {
            blinkController.StartBlink(true, 3f);
        }
        else
        {
            Debug.LogError("BlinkController is not assigned!");
        }
    }
}