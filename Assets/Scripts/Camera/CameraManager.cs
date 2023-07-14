using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineVirtualCamera mainCam;

    [Tooltip("Start value for orthografic size in the Lens menù inside Cinemachine")]
    public float StartZoomAmount = 7f;

    [Tooltip("Speed of the transition\n " +
        "N.B. the action is an interpolation so it scales over time")]
    public float transitionSpeed;

    private float currentZoom;
    private Vector3 currentOffset;
    private Vector2 currentDamping;
    private IEnumerator cinemachineCoroutine;

    [Header("PostProcessing")]
    [SerializeField] Volume rewindVolume;
    [SerializeField] float rewindDeformSpeed;
    [SerializeField] float targetLDIntensity;
    [SerializeField] float targetCASaturation;

    private IEnumerator deformCoroutine;
    private IEnumerator restoreCoroutine;
    private ChromaticAberration chromaticAb;
    private float startAb;
    private float currentAb;
    private LensDistortion lensDist;
    private float startLD;
    private float currentLD;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, UpdateRewindPostProcess);

        PubSub.Instance.RegisterFunction(EMessageType.CameraSwitch, UpdateCamera);
        mainCam.gameObject.SetActive(true);

        Initilize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateRewindPostProcess(null);
        }
    }

    private void Initilize()
    {
        #region Cinemachine
        mainCam.m_Lens.OrthographicSize = StartZoomAmount;
        currentZoom = StartZoomAmount;

        currentOffset = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        currentDamping.x = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        currentDamping.y = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping;
        #endregion

        #region PostProcessing

        if (rewindVolume != null)
        {
            // Chromatic Aberration
            if (!rewindVolume.profile.TryGet(out chromaticAb))
                Debug.LogError("Error TryGet Chromatic Aberration");

            // Lens Distorsion
            if (!rewindVolume.profile.TryGet(out lensDist))
                Debug.LogError("Error TryGet Lens Distortion");
        }
        else
            Debug.LogError("Error need to Assign rewindProfile");

        #endregion
    }

    #region PostProcessing related

    private void UpdateRewindPostProcess(object obj)
    {
        if (deformCoroutine != null)
        {
            StopCoroutine(deformCoroutine);
            deformCoroutine = null;

            if (restoreCoroutine == null)
            {
                restoreCoroutine = RestoreCamera();
                StartCoroutine(restoreCoroutine);
            }
        }
        else
        {
            if (restoreCoroutine != null)
            {
                StopCoroutine(restoreCoroutine);
                restoreCoroutine = null;
            }

            deformCoroutine = DeformCamera();
            StartCoroutine(deformCoroutine);
        }
    }

    private float SetupStartSetting(float start, float current)
    {
        if (start != current)
            start = current;

        return start;
    }

    private IEnumerator DeformCamera()
    {
        currentAb = chromaticAb.intensity.value;
        float maxAbIntensity = chromaticAb.intensity.max;
        startAb = SetupStartSetting(startAb, currentAb);

        currentLD = lensDist.intensity.value;
        startLD = SetupStartSetting(startLD, currentLD);

        while (Mathf.Abs(currentAb - maxAbIntensity) > 0.01f)
        {
            // Change intensity of Chromatic Aberration 
            currentAb = Mathf.Lerp(currentAb, maxAbIntensity, rewindDeformSpeed * Time.deltaTime);
            chromaticAb.intensity.value = currentAb;

            // Change Intensity of Lens Distortion
            currentLD = Mathf.Lerp(currentLD, targetLDIntensity, rewindDeformSpeed * Time.deltaTime);
            lensDist.intensity.value = currentLD;

            yield return null;
        }
    }

    private IEnumerator RestoreCamera()
    {
        while (Mathf.Abs(currentAb - startAb) > 0.01f)
        {
            // Change intensity of Chromatic Aberration 
            currentAb = Mathf.Lerp(currentAb, startAb, rewindDeformSpeed * Time.deltaTime);
            chromaticAb.intensity.value = currentAb;

            // Change Intensity of Lens Distortion
            currentLD = Mathf.Lerp(currentLD, startLD, rewindDeformSpeed * Time.deltaTime);
            lensDist.intensity.value = currentLD;

            yield return null;
        }

    }

    #endregion


    #region Cinemachine Related

    private void UpdateCamera(object obj)
    {
        if (obj is not CameraData) return;
        CameraData cameraData = (CameraData)obj;

        if (cameraData.zoomAmount >= 0 && cameraData.zoomAmount != currentZoom)
        {
            Debug.Log(cameraData.zoomAmount);

            if (cinemachineCoroutine != null)
            {
                StopCoroutine(cinemachineCoroutine);
                cinemachineCoroutine = null;
            }

            cinemachineCoroutine = AdjustCamera(cameraData.zoomAmount, cameraData.offset, cameraData.damping);
            StartCoroutine(cinemachineCoroutine);

        }
    }

    // Apply new camera data to the camera
    private IEnumerator AdjustCamera(float targetZoom, Vector3 targetOffset, Vector2 targetDamping)
    {
        while (Mathf.Abs(currentZoom - targetZoom) > 0.01f)
        {
            // Apply zoom
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, transitionSpeed * Time.deltaTime);

            currentOffset = SplitLerp(currentOffset, targetOffset);
            currentDamping = SplitLerp(currentDamping, targetDamping);

            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = currentDamping.x;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = currentDamping.y;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentOffset;
            mainCam.m_Lens.OrthographicSize = currentZoom;
            yield return null;

        }

        cinemachineCoroutine = null;
    }

    // Used to filter the new camera settings, update settings if they are different from current
    private Vector3 SplitLerp(Vector3 current, Vector3 target)
    {
        if (current.x != target.x)
        {
            current.x = Mathf.Lerp(current.x, target.x, transitionSpeed * Time.deltaTime);
        }
        if (current.y != target.y)
        {
            current.y = Mathf.Lerp(current.y, target.y, transitionSpeed * Time.deltaTime);
        }

        return current;
    }

    #endregion

}
