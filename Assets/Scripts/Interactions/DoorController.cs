using UnityEngine;

public class DoorController : InteractionController
{
    [Header("Настройки двери")]
    public Animator doorAnimator; // Ссылка на компонент Animator двери

    private bool m_DoorOpen = false;
    
    protected override void Interaction()
    {
        if(!m_DoorOpen){
            OpenDoor(true);
            m_DoorOpen = true;
        }
        else { OpenDoor(false); m_DoorOpen = false; }
    }

    private void OpenDoor(bool state)
    {
        if (doorAnimator)
        {
            doorAnimator.SetBool("isOpen", state);
        }
    }
}
