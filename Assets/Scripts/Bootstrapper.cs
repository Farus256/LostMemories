using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bootstrapper : MonoBehaviour
{
    public MenuController menuController;
    public IntroCutScene introCutScene;

    public PlayerController player;
    public BlinkController blinkController;
    
    public bool skipCutscene;
   
    public Camera cutsceneCamera;
    public Camera menuCamera;
    public Camera mainCamera;

    private void Awake()
    {
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
        else
            StartGame();
    }

    private void EnableMenu()
    {
        menuController.enabled = true;
        introCutScene.enabled = false;
        player.enabled = false;
        
        blinkController.StartBlink(true, 3f);
    }

    public void StartCutscene()
    {
        introCutScene.enabled = true;
        introCutScene.PlayIntroCutscene();
    }

    public void StartGame()
    {
        menuController.enabled = false;
        introCutScene.enabled = false;
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