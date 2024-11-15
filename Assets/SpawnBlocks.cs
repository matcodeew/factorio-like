using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{

    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private int _mapSize = 4;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < _mapSize; x++)
            for (int y = 0; y < _mapSize; y++)
                Instantiate(_blockPrefab, new Vector3(x, 0, y), Quaternion.identity);
    }
}
