using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class TooltipScript : MonoBehaviour
{
    public static TooltipScript instance;
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipPanel;
    public List<RectTransform> objectsToTooltip;  
    public List<string> infoTexts;  
    public List<GameObject> RequireRessources = new();
    public Vector2 fixedPosition = new Vector2(100, 100);
    [SerializeField] private GameObject _RepicePanel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
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
                if(rectTransform.gameObject.tag == "Build")
                {
                   _RepicePanel.SetActive(true);
                    int RequireIndex = RequireRessources.IndexOf(rectTransform.gameObject);
                    InvRessource InvRessource = RequireRessources[RequireIndex].transform.GetChild(0).AddComponent<InvRessource>();
                    foreach (var building in InventoryBuildManager.Instance._buildingPrefabs) 
                    {
                        InvRessource.Ressource = building.GetComponent<BuildingData>().NeededRessources[0].Ressource; //0 = index foreach
                        InvRessource.Quantity = building.GetComponent<BuildingData>().NeededRessources[0].Quantity; //0 = index foreach
                        break;
                    }                                  
                }
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
