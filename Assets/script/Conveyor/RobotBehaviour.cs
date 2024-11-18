using System.Collections;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [Header("Target Data")]
    private Vector3 _firstPos;
    private Vector3 _secondPos;
    private Vector3 _currentTarget;
    private bool _firstTargetChosen = true;
    private bool _secondTargetChosen = true;
    private RessourseSpot _ressourceSpot;
    private RessourceTransformer _ressourceTransformer;

    [Header("UI Elements")]
    [SerializeField] private GameObject _setFirstButton;
    [SerializeField] private GameObject _setSecondButton;
    [SerializeField] private GameObject _canvasPanel;

    [Header("Robot Behaviour")]
    [SerializeField] private float _moveSpeed = 2.0f;
    [SerializeField] private float _distanceThreshold = 0.1f;
    [SerializeField, Min(0.0f)] private float _waitingTime;
    private InvRessource _ressourceTransported;
    private bool _actionExecuted;
    private bool _robotIsMoving;
    private bool _ActionWasCanceled;
    private BuildingReceivedEnergy _recieveEnergy;

    [Header("Other")]
    [SerializeField] private GameObject _base;
    private RobotStation _dock;

    public void Init()
    {
        _ressourceTransported = this.gameObject.AddComponent<InvRessource>();
        _recieveEnergy = GetComponent<BuildingReceivedEnergy>();
        _dock = _base.GetComponent<RobotStation>();
    }
    private void Update()
    {
        if(_dock.BuildPosed)
        {
            HandleTargetSelection();
            if (!_recieveEnergy.IsPowered()) { return; }

            if (_robotIsMoving && _recieveEnergy.IsPowered())
            {
                StartCoroutine(MoveRobot());
            }
        }
    }

    public void SetFirstTargetMode() => _firstTargetChosen = false;

    public void SetSecondTargetMode() => _secondTargetChosen = false;

    private void HandleTargetSelection()
    {
        if (!_firstTargetChosen || !_secondTargetChosen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && Input.GetMouseButtonDown(0))
            {
                print(hit.collider.gameObject.name);
                if (!_firstTargetChosen)
                {
                    _ressourceSpot = hit.collider.GetComponent<RessourseSpot>();
                    if (_ressourceSpot != null)
                    {
                        _firstPos = SetTarget(hit.collider.gameObject, _setFirstButton, _setSecondButton);
                        _firstTargetChosen = true;
                    }
                    else
                    {
                        print($"{hit.collider.gameObject.name} is not a ressource spot");
                        ResetTargetSelection();
                    }
                }
                else if (!_secondTargetChosen)
                {
                    _ressourceTransformer = hit.collider.GetComponent<RessourceTransformer>();
                    if (_ressourceTransformer != null)
                    {
                        _secondPos = SetTarget(hit.collider.gameObject, _setSecondButton, null);
                        _secondTargetChosen = true;

                        StartRobotMovement();
                    }
                    else
                    {
                        print($"{hit.collider.gameObject.name} is not a ressource transformer");
                        ResetTargetSelection();
                    }
                }
            }
        }
    }

    private Vector3 SetTarget(GameObject target, GameObject currentButton, GameObject nextButton)
    {
        if (target != null)
        {
            currentButton?.SetActive(false);
            nextButton?.SetActive(true);
            return target.transform.position;
        }

        ResetTargetSelection();
        return Vector3.zero;
    }

    private void StartRobotMovement()
    {
        _robotIsMoving = true;
        _currentTarget = _firstPos;
    }

    private IEnumerator MoveRobot()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget + new Vector3(0, 1, 0), _moveSpeed * Time.deltaTime);
        if (_ActionWasCanceled)
        {
            _currentTarget = _base.transform.position;
            yield return new WaitForSeconds(1.0f);
            TryToPoseRessourceAgain();
        }
        else
        {
            if (Vector3.Distance(transform.position, _currentTarget) < _distanceThreshold)
            {
                _actionExecuted = false;
                if (_currentTarget == _firstPos)
                {
                    yield return new WaitForSeconds(_waitingTime);
                    _currentTarget = _secondPos;
                    if (!_actionExecuted)
                    {
                        TakeRessource();
                    }
                }
                else if (_currentTarget == _secondPos)
                {
                    yield return new WaitForSeconds(_waitingTime);
                    _currentTarget = _firstPos;
                    if (!_actionExecuted)
                    {
                        PoseRessource();
                    }
                }
                else if (_currentTarget == _base.transform.position)
                {
                    _currentTarget = _secondPos;
                }
            }
        }
    }

    private void TakeRessource()
    {
        if (_ressourceSpot != null && !_ressourceSpot.IsEmpty)
        {
            _ressourceTransported.Ressource = _ressourceSpot.AvailableResource[_ressourceSpot.PickRandomRessource()].Ressources;
            _ressourceTransported.Quantity = 1;

            print($"take ressource {_ressourceTransported.Ressource.Name} on quantity {_ressourceTransported.Quantity}");
        }
        _actionExecuted = true;
    }
    private void PoseRessource()
    {
        if (_ressourceSpot != null && _ressourceTransformer != null)
        {
            if (_ressourceTransformer.CheckIfInputCaseIsEmpty())
            {
                _ressourceTransformer.CreateInputCase(_ressourceTransported);
                print($"pose ressource {_ressourceTransported.Ressource.Name} on quantity {_ressourceTransported.Quantity}");
                _ressourceTransported.Ressource = null;

            }
            else
            {
                if (_ressourceTransformer.CheckIfCanStackRessource(_ressourceTransported))
                {
                    _ressourceTransformer.AddInputRessource(_ressourceTransported);
                    _ressourceTransported.Ressource = null;
                }
                else
                {
                    CanceledAction();
                    print($"transformers already has an input resource => {_ressourceTransformer.MachineInput}");
                }
            }
        }
        _actionExecuted = true;
    }
    public void TryToPoseRessourceAgain()
    {
        if (_ActionWasCanceled)
        {
            if (_ressourceTransformer.CheckIfInputCaseIsEmpty())
            {
                _ActionWasCanceled = false;
                StartCoroutine(MoveRobot());
            }
        }
    }
    private void CanceledAction()
    {
        _ActionWasCanceled = true;
        StopAllCoroutines();
        StartCoroutine(MoveRobot());
    }
    private void StartAction() => _ActionWasCanceled = false;

    private void ResetTargetSelection()
    {
        _firstTargetChosen = true;
        _secondTargetChosen = true;
        _setFirstButton.SetActive(true);
        _setSecondButton.SetActive(false);
        _canvasPanel.SetActive(false);
    }
}
