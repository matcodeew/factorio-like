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
    public Dictionary<RectTransform, GameObject> objectsToTooltip = new();
    [SerializeField] private List<RectTransform> _objectToAddToDictionary = new();
    public List<string> infoTexts;
    public List<GameObject> RequireRessources = new();
    public Vector2 fixedPosition = new Vector2(100, 250);
    [SerializeField] private GameObject RepeicePanel;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach (var objectToAdd in _objectToAddToDictionary)
        {
            objectsToTooltip.Add(objectToAdd, objectToAdd.gameObject);
        }
    }
    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        bool isHovering = false;
        foreach (var rectTransform in objectsToTooltip.Keys)
        {
            if (rectTransform.gameObject.activeInHierarchy && IsMouseOverUIElement(rectTransform))
            {
                int index = GetIndexFromDictionaryKey(rectTransform);
                tooltipPanel.SetActive(true);
                tooltipText.text = infoTexts[index];
                tooltipPanel.transform.position = fixedPosition;
                ShowRepeiceBuilding(rectTransform);

                isHovering = true;
                break;
            }
        }
        if (!isHovering)
        {
            tooltipPanel.SetActive(false);
        }
    }

    private void ShowRepeiceBuilding(RectTransform transformSelected)
    {
        if (transformSelected.gameObject.tag == "Build")
        {
            RepeicePanel.SetActive(true);
            List<InvRessource> repeice = new();
            objectsToTooltip.TryGetValue(transformSelected, out GameObject buildValue);
            foreach (var ressource in buildValue.transform.GetComponent<BuildingData>().NeededRessources)
            {
                repeice.Add(new InvRessource(ressource.Ressource, ressource.Quantity));
            }
            for (int i = 0; i < RequireRessources.Count; i++)
            {
                if (i < repeice.Count)
                {
                    RequireRessources[i].SetActive(true);
                    RequireRessources[i].transform.GetChild(0).GetComponent<Image>().sprite = repeice[i].Ressource.Sprite;
                    RequireRessources[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = "x " + repeice[i].Quantity.ToString();
                }
                else
                {
                    RequireRessources[i].SetActive(false);
                }
            }
        }
        else { RepeicePanel.SetActive(false); }
    }

    private bool IsMouseOverUIElement(RectTransform rectTransform)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }

    private int GetIndexFromDictionaryKey(RectTransform rectTransform)
    {
        int index = 0;
        foreach (var key in objectsToTooltip.Keys)
        {
            if (key == rectTransform)
                return index;
            index++;
        }
        return -1; // Si la clé n'existe pas
    }
}
