using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent m_RunOnClick; 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<TextMeshProUGUI>().color = Color.yellow; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<TextMeshProUGUI>().color = Color.white; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
        m_RunOnClick.Invoke(); 
    }
}
