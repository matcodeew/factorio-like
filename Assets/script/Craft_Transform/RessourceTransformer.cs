using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.ComponentModel;

public class RessourceTransformer : MonoBehaviour
{
    [Header("Machine data")]
    [SerializeField] private MachineType _machineType = MachineType.None;
    [Tooltip("List of Ressources that can be modified by the machine")]
    [SerializeField] private float _processTime;

    [SerializeField] private TransformationUI _ressourceUI;

    [HideInInspector] public Scriptable_Ressources MachineInput;
    [HideInInspector] public Scriptable_Ressources FirstOutput;
    [HideInInspector] public Scriptable_Ressources SecondOutput;
    [HideInInspector] public Scriptable_Ressources ThirdOutput;

    [SerializeField] private Transform _machineInput;
    private InvRessource _currentRessourceToTransform;
    [SerializeField] private Transform _parentSlot;
    private bool CreateCaseOneTime = true;
    [SerializeField] private List<GameObject> _instantiateOutputList = new();


    private bool _isInAction = false;
    public void SetTypeOfMachine(BuildingRessourceAccessor data)
    {
        _machineType = data.MachineType;
        _processTime = data.ProcessTime;
    }

    private void Update()
    {
        if (_machineInput.childCount > 0)
        {
            if (!_isInAction && MachineInputNotNull())
            {
                _isInAction = true;
                StartTransformation();
            }
        }
        else
        {
            if(CheckIfAllOutputCaseIsEmpty())
            {
                _isInAction = false;
                ActionWasCancelled = false;
            }
        }
    }

    private bool MachineInputNotNull()
    {
        MachineInput = _machineInput.GetComponentInChildren<InvRessource>().Ressource;

        if (MachineInput == null)
        {
            return false;
        }

        if (MachineInput.IsPure == false)
        {
            return true;
        }
        return false;
    }

    public bool ActionWasCancelled;
    public void StartTransformation()
    {
        _currentRessourceToTransform = _machineInput.GetComponentInChildren<InvRessource>();
        if (_currentRessourceToTransform != null)
        {
            StartCoroutine(HandleProcessTime());
        }
    }

    public void StopTransformation()
    {
        ActionWasCancelled = true;
        _isInAction = false;
    }

    private IEnumerator HandleProcessTime()
    {
        if (CheckIfAllOutputCaseIsEmpty())
        {
            while (_currentRessourceToTransform.Quantity > 0)
            {
                yield return new WaitForSeconds(_processTime);
                if (!ActionWasCancelled)
                {
                    if(_machineInput.childCount == 0)
                    {
                        ActionWasCancelled = true;
                        break;
                    }
                    transformRessource();
                    DisplayOutputPrefabs();
                }
                if(ActionWasCancelled) { break; }
                ActionWasCancelled = false;
            }
        }
    }

    private void transformRessource()
    {
        switch (_machineType)
        {
            case MachineType.Grinder:
                if (MachineInput.GrinderOutput != null)
                {
                    FirstOutput = MachineInput.GrinderOutput;
                    Debug.Log("Grinded " + MachineInput + " into " + FirstOutput);
                }
                break;

            case MachineType.Disassembler:
                if (MachineInput.DisassemblerOutputs.Count > 0)
                {
                    FirstOutput = MachineInput.DisassemblerOutputs[0];
                    SecondOutput = MachineInput.DisassemblerOutputs[1];
                    ThirdOutput = MachineInput.DisassemblerOutputs.Count == 3 ? MachineInput.DisassemblerOutputs[2] : null;
                    Debug.Log("Disassembled " + MachineInput + " into " + FirstOutput + ", " + SecondOutput + ", " + ThirdOutput);
                }
                break;

            case MachineType.Furnace:
                if (MachineInput.FurnaceOutput != null)
                {
                    FirstOutput = MachineInput.FurnaceOutput;
                    Debug.Log("Smelt " + MachineInput + " into " + FirstOutput);
                }
                break;

            case MachineType.None:
                Debug.Log("Machine type was not specified");
                break;

            default:
                Debug.Log("Problem in enum");
                break;
        }
        _currentRessourceToTransform.Quantity--;
        _isInAction = false;
    }

    private void DisplayOutputPrefabs()
    {
        if (_ressourceUI == null)
        {
            Debug.LogWarning("No UI assigned to display outputs.");
            return;
        }

        if(_currentRessourceToTransform.Quantity <= 0)
        {
            StopTransformation();
            DestroyAllChildren(_parentSlot);
            CheckIfAllOutputCaseIsEmpty();
        }
        if(_parentSlot.childCount < 0)
        {
            StopTransformation();
            DestroyAllChildren(_parentSlot);
            CheckIfAllOutputCaseIsEmpty();
        }

        if (FirstOutput != null)
        {
            CreateAndAlignOutputPrefab(FirstOutput, "InventorySlot");
        }

        if (SecondOutput != null)
        {
            CreateAndAlignOutputPrefab(SecondOutput, "InventorySlot");
        }

        if (ThirdOutput != null)
        {
            CreateAndAlignOutputPrefab(ThirdOutput, "InventorySlot");
        }

    }

    private void CreateAndAlignOutputPrefab(Scriptable_Ressources outputResource, string newTag) ///// si le panel est désactiver le script est
                                                                                                 ///// arrete de fonctionner et on ne peut pas relancer 
    {
        DeleteOutPutCase();
        bool itemFound = false;
        foreach (GameObject item in _instantiateOutputList)
        {
            InvRessource ressource = item.GetComponentInChildren<InvRessource>();
            if (ressource.Ressource.Id == outputResource.Id)
            {
                itemFound = true;
                ressource.Quantity++;
                break;
            }
        }
        if (!itemFound)
        {
            GameObject outputItem = Instantiate(InventoryPlayerManager.Instance._globalPrefab, _parentSlot);
            InvRessource invRessource = outputItem.transform.GetChild(0).AddComponent<InvRessource>();
            invRessource.Ressource = outputResource;
            invRessource.Quantity = 1;
            outputItem.transform.GetChild(0).GetComponent<Image>().sprite = outputResource.Sprite;
            _instantiateOutputList.Add(outputItem);
            outputItem.tag = newTag;
        }
    }
    private void DeleteOutPutCase() // si on met une ressource dans l'inventaire en même temps qu'une transfo est faite ya une erreur 
    {
        List<GameObject> CaseToRemove = new List<GameObject>();
        foreach(GameObject outputCase in _instantiateOutputList)
        {
            if(outputCase.tag == "Empty")
            {
                Destroy(outputCase);
                CaseToRemove.Add(outputCase);
            }
        }
        if (CaseToRemove.Count > 0)
        {
            foreach(GameObject outputCase in CaseToRemove)
            {
                _instantiateOutputList.Remove(outputCase);
            }
            CaseToRemove.Clear();
        }
    }
    private bool CheckIfAllOutputCaseIsEmpty()
    {
        DeleteOutPutCase();
        if (_instantiateOutputList.Count == 0)
        {
            return true;
        }
        return false;
    }

    private void DestroyAllChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
