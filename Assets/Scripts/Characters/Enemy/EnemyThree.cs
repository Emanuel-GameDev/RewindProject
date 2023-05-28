using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : BaseEnemy
{
    [Header("Bheaviour Tree Data")]
    [Tooltip("Imposta la velocità di movimento del nemico")]
    [SerializeField] float speed = 2.5f;
    [Tooltip("Imposta la distanza massima entro cui vede il bersaglio")]
    [SerializeField] float viewDistance = 15;


    //Nomi delle variabili nel behaviour tree
    private const string TARGET = "Target";
    private const string VIEW_DISTANCE = "View Distance";
    private const string SPEED = "Speed";

    public override void Awake()
    {
        base.Awake();
        InitialSetup();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitialSetup()
    {
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(SPEED, speed);
    }
}
