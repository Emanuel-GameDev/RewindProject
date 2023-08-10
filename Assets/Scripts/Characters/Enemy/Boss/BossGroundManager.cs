using BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundManager : MonoBehaviour
{
    [SerializeField] Transform groundCenter;
    [SerializeField] Transform ceilingCenter;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] float groundHideDistance = 5f;
    [SerializeField] float ceilingHideDistance = 5f;

    List<GameObject> groundTiles = new();
    List<GameObject> ceilingTiles = new();

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
    }
}
