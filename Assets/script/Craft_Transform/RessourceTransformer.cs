using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.ComponentModel;
using static UnityEditor.Progress;

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
    [SerializeField] private List<GameObject> _instantiateOutputList = new();


    private bool _isInAction = false;

    private void Update()
    {
        if (!CheckIfInputCaseIsEmpty())
        {
            if (!_isInAction && MachineInputNotNull())
            {
                _isInAction = true;
                StartTransformation();
            }
        }
        else if(_instantiateOutputList.Count >= 1)
        {
            MoveOutputInInventory();

        }
    }

    public void MoveOutputInInventory()
    {
        foreach (GameObject outputCase in _instantiateOutputList)
        {
            InvRessource ressource = outputCase.GetComponentInChildren<InvRessource>();
            InventoryPlayerManager.Instance.CreateNewInventorySlot(new RessourceData(ressource.Ressource.Id, ressource.Ressource, ressource.Quantity));
            DestroyAllChildren(outputCase.transform);
            outputCase.tag = "Empty";
            DeleteOutPutCase(outputCase);
        }
        _instantiateOutputList.Clear();
        _isInAction = false;
        ActionWasCancelled = false;
    }

    private bool MachineInputNotNull()
    {
        MachineInput = _machineInput.GetComponentInChildren<InvRessource>().Ressource;
        InventoryPlayerManager.Instance.SlotGameobjectList.Remove(_machineInput.GetComponentInChildren<InvRessource>());
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
                    //if (_machineInput.childCount == 0)
                    //{
                    //    ActionWasCancelled = true;
                    //    break;
                    //}
                    transformRessource();
                    DisplayOutputPrefabs();
                }
                if (ActionWasCancelled) { break; }
                ActionWasCancelled = false;
            }
            ActionWasCancelled = false;
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

        if (_currentRessourceToTransform.Quantity == 0)
        {
            StopTransformation();
            DestroyAllChildren(_machineInput);
        }
        if (CheckIfInputCaseIsEmpty())
        {
            StopTransformation();
            DestroyAllChildren(_machineInput);
        }

        if (FirstOutput != null)
        {
            CreateAndAlignOutputPrefab(FirstOutput, "OutputSlot");
        }

        if (SecondOutput != null)
        {
            CreateAndAlignOutputPrefab(SecondOutput, "OutputSlot");
        }

        if (ThirdOutput != null)
        {
            CreateAndAlignOutputPrefab(ThirdOutput, "OutputSlot");
        }

    }
    public void CreateInputCase(InvRessource inputRessource)
    {
        Transform newImage = Instantiate(InventoryPlayerManager.Instance._globalPrefab.transform.GetChild(0), _machineInput.transform);
        InvRessource invRessource = newImage.AddComponent<InvRessource>();
        invRessource.Ressource = inputRessource.Ressource;
        invRessource.Quantity = inputRessource.Quantity;
        newImage.GetComponent<Image>().sprite = inputRessource.Ressource.Sprite;
        _machineInput.tag = "InventorySlot";
        MachineInput = invRessource.Ressource;
    }

    private void CreateAndAlignOutputPrefab(Scriptable_Ressources outputResource, string newTag)
    {
        bool itemFound = false;
        foreach (GameObject item in _instantiateOutputList)
        {
            if(item.tag == "Empty")
            { 
                itemFound = true;
                Transform newImage = Instantiate(InventoryPlayerManager.Instance._globalPrefab.transform.GetChild(0), item.transform);
                InvRessource invRessource = newImage.AddComponent<InvRessource>();
                invRessource.Ressource = outputResource;
                invRessource.Quantity = 1;
                newImage.GetComponent<Image>().sprite = outputResource.Sprite;
                item.tag = newTag;
                break;
            }

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
            outputItem.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    private void DeleteOutPutCase(GameObject outputCase)
    {
        if (outputCase.tag == "Empty")
        {
            Destroy(outputCase);
        }
    }
    public bool CheckIfInputCaseIsEmpty() => _machineInput.childCount <= 0? true: false;
    private bool CheckIfAllOutputCaseIsEmpty() => _instantiateOutputList.Count == 0? true: false;
    private void DestroyAllChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
