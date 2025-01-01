using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera mainCamera;

    public Animator cutsceneAnimator;

    public Bootstrapper bootstrapper;

    public AudioSource radioAudio;
    
    public BlinkController blinkController;
    
    public void PlayIntroCutscene(bool skipCutscene)
    {
        cutsceneCamera.enabled = true;
        mainCamera.enabled = false;
        StartCoroutine(PlayCutscene(skipCutscene));
    }

    private IEnumerator PlayCutscene(bool skipCutscene)
    {
        if (!skipCutscene)
        {
            //radioAudio.Play();

            blinkController.StartBlink(true, 4f);

            yield return Anim(7.5f);

            blinkController.StartBlink(false, 4f);
        }
        
        bootstrapper.StartGame();
        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
        blinkController.StartBlink(true, 4f);
    }

    private IEnumerator Anim(float duration)
    {
        cutsceneAnimator.SetTrigger("StartCutscene");
        yield return new WaitForSeconds(duration);
    }
}
