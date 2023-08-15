using BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossGroundManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Transform groundCenter;
    [SerializeField] Transform ceilingCenter;
    [SerializeField] Transform hideGroundCenter;
    [SerializeField] Transform hideCeilingCenter;
    [SerializeField] GameObject tilePrefab;

    [Header("Value")]
    [Tooltip("Imposta in quanto tempo compare il terreno/soffitto durante il ribaltamento")]
    [SerializeField] float fadeInDuration = 5f;
    [Tooltip("Imposta in quanto tempo scompare il terreno/soffitto durante il ribaltamento")]
    [SerializeField] float fadeOutDuration = 5f;
    [Tooltip("Imposta per quanto tempo vene scosso il terreno/soffitto prima di iniziare a scomparire")]
    [SerializeField] float shakeDuration = 5f;
    [Tooltip("Imposta l'intensità del movimento orizzontale quando il terreno/soffitto trema")]
    [SerializeField] float shakeHorizontalMagnitude = 2f;
    [Tooltip("Imposta l'intensità del movimento verticale quando il terreno/soffitto trema")]
    [SerializeField] float shakeVerticalMagnitude = 0.5f;
    [Tooltip("Imposta la velocità di movimento orizzontale quando il terreno/soffitto trema")]
    [SerializeField] float shakeHorizontalSpeed = 5f;
    [Tooltip("Imposta la velocità di movimento verticale quando il terreno/soffitto trema")]
    [SerializeField] float shakeVerticalSpeed = 5f;

    List<GameObject> groundTiles = new();
    List<GameObject> ceilingTiles = new();
    bool groundIsActive;
    eState state;
    float elapsedTime;

    private enum eState
    {
        Idle,
        FadeIn,
        FadeOut,
        Shake
    }

    private void Start()
    {
        InitialSetup();
    }

    private void InitialSetup()
    {
        float maxDistance = Camera.main.orthographicSize * Camera.main.aspect;
        float tileWidth = tilePrefab.transform.localScale.x;
        int numberOfPairs = Mathf.FloorToInt(maxDistance / tileWidth);

        groundTiles.Add(Instantiate(tilePrefab, groundCenter.position, Quaternion.identity, groundCenter));
        ceilingTiles.Add(Instantiate(tilePrefab, ceilingCenter.position, Quaternion.Euler(180, 0f, 0f), groundCenter));

        for (int i = 1; i <= numberOfPairs; i++)
        {
            //Pavimento
            Vector3 rightPosition = groundCenter.transform.position + new Vector3(tileWidth * i, 0f, 0f);
            groundTiles.Add(Instantiate(tilePrefab, rightPosition, Quaternion.identity, groundCenter));

            Vector3 leftPosition = groundCenter.transform.position - new Vector3(tileWidth * i, 0f, 0f);
            groundTiles.Add(Instantiate(tilePrefab, leftPosition, Quaternion.identity, groundCenter));

            //Soffitto
            rightPosition = ceilingCenter.transform.position + new Vector3(tileWidth * i, 0f, 0f);
            ceilingTiles.Add(Instantiate(tilePrefab, rightPosition, Quaternion.Euler(180, 0f, 0f), ceilingCenter));

            leftPosition = ceilingCenter.transform.position - new Vector3(tileWidth * i, 0f, 0f);
            ceilingTiles.Add(Instantiate(tilePrefab, leftPosition, Quaternion.Euler(180, 0f, 0f), ceilingCenter));
        }

        groundTiles.Reverse();
        ceilingTiles.Reverse();
        groundIsActive = true;
        DeactivateAllTiles(ceilingTiles);
        state = eState.Idle;
    }

    private void FadeIn()
    {
        if (elapsedTime <= fadeInDuration)
        {
            List<GameObject> tiles;
            float y;
            float t = elapsedTime / fadeInDuration;
            if (groundIsActive)
            {
                tiles = ceilingTiles;
                y = Mathf.Lerp(hideCeilingCenter.position.y, ceilingCenter.position.y, t);
            }
            else
            {
                tiles = groundTiles;
                y = Mathf.Lerp(hideGroundCenter.position.y, groundCenter.position.y, t);
            }

            foreach (GameObject tile in tiles)
            {
                tile.transform.position = new Vector3(tile.transform.position.x, y);
            }
        }
        else
        {
            ChangeState(eState.Shake);
        }
    }

    private void FadeOut()
    {
        List<GameObject> tiles;
        float startY;
        float endY;
        
        if (groundIsActive)
        {
            tiles = groundTiles;
            startY = groundCenter.position.y;
            endY = hideGroundCenter.position.y;
        }
        else
        {
            tiles = ceilingTiles;
            startY = ceilingCenter.position.y;
            endY = hideCeilingCenter.position.y;
        }

        if (elapsedTime <= fadeOutDuration)
        {
            float y;
            float t;
            int count = 0;
            bool increment = false;
            float delay = fadeOutDuration / ((tiles.Count % 2 == 0 ? tiles.Count : tiles.Count + 1) / 2);
            foreach (GameObject tile in tiles)
            {
                if (elapsedTime > count * delay )
                {
                    t = (elapsedTime - (delay * count)) / delay;
                    y = Mathf.Lerp(startY, endY, t);
                    tile.transform.position = new Vector3(tile.transform.position.x, y);
                }
                if (increment)
                {
                    count++;
                    increment = !increment;
                }
                else
                {
                    increment = !increment;
                }
                    
            }
        }
        else
        {
            ChangeState(eState.Idle);
            groundIsActive = !groundIsActive;
            DeactivateAllTiles(tiles);
        }
        
    }

    private void Shake()
    {
        List<GameObject> tiles;
        if (groundIsActive)
        {
            tiles = groundTiles;
        }
        else
        {
            tiles = ceilingTiles;
        }

        if (elapsedTime <= shakeDuration)
        {
            float verticalTime = Time.time * shakeVerticalSpeed;
            float horizontalTime = Time.time * shakeHorizontalSpeed;
            float verticalDisplacement = Mathf.Sin(verticalTime) * shakeVerticalMagnitude;
            float horizontalDisplacement = Mathf.Sin(horizontalTime) * shakeHorizontalMagnitude;
            Vector3 newPosition = new Vector3(horizontalDisplacement, verticalDisplacement);

            foreach (GameObject tile in tiles)
            {
                Transform sprite = tile.GetComponentInChildren<SpriteRenderer>().transform;
                sprite.localPosition = newPosition;
            }
        }
        else
        {
            foreach (GameObject tile in tiles)
            {
                Transform sprite = tile.GetComponentInChildren<SpriteRenderer>().transform;
                sprite.localPosition = Vector3.zero;
            }
            ChangeState(eState.FadeOut);
        }
    }

    private void DeactivateAllTiles(List<GameObject> tiles)
    {
        foreach (GameObject t in tiles)
        {
            t.SetActive(false);
        }
    }

    private void ActivateAllTiles(List<GameObject> tiles)
    {
        foreach (GameObject t in tiles)
        {
            t.SetActive(true);
        }
    }

    public bool UpdateState()
    {
        elapsedTime += Time.deltaTime;
        bool idle = false;
        switch (state)
        {
            case eState.FadeIn:
                FadeIn(); 
                break;
            case eState.FadeOut:
                FadeOut(); 
                break;
            case eState.Shake: 
                Shake(); 
                break;
            case eState.Idle: 
                idle = true; 
                break;
        }
        return idle;
    }

    public void StartChangeGround()
    {
        List<GameObject> tiles;
        if (groundIsActive)
            tiles = ceilingTiles;
        else
            tiles = groundTiles;

        ActivateAllTiles(tiles);

        ChangeState(eState.FadeIn);
    }

    private void ChangeState(eState state)
    {
        this.state = state;
        elapsedTime = 0;
    }

    public bool IsInFadeOut()
    {
        if(state == eState.FadeOut) 
            return true;
        else 
            return false;
    }
}
