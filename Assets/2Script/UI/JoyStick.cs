using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform leverTransform;
    private RectTransform rectTransform;
    private Vector2 inputPos;
    private Vector2 leverPos;
    private Vector2 inputDir;

    [SerializeField, Range(0, 150)] private float leverRange;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        InputDirection();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlLever(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        leverTransform.anchoredPosition = Vector2.zero;
        inputDir = Vector2.zero;
    }

    private void ControlLever(PointerEventData p_eventData)
    {
        inputPos = p_eventData.position - rectTransform.anchoredPosition;
        leverPos = inputPos.sqrMagnitude < leverRange * leverRange ? inputPos : inputPos.normalized * leverRange;
        inputDir = Vector2.ClampMagnitude(inputPos, 1f);
        leverTransform.anchoredPosition = leverPos;
    }

    private void InputDirection()
    {
        Debug.Log(inputDir.x + " / " + inputDir.y);
    }
}
