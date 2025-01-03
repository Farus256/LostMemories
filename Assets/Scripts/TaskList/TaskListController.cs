using UnityEngine;
using DG.Tweening;

public class TaskListController : MonoBehaviour
{
    public KeyCode taskListKey = KeyCode.Tab;
    public Transform notebookObject;
    public Transform cameraTransform;
    public Transform notebookStartPosition;
    public Transform notebookTargetPosition;
    public float cameraDownAngle = 45f;
    public float animationDuration = 1.5f;
    public MonoBehaviour cameraController;

    private bool m_IsTaskListOpen = false;
    private Quaternion m_InitialCameraRotation;
    private Renderer m_NotebookRenderer;

    private void Start()
    {
        m_InitialCameraRotation = cameraTransform.localRotation;
        notebookObject.localPosition = notebookStartPosition.localPosition;

        m_NotebookRenderer = notebookObject.GetComponent<Renderer>();
        m_NotebookRenderer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(taskListKey))
        {
            m_IsTaskListOpen = !m_IsTaskListOpen;
            if (m_IsTaskListOpen)
            {
                ShowTaskList();
            }
            else
            {
                HideTaskList();
            }
        }
    }

    private void ShowTaskList()
    {
        m_NotebookRenderer.enabled = true;
        m_InitialCameraRotation = cameraTransform.localRotation;

        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        cameraTransform.DOLocalRotateQuaternion(Quaternion.Euler(cameraDownAngle, 0f, 0f), animationDuration);

        notebookObject.DOLocalMove(notebookTargetPosition.localPosition, animationDuration);
    }

    private void HideTaskList()
    {
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        cameraTransform.DOLocalRotateQuaternion(Quaternion.Euler(m_InitialCameraRotation.x, 0f, 0f), animationDuration);

        notebookObject.DOLocalMove(notebookStartPosition.localPosition, animationDuration)
            .OnComplete(() => m_NotebookRenderer.enabled = false);
    }
}
