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
    //[SerializeField] float timelineSpeedMultiplier = 2.0f;
    [SerializeField] float lockTime = 5f;
    [SerializeField] List<TimeZone> timeZones;

    //private float timelineSpeed;
    private float timelineDuration;
    private PlayerInputs playerInputs;
    private eZone actualZone;
    private bool isPlaying = false;
    private bool isLocked = false;
    private float elapsedTime = 0;

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
        TimelineControls(); //da Togliere quando diventerà un'abilità

        CheckIsPlaying();
        CheckIsLocked();
    }

    private void CheckIsLocked()
    {
        if (isLocked)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > lockTime)
            {
                PlayCurrentTimeline();
            }
        }
    }

    private void CheckIsPlaying()
    {
        if (isPlaying)
        {
            if (timelineDirector.time >= timelineDuration)
            {
                PauseTimeline();
            }
        }
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

        timelineDirector.playOnAwake = false;
        timelineDirector.extrapolationMode = DirectorWrapMode.Hold;

    }

    private void CalculateDuration()
    {
        timelineDuration = (float)timelineDirector.duration;
    }

    private void TimelineControls()
    {
        //bool speedUpTime = playerInputs.TimelineController.SpeedUpTime.ReadValue<float>() > 0;
        bool forwardingTime = playerInputs.TimelineController.ForwardingTime.ReadValue<float>() > 0;
        bool rewindTime = playerInputs.TimelineController.RewindTime.ReadValue<float>() > 0;

        if (Input.GetKeyDown(KeyCode.P)) { PlayCurrentTimeline(); } ///Temporanea per test
        if (Input.GetKeyDown(KeyCode.O)) { LockInTime(); } ///Temporanea per test

        //if (speedUpTime)
        //{
        //    speed = timelineSpeed * timelineSpeedMultiplier;
        //}
        //else
        //{
        //    speed = timelineSpeed;
        //}

        if (rewindTime)
        {
            RewindTimeline();
        }

        if (forwardingTime)
        {
            ForwardingTimeline();
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

        PauseTimeline();

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

    public void PlayCurrentTimeline()
    {
        timelineDirector.Play();
        isPlaying = true;
    }

    public void LockInTime()
    {
        isLocked = true;
        PauseTimeline();
        elapsedTime = 0;
    }

    public void PauseTimeline()
    {
        isPlaying = false;
        timelineDirector.Pause();
    }

    public void RewindTimeline()
    {
        if (timelineDirector.time > 0)
        {
            timelineDirector.time -= timelineSpeed * Time.deltaTime;
            timelineDirector.Evaluate();
        }
    }

    public void ForwardingTimeline()
    {
        if (timelineDirector.time < timelineDuration)
        {
            timelineDirector.time += timelineSpeed * Time.deltaTime;
            timelineDirector.Evaluate();
        }
    }

}
