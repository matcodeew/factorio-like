using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestinationChecker : MonoBehaviour
{
    [SerializeField] private GameObject _endCirclePanel;
    public Transform target;
    public float arrivalThreshold = 1f;

    void Update()
    {
        if (Menu.Finish == true)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < arrivalThreshold)
            {
                StartCoroutine(EndCircle());
            }
        }
    }

    public IEnumerator EndCircle()
    {
        _endCirclePanel.SetActive(true);
        yield return new WaitForSeconds(1.95f);
        SceneManager.LoadScene("StartEndGame");
    }
}
