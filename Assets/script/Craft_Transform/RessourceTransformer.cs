using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

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
    [SerializeField] private List<Scriptable_Ressources> _transformationList;
    [SerializeField] private float _processTime;

    private Scriptable_Ressources _input;
    private Scriptable_Ressources _firstOutput;
    private Scriptable_Ressources _secondOutput;
    private Scriptable_Ressources _thirdOutput;


    public bool ActionWasCancelled;


    public void StartTransformation()
    {
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
                    _firstOutput = _input.GrinderOutput;
                    Debug.Log("Grinded");
                    break;
                }
            case MachineType.Disassembler:
                {
                    // Disassembler is a 1 to 3 machine (it means 1 input, 3 output maximum)
                    _firstOutput = _input.DisassemblerOutputs[0];
                    _secondOutput = _input.DisassemblerOutputs[1];
                    _thirdOutput = _input.DisassemblerOutputs[2];
                    Debug.Log("Disassembled");
                    break;
                }
            case MachineType.Furnace:
                {
                    Debug.Log("Smelt");
                    // Furnace is a 1 to 1 machine (it means 1 input, 1 output)
                    _firstOutput = _input.FurnaceOutput;

                    break;
                }
            case MachineType.None:

                {
                    Debug.Log("Machine type was not specified");
                    break;
                }
            default:
                {
                    Debug.Log("Problem in enum");
                    break;
                }
        }
    }
}
