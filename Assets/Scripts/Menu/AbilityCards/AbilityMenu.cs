using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour
{
    [SerializeField] private RectTransform showCardsPanel;
    [SerializeField] private Vector2 CardSize;

    private List<Image> loadedCards;

    private void LoadCards()
    {
        List<Ability> unlockedAbilities = new List<Ability>();

        if (loadedCards == null)
            loadedCards = new List<Image>();

        unlockedAbilities = GameManager.Instance.abilityManager.GetUnlockedAbilities();

        for (int i = 0; i < unlockedAbilities.Count; i++)
        {
            GameObject newCard = new GameObject("PanelSlot_" + i);

            // Setup Size
            newCard.AddComponent<RectTransform>();
            newCard.GetComponent<RectTransform>().sizeDelta = CardSize;

            // Setup Image
            newCard.AddComponent<Image>();
            newCard.GetComponent<Image>().sprite = unlockedAbilities[i].icon;

            newCard.transform.SetParent(showCardsPanel, false);   
            loadedCards.Add(newCard.GetComponent<Image>());
        }
    }

    public bool CanBeOpened()
    {
        if (GameManager.Instance.abilityManager.GetUnlockedAbilities().Count == 0)
            return false;
        else
            return true;
    }

    public void Open()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        LoadCards();
    }

    public void Close()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        loadedCards.Clear();

        for (int i = 0; i < showCardsPanel.childCount; i++)
        {
            Destroy(showCardsPanel.GetChild(i).gameObject);
        }
    }
}
