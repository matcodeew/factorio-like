using UnityEngine;

public class Anim : MonoBehaviour
{
    [SerializeField] private GameObject _starShip;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _generator;
    [SerializeField] private GameObject _chest;

    public void DestroyShip()
    {
        if (_starShip != null)
        {
            Destroy(_starShip);
        }
    }

    public void CreatePlayer()
    {
        if (_player != null)
        {
            GameObject newGo = Instantiate(_generator);
            newGo.transform.position = new Vector3(15, 2, 15);
            GameObject go = Instantiate(_chest);
            go.transform.position = new Vector3(10, 2, 10);
            _player.SetActive(true);
        }
    }

    public void SetActiveFalseAnimator()
    {
        if (_player != null)
        {
            _player.GetComponent<Animator>().enabled = false;
        }
    }
}
