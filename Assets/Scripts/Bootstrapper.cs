using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    public MenuController menuController;
    public IntroCutscene introCutscene;

    public PlayerController player;

    private void Awake()
    {
        BlinkController.InitializeBlink();
        menuController.enabled = true;
        introCutscene.enabled = false;
        player.enabled = false;
    }

    private void Start()
    {

    }

    public void StartGame()
    {
        player.enabled = true;
        introCutscene.enabled = true;
        introCutscene.PlayIntroCutscene();
    }
}
