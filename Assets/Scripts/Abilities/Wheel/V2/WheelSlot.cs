using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSlot : MonoBehaviour
{
    [SerializeField] private Ability attachedAbility;

    public bool central = false;

    private TextMeshProUGUI slotText;

    private void Awake()
    {
        slotText = GetComponentInChildren<TextMeshProUGUI>(includeInactive:true);

        if (slotText != null)
        {
            central = true;

            Outline component = gameObject.GetComponentInChildren<Outline>();

            // Se il componente non è presente, aggiungilo
            if (component == null)
            {
                component = gameObject.AddComponent<Outline>();
            }
        }
    }

    public TextMeshProUGUI GetSlotText()
    {
        return slotText;
    }

    public void SetSlotText(string text)
    {
        if (slotText == null) return; 

        slotText.text = text;
        slotText.gameObject.SetActive(true);
    }

    public void AttachAbility(Ability ability)
    {
        attachedAbility = ability;
    }

    public Ability GetAttachedAbility()
    {
        return attachedAbility;
    }
}
