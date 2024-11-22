using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public Animator cutsceneAnimator;

    public AudioSource radioAudio;


    public void PlayIntroCutscene()
    {
        cutsceneCamera.enabled = true;
        mainCamera.enabled = false;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        radioAudio.Play();

        yield return BlinkController.Blink(true, 4f);

        yield return Anim(7.5f);

        yield return BlinkController.Blink(false, 4f);

        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
        yield return BlinkController.Blink(true, 4f);
    }

    private IEnumerator Anim(float duration)
    {
        cutsceneAnimator.SetTrigger("StartCutscene");
        yield return new WaitForSeconds(duration);
    }
}
