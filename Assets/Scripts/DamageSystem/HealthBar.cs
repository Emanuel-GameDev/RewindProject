using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected GameObject healthIconPrefab;

    List<GameObject> icons;

    private void Awake()
    {
        icons = new List<GameObject>();
    }

    public virtual void InitializeHealthBar(int health)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            Destroy(icons[i]);
        }

        icons.Clear();

        for (int i = 0; i < health; i++)
        {
            icons.Add(Instantiate(healthIconPrefab, transform));
        }
    }

    public virtual void UpdateHealthBar(int health)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if(i < health)
                icons[i].SetActive(true);
            else
                icons[i].SetActive(false);
        }
    }


}
