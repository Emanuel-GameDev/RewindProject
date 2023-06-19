using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Tooltip("New value for orthografic size in the Lens menù inside Cinemachine")]
    [SerializeField] private float zoomAmount;

    [Tooltip("New value for offset in the Body menù inside Cinemachine")]
    [SerializeField] private Vector3 followOffset;

    [Tooltip("New value for damping in the Body menù inside Cinemachine")]
    [SerializeField] private Vector2 damping;

    CameraData cameraData;
    CameraData prevCameraData;
    private bool triggered = false;

    private void Start()
    {
        cameraData = new CameraData(zoomAmount, followOffset, damping);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check player collision
        if (collision.gameObject.GetComponent<Character>() != null && !triggered)
        {
            if (cameraData == null) return;

            SaveCameraData();
            PubSub.Instance.Notify(EMessageType.CameraSwitch, cameraData);
            triggered = true;
        }
        else if (collision.gameObject.GetComponent<Character>() != null && triggered)
        {
            if (prevCameraData == null) return;

            PubSub.Instance.Notify(EMessageType.CameraSwitch, prevCameraData);

            triggered = false;
        }
    }

    // Used to save camera data on first trigger
    private void SaveCameraData()
    {
        if (prevCameraData != null) return;

        CinemachineVirtualCamera mainCam = GameManager.Instance.cameraManager.mainCam;
        float prevZoom = mainCam.m_Lens.OrthographicSize;

        CinemachineTransposer transposer = mainCam.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 prevFollowOffset = transposer.m_FollowOffset;
        Vector3 prevDamping = new Vector3();
        prevDamping.x = transposer.m_XDamping;
        prevDamping.y = transposer.m_YDamping;

        prevCameraData = new CameraData(prevZoom, prevFollowOffset, prevDamping);
    }
}
