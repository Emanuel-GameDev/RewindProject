using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathCreator : MonoBehaviour
{

    LineRenderer lineRenderer;
    public float projectileSpeed = 1;
    List<Vector3> points;

    public GameObject instatietedProjectile;
    bool projectileSpawned=false;

    public bool isDrawing=false; 

    private void Awake()
    {
        points = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (isDrawing)
        {
            RegisterPoints();
            return;
        }

        if (!instatietedProjectile)
        {
            Destroy(gameObject);
            return;
        }

        if (projectileSpawned)
        {
            if(points.Count>0)
            instatietedProjectile.transform.position = Vector2.MoveTowards(instatietedProjectile.transform.position, points[0], projectileSpeed * Time.deltaTime );
           
            if (instatietedProjectile.transform.position == points[0])
            {
                points.RemoveAt(0);
            }
            //potrebbe essere un po pesante
                ReloadLine();

            if (points.Count == 0)
            {
                Destroy(instatietedProjectile.gameObject);
            }

        }

    }

    private void RegisterPoints()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (points.Count > 1)
        {
            if (Vector2.Distance(points[points.Count-1], point) > 0.1)
            {
                points.Add(new Vector3(point.x, point.y));
            }
        }
        else
            points.Add(new Vector3(point.x, point.y));

        ReloadLine();

    }

    void ReloadLine()
    {
        lineRenderer.positionCount = points.Count;
        List<Vector3> pointsToDraw = new List<Vector3>();

        if (instatietedProjectile)
        {
            lineRenderer.positionCount += 1;
            pointsToDraw.Add(instatietedProjectile.transform.position);
        }

        pointsToDraw.AddRange(points);
        
        lineRenderer.SetPositions(pointsToDraw.ToArray());
    }

    public void SpawnProjectile(GameObject projectile)
    {
        if(points.Count<1)
            return;

        instatietedProjectile = Instantiate(projectile, points[0], Quaternion.identity);
        instatietedProjectile.GetComponent<PlayerProjectile>().lifeTime = 100f;
        points.RemoveAt(0);
        projectileSpawned = true;
    }


    
}
