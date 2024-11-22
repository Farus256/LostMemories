using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public Animator cutsceneAnimator;
    public Image topBlackImage;
    public Image bottomBlackImage;
    public AudioSource radioAudio;

    private Vector2 topImageStartPos;
    private Vector2 bottomImageStartPos;

    void Start()
    {
        // Сохраняем начальные позиции для восстановления после моргания
        topImageStartPos = topBlackImage.rectTransform.anchoredPosition;
        bottomImageStartPos = bottomBlackImage.rectTransform.anchoredPosition;

        cutsceneCamera.enabled = true;
        mainCamera.enabled = false;

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        radioAudio.Play();

        yield return Blink(true, 4f);

        yield return Anim(7.5f);

        yield return Blink(false, 4f);

        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
        yield return Blink(true, 4f);
    }

    private IEnumerator Blink(bool openEyes, float duration)
    {
        if (openEyes)
        {
            topBlackImage.rectTransform.DOAnchorPosY(topImageStartPos.y + 400f, duration).SetEase(Ease.InOutQuad);
            bottomBlackImage.rectTransform.DOAnchorPosY(bottomImageStartPos.y - 400f, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            topBlackImage.rectTransform.DOAnchorPosY(topImageStartPos.y, duration).SetEase(Ease.InOutQuad);
            bottomBlackImage.rectTransform.DOAnchorPosY(bottomImageStartPos.y, duration).SetEase(Ease.InOutQuad);
        }

        yield return new WaitForSeconds(duration);
    }

    private IEnumerator Anim(float duration)
    {
        cutsceneAnimator.SetTrigger("StartCutscene");
        yield return new WaitForSeconds(duration);
    }
}
