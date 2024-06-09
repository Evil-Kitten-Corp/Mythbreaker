using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTransitionText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public Color normalColor;
    public Color hoveredColor;
    public float transitionTime = 0.05f;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.DOColor(normalColor, transitionTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.DOColor(hoveredColor, transitionTime);
    }
}
