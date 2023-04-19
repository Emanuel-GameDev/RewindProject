using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    [SerializeField] PlayableDirector timelineDirector;
    [SerializeField] float timelineSpeed = 1.0f;
    [SerializeField] float timelineSpeedMultiplier = 2.0f;
    [SerializeField] List<TimeZone> timeZones;

    private float speed;
    private float timelineDuration;
    private PlayerInputs playerInputs;
    private eZone actualZone;

    //Instance
    //==========================================================================================================
    public static TimelineManager Instance { get; private set; }
    private void InstanceSet()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    //Start,Update, Awake, etc.
    //==========================================================================================================
    private void Start()
    {
        InitialSetup();
        CalculateDuration();
        ResetTime();
    }

    void Update()
    {
        TimelineControls();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        InstanceSet();
    }

   

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    //FUNZIONI PRIVATE
    //==========================================================================================================

    private void InitialSetup()
    {
        if (timelineDirector == null)
            timelineDirector = GetComponent<PlayableDirector>();
    }

    private void CalculateDuration()
    {
        timelineDuration = (float)timelineDirector.duration;
    }

    private void TimelineControls()
    {
        bool speedUpTime = playerInputs.TimelineController.SpeedUpTime.ReadValue<float>() > 0;
        bool forwardingTime = playerInputs.TimelineController.ForwardingTime.ReadValue<float>() > 0;
        bool rewindTime = playerInputs.TimelineController.RewindTime.ReadValue<float>() > 0;

        if (speedUpTime)
        {
            speed = timelineSpeed * timelineSpeedMultiplier;
        }
        else
        {
            speed = timelineSpeed;
        }

        if (rewindTime && timelineDirector.time > 0)
        {
            timelineDirector.time -= speed;
            timelineDirector.Evaluate();
        }

        if (forwardingTime && timelineDirector.time < timelineDuration)
        {
            timelineDirector.time += speed;
            timelineDirector.Evaluate();
        }
    }

    private void SetTime(float actualTime)
    {
        timelineDirector.time = actualTime;
    }

    private void ResetTime()
    {
        timelineDirector.time = 0;
    }

    //FUNZIONI PUBBLICHE
    //==========================================================================================================

    public void ChangeTimeline(eZone zone)
    {
        if(zone == actualZone) 
            return;
        
        TimeZone timezone = timeZones.Find(x => x.zone == zone);
        PlayableAsset timeline = timezone.timeline;

        if (timeline != null)
            timelineDirector.playableAsset = timeline;
        else
            Debug.LogError("Timeline non trovata!");

        timeZones.Find(x => x.zone == actualZone).SetActualTime((float)timelineDirector.time);

        CalculateDuration();
        SetTime(timezone.actualTime);

        actualZone = zone;
    }
}
