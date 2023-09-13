using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    [SerializeField] public List<HealthSection> healthSections;


    public override void InitializeHealthBar(int health)
    {
        for (int i = 0; i < healthSections.Count; i++)
        {
            healthSections[i].ActivateSection(true);
        }
    }

    public override void UpdateHealthBar(int health)
    {
        for (int i = 0; i < healthSections.Count; i++)
        {
            if (i < health)
                healthSections[i].ActivateSection(true);
            else
                healthSections[i].ActivateSection(false);
        }
    }

}
