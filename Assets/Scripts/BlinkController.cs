using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlinkController : MonoBehaviour
{
    public static BlinkController Instance { get; private set; } // Singleton

    private Image _topBlackImage;
    private Image _bottomBlackImage;
    private Vector2 _topImageStartPos;
    private Vector2 _bottomImageStartPos;

    private void Awake()
    {
        // Настройка Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
    }

    public void InitializeBlink()
    {
        _topBlackImage = GameObject.Find("TopBlackImage").GetComponent<Image>();
        _bottomBlackImage = GameObject.Find("BottomBlackImage").GetComponent<Image>();

        _topImageStartPos = _topBlackImage.rectTransform.anchoredPosition;
        _bottomImageStartPos = _bottomBlackImage.rectTransform.anchoredPosition;
    }

    private IEnumerator Blink(bool openEyes, float duration)
    {
        if (_topBlackImage == null || _bottomBlackImage == null)
        {
            Debug.LogError("BlinkController is not initialized. Call InitializeBlink first.");
            yield break;
        }

        if (openEyes)
        {
            _topBlackImage.rectTransform.DOAnchorPosY(_topImageStartPos.y + 600f, duration).SetEase(Ease.InOutQuad);
            _bottomBlackImage.rectTransform.DOAnchorPosY(_bottomImageStartPos.y - 600f, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            _topBlackImage.rectTransform.DOAnchorPosY(_topImageStartPos.y, duration).SetEase(Ease.InOutQuad);
            _bottomBlackImage.rectTransform.DOAnchorPosY(_bottomImageStartPos.y, duration).SetEase(Ease.InOutQuad);
        }

        yield return new WaitForSeconds(duration);
    }

    public void StartBlink(bool openEyes, float duration)
    {
        StartCoroutine(Blink(openEyes, duration));
    }
}
