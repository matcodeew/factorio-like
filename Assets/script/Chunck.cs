using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunck // : MonoBehaviour
{
    public Dictionary<Vector3, TileData> TileChunk = new Dictionary<Vector3, TileData>();
    public ChunkState state = ChunkState.None;

}
public enum ChunkState
{
    IndustrialZone,
    UrbanZone,
    Forest,
    Career,
    None,
}
    