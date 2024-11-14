using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GlobalBumble : MonoBehaviour
{
    // Classe pour stocker l'objet UI et son texte de dialogue
    [System.Serializable]
    public class DialogueObject
    {
        public GameObject uiObject; // L'objet UI (ex. bouton ou image)
        public string dialogueText; // Le texte de la bulle de dialogue
    }

    // Liste des objets UI et leurs textes associés
    public List<DialogueObject> dialogueObjects;

    // Variables pour la bulle de dialogue
    public GameObject dialogueBubblePrefab; // Le prefab du panel de la bulle
    private GameObject instantiatedBubble; // Instance de la bulle
    private TextMeshProUGUI bubbleText; // Texte de la bulle
    private Canvas canvas; // Canvas sur lequel la bulle sera affichée

    void Start()
    {
        // Trouve le Canvas sur lequel la bulle sera affichée (en supposant qu'il y en a un dans la scène)
        canvas = FindObjectOfType<Canvas>();

        // Ajoute les événements de survol de la souris à chaque objet dans la liste
        foreach (var dialogueObject in dialogueObjects)
        {
            EventTrigger trigger = dialogueObject.uiObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = dialogueObject.uiObject.AddComponent<EventTrigger>();

            // Crée un événement pour la souris entrant
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => OnPointerEnter(dialogueObject));
            trigger.triggers.Add(entryEnter);

            // Crée un événement pour la souris quittant
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => OnPointerExit());
            trigger.triggers.Add(entryExit);
        }
    }

    // Fonction appelée lorsque la souris entre en contact avec l'objet UI
    private void OnPointerEnter(DialogueObject dialogueObject)
    {
        ShowDialogueBubble(dialogueObject);
    }

    // Fonction appelée lorsque la souris quitte l'objet UI
    private void OnPointerExit()
    {
        HideDialogueBubble();
    }

    // Affiche la bulle de dialogue au-dessus de l'élément UI
    private void ShowDialogueBubble(DialogueObject dialogueObject)
    {
        // Instancier la bulle de dialogue
        instantiatedBubble = Instantiate(dialogueBubblePrefab, canvas.transform);

        // Accède au texte de la bulle et définit le message
        bubbleText = instantiatedBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (bubbleText != null)
        {
            bubbleText.text = dialogueObject.dialogueText;
        }

        // Positionne la bulle au-dessus de l'élément UI
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out position);
        instantiatedBubble.GetComponent<RectTransform>().localPosition = position + new Vector2(0, 50); // Ajuste pour qu'elle soit un peu au-dessus
    }

    // Cache la bulle de dialogue
    private void HideDialogueBubble()
    {
        if (instantiatedBubble != null)
        {
            Destroy(instantiatedBubble);
        }
    }
}
