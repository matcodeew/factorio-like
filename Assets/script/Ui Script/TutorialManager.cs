using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject panel;
    public TextMeshProUGUI tutorialText;
    public GameObject[] tutorialObjects;
    public string[] tutorialMessages;
    public float highlightDuration = 3f;

    private int currentIndex = 0;
    private Color[] originalColors;
    private Vector3[] originalPositions;
    private Transform[] originalParents;
    private Coroutine tutorialCoroutine;

    void Awake()
    {      
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int objectCount = tutorialObjects.Length;
        originalColors = new Color[objectCount];
        originalPositions = new Vector3[objectCount];
        originalParents = new Transform[objectCount];

        for (int i = 0; i < objectCount; i++)
        {
            Renderer renderer = tutorialObjects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                originalColors[i] = renderer.material.color;
            }
            originalPositions[i] = tutorialObjects[i].transform.position;
            originalParents[i] = tutorialObjects[i].transform.parent;
        }

        panel.SetActive(false);
        tutorialText.gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        if (tutorialCoroutine == null) 
        {
            tutorialCoroutine = StartCoroutine(ShowTutorial());
        }
    }

    IEnumerator ShowTutorial()
    {
        panel.SetActive(true);
        tutorialText.gameObject.SetActive(true);

        while (currentIndex < tutorialObjects.Length)
        {
            HighlightObject(currentIndex);
            tutorialText.text = tutorialMessages[currentIndex];
            yield return new WaitForSeconds(highlightDuration);
            RemoveHighlight();
            currentIndex++;
        }
        panel.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        tutorialCoroutine = null;
    }

    void HighlightObject(int index)
    {
        if (tutorialObjects[index] != null)
        {
            GameObject obj = tutorialObjects[index];
            Renderer renderer = obj.GetComponent<Renderer>();

            obj.SetActive(true);
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
            obj.transform.SetParent(panel.transform, true);
        }
    }

    void RemoveHighlight()
    {
        if (tutorialObjects[currentIndex] != null)
        {
            GameObject obj = tutorialObjects[currentIndex];
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = originalColors[currentIndex];
            }

            obj.transform.SetParent(originalParents[currentIndex], true);
            obj.transform.position = originalPositions[currentIndex];

            obj.SetActive(false);
        }
    }

    // Function to skip the tutorial
    public void SkipTutorial()
    {
        if (tutorialCoroutine != null)
        {
            StopCoroutine(tutorialCoroutine);  // Stop the tutorial coroutine
        }

        // Immediately hide the tutorial UI elements
        panel.SetActive(false);
        tutorialText.gameObject.SetActive(false);

        // Optionally reset the tutorial index and other states if you want to restart later
        currentIndex = tutorialObjects.Length;
    }
}
