using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TooltipScript : MonoBehaviour
{
    // Références UI
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipPanel;  // Panneau qui contient le texte

    // Liste des RectTransforms des objets et des informations associées
    public List<RectTransform> objectsToTooltip;  // Liste des RectTransforms à interagir avec
    public List<string> infoTexts;  // Liste des textes d'information pour chaque objet

    // Position fixe du panneau de tooltip (exemple ici, dans le coin supérieur droit)
    public Vector2 fixedPosition = new Vector2(100, 100);

    private void Start()
    {
        // Cache le panneau de la bulle d'information au début
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        // Vérifie si la souris survole l'un des objets et si l'objet est activé
        bool isHovering = false;
        foreach (var rectTransform in objectsToTooltip)
        {
            // Vérifie si l'objet associé au RectTransform est activé
            if (rectTransform.gameObject.activeInHierarchy && IsMouseOverUIElement(rectTransform))
            {
                // Trouve l'index de l'objet survolé
                int index = objectsToTooltip.IndexOf(rectTransform);

                // Affiche la bulle d'information pour cet objet
                tooltipPanel.SetActive(true);
                tooltipText.text = infoTexts[index];  // Met à jour le texte d'information

                // Position fixe du panneau de tooltip (ajusté à la position choisie)
                tooltipPanel.transform.position = fixedPosition;

                isHovering = true;
                break;
            }
        }

        // Si la souris n'est pas sur un objet ou si l'objet est désactivé, cache la bulle d'information
        if (!isHovering)
        {
            tooltipPanel.SetActive(false);
        }
    }

    // Méthode pour vérifier si la souris est sur un élément UI
    private bool IsMouseOverUIElement(RectTransform rectTransform)
    {
        // Utilise un raycast pour vérifier si la souris est au-dessus de l'UI
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }
}
