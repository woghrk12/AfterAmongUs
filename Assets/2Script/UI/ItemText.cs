using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    [SerializeField] private Text itemText;
    [SerializeField] private Text numText;
    [SerializeField] private float time;

    private Color textColor;
    private float alphaValue;

    public void SetText(ItemType p_itemType, int p_num)
    {
        itemText.text = p_itemType.ToString();
        numText.text = p_num.ToString();
        StartCoroutine(FadeOut(time));
    }

    private IEnumerator FadeOut(float p_time)
    {
        var t_time = p_time;
        
        yield return new WaitForSeconds(2f);

        textColor = itemText.color;

        while (t_time > 0f)
        {
            t_time -= 0.1f;
            alphaValue = Mathf.Lerp(0f, 1f, t_time / p_time);
            textColor.a = alphaValue;

            itemText.color = textColor;
            numText.color = textColor;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    } 
}
