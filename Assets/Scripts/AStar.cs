using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public struct TileData {
    public int x, y, cost;
}

public class TileNode {
    public int index, f, g, h;
    public TileNode parent;
}

public class AStar {
    private TileData[] tileData;

    public AStar(TileData[] tileData) {
        this.tileData = tileData;
    }

    private TileNode ToNode(Vector2Int pos) {
        int index = Array.FindIndex(tileData, (t) => t.x == pos.x && t.y == pos.y);
        return new TileNode() { index = index };
    }
    private int GetDistance(TileNode nodeA, TileNode nodeB) {
        int dstX = Mathf.Abs(tileData[nodeA.index].x - tileData[nodeB.index].x);
        int dstY = Mathf.Abs(tileData[nodeA.index].y - tileData[nodeB.index].y);
        return (dstX > dstY) ?
            14 * dstY + 10 * (dstX - dstY) :
            14 * dstX + 10 * (dstY - dstX);
    }

    private TileNode FindLowestF(List<TileNode> openList) {
        List<TileNode> list = new List<TileNode>(openList);
        list.Sort((a, b) => a.f - b.f);
        return list[0];
    }

    private bool ExistsInList(List<TileNode> list, TileNode node) {
        return list.Exists((n) => n.index == node.index);
    }

    public List<Vector2Int> Path(Vector2Int from, Vector2Int to) {
        List<TileNode> openList = new List<TileNode>();
        List<TileNode> closedList = new List<TileNode>();

        openList.Add(ToNode(from));
        TileNode endNode = ToNode(to);

        while (openList.Count > 0) {
            TileNode currentNode = FindLowestF(openList);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // found the goal
            if (currentNode.index == endNode.index) {
                List<Vector2Int> path = new List<Vector2Int>();
                TileNode parent = currentNode.parent;
                path.Add(to);

                while (parent != null) {
                    TileData data = tileData[parent.index];
                    path.Add(new Vector2Int(data.x, data.y));
                    parent = parent.parent;
                }

                path.Reverse();
                return path;
            }

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    bool notEdge = x == 0 || y == 0;

                    if (notEdge) {
                        TileData currentNodeData = tileData[currentNode.index];
                        TileNode childNode = ToNode(new Vector2Int(currentNodeData.x + x, currentNodeData.y + y));

                        if (childNode.index == -1 || tileData[childNode.index].cost == 0 || ExistsInList(closedList, childNode)) {
                            continue;
                        }

                        childNode.parent = currentNode;
                        childNode.g = currentNode.g + 1;
                        childNode.h = GetDistance(childNode, endNode);
                        childNode.f = childNode.g + childNode.h;

                        if (ExistsInList(openList, childNode)) {
                            TileNode openNode = openList.Find((n) => n.index == childNode.index);
                            if (childNode.g > openNode.g) continue;
                        }

                        openList.Add(childNode);
                    }
                }
            }
        }

        return null;
    }
}
