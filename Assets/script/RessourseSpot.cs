using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourseSpot : MonoBehaviour
{
    private void InstantiateGameObject(Scriptable_RessourceSpot data)
    {
        if (data.Prefab == null)
        {
            Debug.LogWarning("Prefab is missing");
            return;
        }

        GameObject newObject = Instantiate(data.Prefab);
        newObject.transform.position = MapManager.Instance.AccessTileByPos(new Vector3(Random.Range(0, 32), 1, Random.Range(0, 32))).transform.position;
        newObject.name = data.Name;
    }
}