using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroCutScene : MonoBehaviour
{
    public static IntroCutScene Instance { get; private set; }

    public Camera cutsceneCamera;
    public Camera mainCamera;

    public Animator cutsceneAnimator;

    public Bootstrapper bootstrapper;

    public AudioSource radioAudio;

    public BlinkController blinkController;

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

    public void PlayIntroCutscene()
    {
        cutsceneCamera.enabled = true;
        mainCamera.enabled = false;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        //radioAudio.Play();
            
        blinkController.StartBlink(true, 4f);
        yield return new WaitForSeconds(4f);
            
        yield return Anim(5.5f);

        blinkController.StartBlink(false, 4f);
        yield return new WaitForSeconds(4f);

        bootstrapper.StartGame();
        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
        blinkController.StartBlink(true, 4f);
        yield return new WaitForSeconds(4f);
    }

    private IEnumerator Anim(float duration)
    {
        cutsceneAnimator.SetTrigger("StartCutscene");
        yield return new WaitForSeconds(duration);
    }
}