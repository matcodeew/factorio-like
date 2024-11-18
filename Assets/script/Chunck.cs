using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk // : MonoBehaviour
{
    public Dictionary<Vector3, TileData> Tiles = new Dictionary<Vector3, TileData>();
    public float ChunckDecontaminationValue; // value that vary between 0.0 and 1.0

    public float UpdateChunkDecontaminationValue()
    {
        ChunckDecontaminationValue = Tiles.Values.Average(tile => tile.PurifiedValue);
        return ChunckDecontaminationValue;
    }
}
