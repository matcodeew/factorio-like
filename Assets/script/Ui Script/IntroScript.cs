using System.Collections;
using TMPro;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    [SerializeField] private GameObject _PanelCircleStart;
    public GameObject textObject;
    TextMeshProUGUI uiText;
    string originalText;
    public static bool _finish { get; set; }

    public float delay = 0.2f;

    void Awake()
    {
        uiText = textObject.GetComponent<TextMeshProUGUI>();
        originalText = uiText.text;
        uiText.text = null;
        StartCoroutine(ShowLetterByLetter());
    }

    IEnumerator ShowLetterByLetter()
    {
        yield return new WaitForSeconds(1.75f);
        _PanelCircleStart.SetActive(false);
        for (int i = 0; i <= originalText.Length; i++)
        {
            uiText.text = originalText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
