using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    [SerializeField] PlayableDirector timelineDirector;
    [Tooltip("Imposta la velocità di movimento degli oggtti lungo la timeline")]
    [SerializeField] float timelineSpeed = 1.0f;
    [Tooltip("Imposta per quanto tempo rimangono bloccati nel tempo gli oggetti")]
    [SerializeField] float lockTime = 5f;
    [Tooltip("Usato per collegare le timeline con le Zone corrispondenti")]
    [SerializeField] List<TimeZone> timeZones;


    private float timelineDuration;
    private eZone actualZone;
    private bool isPlaying = false;
    private bool isLocked = false;
    private bool _rewindIsActive = false;
    private bool RewindIsactive
    {
        get { return _rewindIsActive; }
        set
        {
            if (value)
            {
                PlayerController.instance.inputs.Player.Disable();
                PubSub.Instance.Notify(EMessageType.TimeRewindStart, this);
            }
            else
            {
                PlayerController.instance.inputs.Player.Enable();
                PubSub.Instance.Notify(EMessageType.TimeRewindStop, this);
            }
            _rewindIsActive = value;
        }
    }

    private bool timelineIsAtStart => timelineDirector.time <= 0;
    private bool timelineIsAtEnd => timelineDirector.time >= (timelineDuration - 0.001);
    private float elapsedTime = 0;

    private bool isForwarding = false;
    private bool isRewinding = false;
    private bool canUseRewind = false;


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
        CheckIsPlaying();
        CheckIsLocked();
        ManipulateTimeline();
    }

    private void ManipulateTimeline()
    {
        if (isForwarding) ForwardingTimeline();
        if (isRewinding) RewindTimeline();
    }

    private void Awake()
    {
        InstanceSet();
    }

    //FUNZIONI PRIVATE
    //==========================================================================================================
    #region Funzioni Private
    private void InitialSetup()
    {
        if (timelineDirector == null)
            timelineDirector = GetComponent<PlayableDirector>();

        timelineDirector.playOnAwake = false;
        timelineDirector.extrapolationMode = DirectorWrapMode.Hold;
        //RewindIsactive = false;
    }

    private void CalculateDuration()
    {
        timelineDuration = (float)timelineDirector.duration;
    }

    private void SetTime(float actualTime)
    {
        timelineDirector.time = actualTime;
    }

    private void ResetTime()
    {
        timelineDirector.time = 0;
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
            if (timelineIsAtEnd)
            {
                PauseTimeline();
            }
        }
    }

    private bool CanRewind()
    {
        if (isLocked) return false;
        if (isPlaying) return false;
        if (!canUseRewind) return false;

        if (!RewindIsactive)
        {
            StartStopControlTimeline();
        }

        return true;
    }

    #endregion

    //FUNZIONI PUBBLICHE
    //==========================================================================================================
    #region Funzioni Pubbliche
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
        isLocked = false;
    }

    public void LockInTime()
    {
        if (timelineIsAtStart || timelineIsAtEnd)
        {
            RewindIsactive = false;
        }
        else
        {
            isLocked = true;
            PauseTimeline();
            elapsedTime = 0;
            RewindIsactive = false;
        }
    }

    public void PauseTimeline()
    {
        isPlaying = false;
        timelineDirector.Pause();
    }

    public void RewindTimeline()
    {
        if (!timelineIsAtStart)
        {
            timelineDirector.time -= timelineSpeed * Time.deltaTime;
            timelineDirector.Evaluate();
        }
    }

    public void ForwardingTimeline()
    {
        if (!CanRewind()) return;

        if (!timelineIsAtEnd)
        {
            timelineDirector.time += timelineSpeed * Time.deltaTime;
            timelineDirector.Evaluate();
        }
    }

    public void StartStopControlTimeline()
    {
        if (isLocked || isPlaying || !canUseRewind)
            return;
        
        if (!RewindIsactive)
            RewindIsactive = true;
        else
            LockInTime();
    }

    public void StartForwarding()
    {
        if (!CanRewind()) return;
        isForwarding = true;
    }

    public void StartRewinding()
    {
        if (!CanRewind()) return;
        isRewinding = true;
    }

    public void StopForwarding()
    {
        isForwarding = false;
    }

    public void StopRewinding()
    {
        isRewinding = false;
    }

    public void SetCanUseRewind(bool v)
    {
        canUseRewind = v;
    }

    #endregion
}
