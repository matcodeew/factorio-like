using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject darkPanel; // Le panneau sombre.
    public TextMeshProUGUI tutorialText; // Texte des instructions.

    [Header("Tutorial Steps")]
    public List<GameObject> highlightObjects; // Liste des objets à surligner.
    public List<string> tutorialTexts; // Liste des textes correspondants.
    public Transform highlightParent; // Le parent dans lequel les objets seront déplacés.

    private int currentStep = 0;
    private List<Transform> originalParents = new List<Transform>(); // Pour stocker les parents d'origine.
    private List<int> originalSiblingIndexes = new List<int>(); // Pour stocker l'index hiérarchique original.

    void Start()
    {
        // Vérification des données
        if (highlightObjects.Count != tutorialTexts.Count)
        {
            Debug.LogError("Les listes des objets et des textes doivent avoir la même taille !");
            return;
        }

        // Stockage des parents et des indices originaux
        foreach (var obj in highlightObjects)
        {
            originalParents.Add(obj.transform.parent);
            originalSiblingIndexes.Add(obj.transform.GetSiblingIndex());
            // Ajouter le script d'interaction à chaque objet interactif
            TutorialObject tutorialObjectScript = obj.AddComponent<TutorialObject>();
            tutorialObjectScript.tutorialManager = this;
        }

        // Démarrage du tutoriel
        darkPanel.SetActive(true);
        HighlightCurrentStep(); // Démarrer avec le premier élément
    }

    void HighlightCurrentStep()
    {
        // Mettre en évidence l'objet actuel
        HighlightObject(currentStep);
    }

    void MoveToNextStep()
    {
        // Retirer la surbrillance de l'objet actuel
        RemoveHighlight(currentStep);

        // Passer à l'étape suivante
        currentStep++;

        // Vérifier si le tutoriel est terminé
        if (currentStep >= highlightObjects.Count)
        {
            EndTutorial();
        }
        else
        {
            HighlightCurrentStep(); // Continuer avec l'étape suivante
        }
    }

    void HighlightObject(int index)
    {
        // Déplacer l'objet dans le parent cible et le mettre en bas de la hiérarchie à chaque étape
        Transform objTransform = highlightObjects[index].transform;
        objTransform.SetParent(highlightParent, true); // true conserve la position mondiale
        objTransform.SetAsLastSibling(); // Le place en bas de la hiérarchie

        // Mettre à jour le texte du tutoriel
        tutorialText.text = tutorialTexts[index];
    }

    void RemoveHighlight(int index)
    {
        // Remettre l'objet dans son parent et son index original
        Transform objTransform = highlightObjects[index].transform;
        objTransform.SetParent(originalParents[index], true); // true conserve la position mondiale
        objTransform.SetSiblingIndex(originalSiblingIndexes[index]); // Rétablir l'index d'origine
    }

    void EndTutorial()
    {
        // Désactiver le panneau sombre et effacer le texte
        darkPanel.SetActive(false);
        tutorialText.text = "";
    }

    // Script pour gérer l'interaction avec chaque objet interactif
    public class TutorialObject : MonoBehaviour
    {
        public TutorialManager tutorialManager; // Référence au manager du tutoriel.

        // Appelé lorsque l'utilisateur clique sur cet objet
        void OnMouseDown()
        {
            // Vérifier si l'objet est celui surligné
            if (tutorialManager != null)
            {
                // Passer à l'étape suivante du tutoriel
                tutorialManager.MoveToNextStep();
            }
        }
    }
}
