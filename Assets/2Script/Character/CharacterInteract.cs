using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteract : MonoBehaviour
{
    [SerializeField] private Button useButton = null;
    private List<GameObject> canUseObjects = new List<GameObject>();
    private GameObject useObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interactable")) return;

        canUseObjects.Add(collision.gameObject);

        if (useButton.interactable) return;

        useObject = collision.gameObject;
        useButton.interactable = true;
        useButton.onClick.AddListener(() =>
        {
            collision.GetComponent<IInteractable>().Use();
        });
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interactable")) return;

        canUseObjects.Remove(collision.gameObject);

        if (useObject != collision.gameObject) return;
        
        useButton.onClick.RemoveAllListeners();

        if (canUseObjects.Count <= 0)
        {
            useObject = null;
            useButton.interactable = false;
            return;
        }

        useObject = canUseObjects[0];
        useButton.onClick.AddListener(() =>
        {
            collision.GetComponent<IInteractable>().Use();
        });
    }
}
