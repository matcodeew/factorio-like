using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class RessourceTransformer : MonoBehaviour
{
    public enum MachineType
    {
        Grinder,
        Disassembler,
        Furnace,
        None
    }

    [Header("Machine data")]
    [SerializeField] private MachineType _machineType;
    [Tooltip("List of Ressources that can be modified by the machine")]
    [SerializeField] private Scriptable_RessourceList _transformationListHolder;
    private List<Scriptable_Ressources> _transformationList;
    [SerializeField] private float _processTime;

    [SerializeField] private TransformationUI _ressourceUI;

    [HideInInspector] public Scriptable_Ressources MachineInput;
    [HideInInspector] public Scriptable_Ressources FirstOutput;
    [HideInInspector] public Scriptable_Ressources SecondOutput;
    [HideInInspector] public Scriptable_Ressources ThirdOutput;

    [SerializeField] private Transform _machineInput;
    [SerializeField] private Transform _parentSlot;

    public bool ActionWasCancelled;

    void Start()
    {
        _transformationList = _transformationListHolder.RessourceList;

        if (_transformationList.Count != 0)
        {
            MachineInput = _transformationList[UnityEngine.Random.Range(0, _transformationList.Count - 1)];
            StartTransformation();
        }
    }

    public void StartTransformation()
    {
        FirstOutput = null;
        SecondOutput = null;
        ThirdOutput = null;

        StartCoroutine(HandleProcessTime());
    }

    public void StopTransformation()
    {
        ActionWasCancelled = true;
    }

    private IEnumerator HandleProcessTime()
    {
        yield return new WaitForSeconds(_processTime);
        if (!ActionWasCancelled)
        {
            transformRessource();
            DisplayOutputPrefabs(); 
        }

        ActionWasCancelled = false;
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
        MachineInput = null;
    }

    private void DisplayOutputPrefabs()
    {
        if (_ressourceUI == null)
        {
            Debug.LogWarning("No UI assigned to display outputs.");
            return;
        }
        if (FirstOutput != null)
            CreateAndAlignOutputPrefab(FirstOutput , "InventorySlot"); 

        if (SecondOutput != null)
            CreateAndAlignOutputPrefab(SecondOutput, "InventorySlot");

        if (ThirdOutput != null)
            CreateAndAlignOutputPrefab(ThirdOutput, "InventorySlot");
    }

    private void CreateAndAlignOutputPrefab(Scriptable_Ressources outputResource, string newTag)
    {
        DestroyAllChildren(_machineInput);    
        _machineInput.tag = "Empty";
        GameObject outputItem = Instantiate(InventoryPlayerManager.Instance._EmptyPrefab, _parentSlot);
        InvRessource invRessource = outputItem.AddComponent<InvRessource>();
        invRessource.Ressource = outputResource;
        invRessource.Quantity = 1;
        outputItem.tag = newTag;

        GameObject ImageRessource = new GameObject();
        ImageRessource.transform.position = outputItem.transform.position;  
        ImageRessource.transform.parent = outputItem.transform;
        //ImageRessource.AddComponent<Image>().sprite = invRessource.Ressource.Sprite;
        ImageRessource.AddComponent<DragUiElementInventory>();
        ImageRessource.AddComponent<Image>().color = Color.red; // a changer

        //if(outputItem.tag == "Empty")
        //{
        //    Destroy(outputItem);
        //}
    }

    private void DestroyAllChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
