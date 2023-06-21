using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class JumpLink : MonoBehaviour
{
    #region Menu
    private static int numberOfChild = 2;


    [MenuItem("GameObject/Custom/Jump Link")]
    private static void CreateJumpLink()
    {
        GameObject go = new GameObject();
        go.AddComponent<JumpLink>();
        go.name = "Jump Link";
        for (int i = 0; i< numberOfChild; i++)
        {
            AddJumpPoint(go, i);
        }
        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

    }

    private static void AddJumpPoint(GameObject go, int number)
    {
        GameObject jp = new GameObject();
        jp.AddComponent<JumpPoint>();
        jp.name = "Point " + number;
        jp.transform.parent = go.transform;
        jp.transform.localPosition = new Vector3(number, 0, 0);
    }
    #endregion

    [SerializeField] float enemyMaxJumpDistance = 7;
    [SerializeField] List<JumpPoint> jumpPoints;

    private void Start()
    {
        SearchJumpPoints();
    }

    private void SearchJumpPoints()
    {
        jumpPoints = GetComponentsInChildren<JumpPoint>().ToList<JumpPoint>();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Vector3 pointA = transform.GetChild(i).transform.position;
            Vector3 pointB = transform.GetChild(i + 1).transform.position;


            if (Vector2.Distance(pointA,pointB) < enemyMaxJumpDistance)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color= Color.red;
            }

            Gizmos.DrawLine(pointA, pointB);
        }
    }
}
