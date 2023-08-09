using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    #region Variables

    [Header("Cinemachine")]
    public CinemachineVirtualCamera mainCam;
    private List<Camera> overlays = new List<Camera>();

    [Tooltip("Duration of the transition")]
    [SerializeField] float cameraTDuration;

    private float currentZoom;
    private Vector3 currentOffset;
    private Vector2 currentDamping;
    private IEnumerator cinemachineCoroutine;

    [Header("PostProcessing")]
    [Tooltip("Duration of the transition")]
    [SerializeField] float deformTDuration;
    [Tooltip("LD = Lens Distortion")]
    [SerializeField, Range(-1f, +1f)] float targetLDIntensity;
    [SerializeField, Range(-100f, +100f)] float targetSaturation;
    [SerializeField, Range(-180f, +180f)] float targetHueShift; 

    private Volume rewindVolume;
    private IEnumerator deformCoroutine;
    private bool triggered = false;
    private ChromaticAberration chromaticAb;
    private float startAb;
    private LensDistortion lensDist;
    private float startLd;
    private ColorAdjustments colorAd;
    private float startSat;
    private float startHue;

    #endregion

    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, UpdateRewindPostProcess);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, UpdateRewindPostProcess);

        PubSub.Instance.RegisterFunction(EMessageType.RewindZoneEntered, GiveVolume);

        PubSub.Instance.RegisterFunction(EMessageType.CameraSwitch, UpdateCamera);
        mainCam.gameObject.SetActive(true);

        InitilizeCinemachine();
        InitializeOverlayCameras();
    }

    private void InitializeOverlayCameras()
    {
        Camera mainCamNoVirtual = Camera.main;
        if (mainCamNoVirtual.transform.childCount > 0)
        {
            for (int i = 0; i < mainCamNoVirtual.transform.childCount; i++)
            {
                if (mainCamNoVirtual.transform.GetChild(i).GetComponents<Camera>() != null)
                    overlays.Add(mainCamNoVirtual.transform.GetChild(i).GetComponent<Camera>());
            }
        }

        foreach (Camera camera in overlays)
        {
            camera.orthographicSize = mainCam.m_Lens.OrthographicSize;
        }
    }

    private void GiveVolume(object obj)
    {
        if (obj is not Volume) return;

        rewindVolume = obj as Volume;

        InitializeVolume();
    }

    private void Update()
    {
        // TEmporaneo, per testing
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateRewindPostProcess(1);
        }
    }

    private void InitializeVolume()
    {
        if (rewindVolume != null)
        {
            // Chromatic Aberration
            if (!rewindVolume.profile.TryGet(out chromaticAb))
                Debug.LogError("Error TryGet Chromatic Aberration");

            // Lens Distorsion
            if (!rewindVolume.profile.TryGet(out lensDist))
                Debug.LogError("Error TryGet Lens Distortion");

            // Color adjustment
            if (!rewindVolume.profile.TryGet(out colorAd))
                Debug.LogWarning("Error TryGet Color Adjustments");
        }
        else
            Debug.LogError("Error: no reference to rewindProfile");
    }

    private void InitilizeCinemachine()
    {
        currentZoom = mainCam.m_Lens.OrthographicSize;

        currentOffset = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        currentDamping.x = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        currentDamping.y = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping;
    }

    #region PostProcessing related

    private void UpdateRewindPostProcess(object obj)
    {
        float currentAb = chromaticAb.intensity.value;
        float currentLD = lensDist.intensity.value;
        float currentSat = colorAd.saturation.value;
        float currentHue = colorAd.hueShift.value;

        if (!triggered)
        {
            // Active rewind
            if (deformCoroutine != null)
            {
                StopCoroutine(deformCoroutine);
                deformCoroutine = null;
            }

            float maxAbIntensity = chromaticAb.intensity.max;
            startAb = SetupStartSetting(startAb, currentAb);
            startLd = SetupStartSetting(startLd, currentLD);
            startSat = SetupStartSetting(startSat, currentSat);
            startHue = SetupStartSetting(startHue, currentHue);

            deformCoroutine = DeformCamera(currentAb, maxAbIntensity, currentLD, targetLDIntensity, currentSat, targetSaturation, startHue, targetHueShift);
            StartCoroutine(deformCoroutine);

            triggered = !triggered;
        }
        else
        {
            // Stop rewind
            if (deformCoroutine != null)
            {
                StopCoroutine(deformCoroutine);
                deformCoroutine = null;
            }

            deformCoroutine = DeformCamera(currentAb, startAb, currentLD, startLd, currentSat, startSat, currentHue, startHue);
            StartCoroutine(deformCoroutine);

            triggered = !triggered;
        }
    }

    private float SetupStartSetting(float start, float current)
    {
        if (start != current)
            start = current;

        return start;
    }

    private IEnumerator DeformCamera(float currentAb, float targetAb, float currentLD, float targetLD, float currentSat, float targetSat, float currentHue, float targetHue)
    {
        float elapsedTime = 0;

        while (elapsedTime <= deformTDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / deformTDuration;

            // Change intensity of Chromatic Aberration 
            currentAb = Mathf.Lerp(currentAb, targetAb, t);
            chromaticAb.intensity.value = currentAb;

            // Change Intensity of Lens Distortion
            currentLD = Mathf.Lerp(currentLD, targetLD, t);
            lensDist.intensity.value = currentLD;

            // Change Saturation of Color Adjustments
            currentSat = Mathf.Lerp(currentSat, targetSat, t);
            colorAd.saturation.value = currentSat;

            // Change HueShift of Color Adjustments
            currentHue = Mathf.Lerp(currentHue, targetHue, t);
            colorAd.hueShift.value = currentHue;

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
        float elapsedTime = 0;

        while (elapsedTime <= cameraTDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraTDuration;

            currentZoom = Mathf.Lerp(currentZoom, targetZoom, t);

            currentOffset = SplitLerp(currentOffset, targetOffset, t);
            currentDamping = SplitLerp(currentDamping, targetDamping, t);

            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = currentDamping.x;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = currentDamping.y;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentOffset;
            mainCam.m_Lens.OrthographicSize = currentZoom;

            foreach (Camera camera in overlays)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, t);
            }

            yield return null;
        }

        cinemachineCoroutine = null;
    }

    // Used to filter the new camera settings, update settings if they are different from current
    private Vector3 SplitLerp(Vector3 current, Vector3 target, float t)
    {
        if (current.x != target.x)
        {
            current.x = Mathf.Lerp(current.x, target.x, t);
        }
        if (current.y != target.y)
        {
            current.y = Mathf.Lerp(current.y, target.y, t);
        }

        return current;
    }

    #endregion

}

