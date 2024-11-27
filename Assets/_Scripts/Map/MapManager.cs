using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public Map mapData; // The map data
    public GameObject nodePrefab; // Prefab for a map node
    public Transform mapParent; // Parent transform for organizing nodes

    private List<GameObject> nodeObjects = new List<GameObject>();
    private int currentNodeIndex = 0;



    void Start()
    {
        GenerateMap();
        HighlightCurrentNode();
    }


    void GenerateMap()
    {
        foreach (var node in mapData.nodes)
        {
            // Instantiate a node at the given position
            GameObject nodeObject = Instantiate(nodePrefab, node.position, Quaternion.identity, mapParent);
            nodeObjects.Add(nodeObject);
        }

        // Draw lines between connected nodes
        for (int i = 0; i < mapData.nodes.Count; i++)
        {
            foreach (int connectedIndex in mapData.nodes[i].connectedNodes)
            {
                DrawConnection(nodeObjects[i].transform.position, nodeObjects[connectedIndex].transform.position);
            }
        }
    }

    void DrawConnection(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Connection");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
    }

    void HighlightCurrentNode()
    {
        // Highlight the current node visually
        if (currentNodeIndex < nodeObjects.Count)
        {
            var currentNode = nodeObjects[currentNodeIndex];
            currentNode.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public void MoveToNode(int nodeIndex)
    {
        if (mapData.nodes[currentNodeIndex].connectedNodes.Contains(nodeIndex))
        {
            currentNodeIndex = nodeIndex;
            HighlightCurrentNode();

            string nodeType = mapData.nodes[nodeIndex].nodeType;
            LoadRoom(nodeType);
        }
    }

    void LoadRoom(string roomType)
    {
        switch (roomType)
        {
            case "Combat":
                // Load combat scene
                SceneManager.LoadScene("CombatScene");
                break;
            case "Treasure":
                // Load treasure scene
                SceneManager.LoadScene("TreasureScene");
                break;
            default:
                Debug.LogWarning("Unknown room type!");
                break;
        }
    }
}
