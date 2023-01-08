using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpinnerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Animator m_Spinner; 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Spinner.SetTrigger("t_Reveal");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Spinner.SetTrigger("t_Hide");
    }
}
