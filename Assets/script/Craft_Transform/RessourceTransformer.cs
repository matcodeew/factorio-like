using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

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
    [SerializeField] MachineType _machineType;
    [Tooltip("List of Ressources that can be modified by the machine")]
    [SerializeField] private Scriptable_RessourceList _transformationListHolder;
    private List<Scriptable_Ressources> _transformationList;
    [SerializeField] private float _processTime;

    [HideInInspector] public Scriptable_Ressources MachineInput;
    [HideInInspector] public Scriptable_Ressources FirstOutput;
    [HideInInspector] public Scriptable_Ressources SecondOutput;
    [HideInInspector] public Scriptable_Ressources ThirdOutput;


    [SerializeField] private TransformationUI _ressourceUI;


    public bool ActionWasCancelled;

    void Start()
    {
        _transformationList = _transformationListHolder.RessourceList;
        // _transformationList = FilterListByMachineType();

        if (_transformationList.Count != 0)
        {
            MachineInput = _transformationList[UnityEngine.Random.Range(0, _transformationList.Count - 1)];
            StartTransformation();
        }
    }

    // List<Scriptable_Ressources> FilterListByMachineType()
    // {
    //     bool isGrinder = _machineType == MachineType.Grinder;
    //     bool isList = _machineType == MachineType.Disassembler;

    //     return _transformationList.Where
    //     (x =>
    //         (
    //             isList ?
    //                 x.DisassemblerOutputs.Count > 0 :
    //                 (
    //                     isGrinder ?
    //                         x.GrinderOutput != null :
    //                         x.FurnaceOutput != null
    //                 )
    //         )
    //     ).ToList();
    // }

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
            transformRessource();

        ActionWasCancelled = false;
        yield return null;
    }

    private void transformRessource()
    {
        switch (_machineType)
        {
            case MachineType.Grinder:
                {
                    // Grinder is a 1 to 1 machine (it means 1 input, 1 output)
                    if (MachineInput.GrinderOutput != null)
                    {
                        FirstOutput = MachineInput.GrinderOutput;
                        Debug.Log("Grinded " + MachineInput + " into " + FirstOutput);
                    }
                    break;
                }
            case MachineType.Disassembler:
                {
                    // Disassembler is a 1 to 3 machine (it means 1 input, 3 output maximum)
                    FirstOutput = MachineInput.DisassemblerOutputs[0];
                    SecondOutput = MachineInput.DisassemblerOutputs[1];
                    ThirdOutput = MachineInput.DisassemblerOutputs.Count == 3 ? MachineInput.DisassemblerOutputs[2] : null;
                    Debug.Log("Disassembled  " + MachineInput + " into " + FirstOutput + ", " + SecondOutput + ", " + ThirdOutput);
                    break;
                }
            case MachineType.Furnace:
                {
                    // Furnace is a 1 to 1 machine (it means 1 input, 1 output)
                    FirstOutput = MachineInput.FurnaceOutput;
                    Debug.Log("Smelt " + MachineInput + " into " + FirstOutput);
                    break;
                }
            case MachineType.None:
                {
                    Debug.Log("Machine type was not specified");
                    return;
                }
            default:
                {
                    Debug.Log("Problem in enum");
                    return;
                }
        }
        MachineInput = null;
    }

    private void UpdateMachineUI()
    {
        // this method will update the ui so the player can interract with it 
    }
}
