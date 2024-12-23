using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bootstrapper : MonoBehaviour
{
    public MenuController menuController;
    [FormerlySerializedAs("introCutScene")] [FormerlySerializedAs("introCutscene")] public CutScene cutScene;

    public PlayerController player;
    
    public bool skipCutscene;

    private void Awake()
    {
        BlinkController.InitializeBlink();
        EnableMenu();
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
        player.enabled = true;
    }
}
