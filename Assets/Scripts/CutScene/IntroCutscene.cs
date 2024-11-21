using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public Animator cutsceneAnimator;  // Animator для камеры
    public Image topBlackImage;
    public Image bottomBlackImage;
    public AudioSource radioAudio;

    void Start()
    {
        cutsceneCamera.enabled = true;
        mainCamera.enabled = false;

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // 1. Моргание (глаза открываются)
        yield return StartCoroutine(Blink(0f, 1000f, 4f));

        // 2. Анимация подъема и поворота камеры
        cutsceneAnimator.SetTrigger("StartCutscene");

        // Ждем окончания анимации
        yield return new WaitForSeconds(2f); // Длительность анимации подъема и поворота

        // 3. Проигрывание звука
        radioAudio.Play();
        yield return new WaitForSeconds(radioAudio.clip.length);

        // 4. Моргание (глаза закрываются)
        yield return StartCoroutine(Blink(300f, 0f, 0.5f));

        // 5. Переключение на основную камеру
        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
    }

    private IEnumerator Blink(float startOffset, float endOffset, float duration)
    {
        // Анимация верхнего изображения: двигаем вниз
        topBlackImage.rectTransform.DOAnchorPosY(endOffset, duration)
            .SetEase(Ease.InOutQuad);

        // Анимация нижнего изображения: двигаем вверх
        bottomBlackImage.rectTransform.DOAnchorPosY(-endOffset, duration)
            .SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(duration);
    }
}
