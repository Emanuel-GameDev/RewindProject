using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEditor;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI buttonText;

    [SerializeField] Color baseColor;
    [SerializeField] Color selectedColor;

    [SerializeField] UnityEvent OnClick;

    Vector3 normalSize;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
        GetComponent<RectTransform>().localScale = new Vector3(normalSize.x - 0.1f, normalSize.y - 0.1f);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = selectedColor;
        GetComponent<RectTransform>().localScale = new Vector3(normalSize.x+0.1f,normalSize.y+0.1f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = baseColor;
        GetComponent<RectTransform>().localScale = new Vector3(normalSize.x - 0.1f, normalSize.y - 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = baseColor;
        normalSize = GetComponent<RectTransform>().localScale;
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
