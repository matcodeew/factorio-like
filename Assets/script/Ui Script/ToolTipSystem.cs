using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TooltipScript : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipPanel;
    public List<RectTransform> objectsToTooltip;  
    public List<string> infoTexts;  
    public Vector2 fixedPosition = new Vector2(100, 100);

    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        bool isHovering = false;
        foreach (var rectTransform in objectsToTooltip)
        {
            if (rectTransform.gameObject.activeInHierarchy && IsMouseOverUIElement(rectTransform))
            {
                int index = objectsToTooltip.IndexOf(rectTransform);
                tooltipPanel.SetActive(true);
                tooltipText.text = infoTexts[index]; 
                tooltipPanel.transform.position = fixedPosition;

                isHovering = true;
                break;
            }
        }
        if (!isHovering)
        {
            tooltipPanel.SetActive(false);
        }
    }

    private bool IsMouseOverUIElement(RectTransform rectTransform)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }
}
