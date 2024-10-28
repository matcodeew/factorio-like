using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRessource : MonoBehaviour
{
    public List<Scriptable_RessourceSpot> TilesWithSpot = new();
    private void CheckRessourceOnClick(Vector3 _clikedTarget)
    {
        foreach(var tile in TilesWithSpot)
        {
            if(MapManager.Instance.AccessTileByPos(_clikedTarget) == tile)
            {
                StartCoroutine(StillRessource(tile));
                break;
            }
        }
    }

    private IEnumerator StillRessource(Scriptable_RessourceSpot _ressourceSpot)
    {
        yield return new WaitForSeconds(_ressourceSpot.MiningTime);
        //Player.Inventaire.AddRessource(_ressourcespot.AvailableResource[Random.Range(0, _ressourcespot.AvailableResource.Count)];l
    }
}