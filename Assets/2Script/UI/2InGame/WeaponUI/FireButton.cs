using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isFire = false;
    [SerializeField] private Image fireButton = null;

    public void Alert(string p_text)
    {
        Debug.Log(p_text);
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
  
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

  
}
