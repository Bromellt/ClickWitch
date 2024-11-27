using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "Map System/Map")]
public class Map : ScriptableObject
{
    public List<MapNode> nodes;
}
