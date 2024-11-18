using UnityEngine;

public class Anim : MonoBehaviour
{
    [SerializeField] private GameObject _starShip;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _generator;
    [SerializeField] private GameObject _panelNoInteract;
    [SerializeField] private GameObject _panelCircle;

    private void Start()
    {
        _panelNoInteract.SetActive(true);
    }
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
            newGo.transform.position = new Vector3(15, 1, 15);
            newGo.GetComponent<ISubscrireEvent>().SubscrireEvent();
            _player.SetActive(true);
        }
    }

    public void SetActiveFalseAnimator()
    {
        if (_player != null)
        {
            _panelCircle.SetActive(false);
            _panelNoInteract.SetActive(false);
            _player.GetComponent<Animator>().enabled = false;
        }
    }

    public void DisablePlayer()
    {
        _player.SetActive(false);
    }
}
