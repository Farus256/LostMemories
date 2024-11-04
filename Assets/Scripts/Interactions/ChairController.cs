using UnityEngine;
using DG.Tweening;

public class ChairController : InteractionController
{
    [Header("Настройки сидения")]
    public Transform sitPosition;
    public float sitTransitionDuration = 0.5f;
    private bool m_IsSitting = false;

    private Vector3 m_PlayerPositionDefault;
    //private Quaternion m_PlayerRotationDefault;

    protected override void Update()
    {
        if (m_IsSitting && Input.GetKeyDown(interactKey) && IsPlayerNear())
        {
            StandUp();
        }
        else if (playerTransform && IsPlayerNear() && IsLookingAtObject() && Input.GetKeyDown(interactKey))
        {
            Interaction();
        }
    }

    protected override void Interaction()
    {
        if (!m_IsSitting)
        {
            m_PlayerPositionDefault = playerTransform.position;
            //m_PlayerRotationDefault = playerTransform.rotation;
            SitDown();
        }
        else
        {
            StandUp();
        }
    }

    private void SitDown()
    {
        //playerTransform.DOKill();

        playerTransform.DOMove(sitPosition.position, sitTransitionDuration).OnComplete(() =>
        {
            m_IsSitting = true;
            m_PlayerController.SetMovementEnabled(false);
        });
    }

    private void StandUp()
    {
        playerTransform.DOKill();

        playerTransform.DOMove(m_PlayerPositionDefault, sitTransitionDuration).OnComplete(() =>
        {
            m_IsSitting = false;
            m_PlayerController.SetMovementEnabled(true);
        });
    }
}
