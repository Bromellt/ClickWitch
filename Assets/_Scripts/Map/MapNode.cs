using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNode
{
    public string nodeType; // e.g., "Combat", "Treasure", etc.
    public Vector2 position; // Position on the map
    public List<int> connectedNodes; // Indices of connected nodes

}
