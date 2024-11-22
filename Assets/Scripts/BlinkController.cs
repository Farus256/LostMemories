using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class BlinkController : MonoBehaviour
{
    private static Image _topBlackImage;
    private static Image _bottomBlackImage;
    private static Vector2 _topImageStartPos;
    private static Vector2 _bottomImageStartPos;


    public static void InitializeBlink()
    {
        _topBlackImage = GameObject.Find("TopBlackImage").GetComponent<Image>();
        _bottomBlackImage = GameObject.Find("BottomBlackImage").GetComponent<Image>();

        _topImageStartPos = _topBlackImage.rectTransform.anchoredPosition;
        _bottomImageStartPos = _bottomBlackImage.rectTransform.anchoredPosition;
    }
    public static IEnumerator Blink(bool openEyes, float duration)
    {
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
}
