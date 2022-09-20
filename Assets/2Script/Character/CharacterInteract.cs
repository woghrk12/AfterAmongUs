using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteract : MonoBehaviour
{
    private Button useButton = null;

    private List<IInteractable> canUseObjects = new List<IInteractable>();
    private IInteractable useObject = null;

    private void Awake()
    {
        useButton = UIManager.Instance.UseButton;
        useButton.interactable = false;
    }

    public void AddObj(IInteractable p_obj)
    {
        canUseObjects.Add(p_obj);

        if (useObject != null) return;

        useObject = p_obj;
        useButton.onClick.AddListener(() => { p_obj.Use(); });
        useButton.interactable = true;
    }

    public void RemoveObj(IInteractable p_obj)
    {
        canUseObjects.Remove(p_obj);

        if (useObject != p_obj) return;

        useButton.onClick.RemoveAllListeners();

        if (canUseObjects.Count <= 0)
        {
            useButton.interactable = false;
            useObject = null;
            return;
        }

        useObject = canUseObjects[0];
        useButton.onClick.AddListener(() => { p_obj.Use(); });
    }
}
