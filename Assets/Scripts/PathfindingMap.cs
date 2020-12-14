using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PathfindingMap : MonoBehaviour {
    public static PathfindingMap Instance { get; private set; }

    public List<TileData> grid;
    private Tilemap floor, walls;
    private Door[] doors;
    private HiddenDoor[] hiddenDoors;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        floor = transform.Find("Floor").GetComponent<Tilemap>();
        walls = transform.Find("Walls").GetComponent<Tilemap>();

        doors = FindObjectsOfType<Door>();
        hiddenDoors = FindObjectsOfType<HiddenDoor>();

        Recalculate();
    }

    public bool CanMove(Vector3 pos) {
        Vector3Int gridPos = floor.WorldToCell(pos);
        if (!floor.HasTile(gridPos)) return false;

        if (FindObjectsOfType<Boulder>().Any((b) => floor.WorldToCell(b.transform.position) == gridPos)) return false;
        if (doors.Any((d) => floor.WorldToCell(d.transform.position) == gridPos && d.IsClosed())) return false;
        if (hiddenDoors.Any(d => {
            BoxCollider2D collider = d.GetComponent<BoxCollider2D>();
            return d.IsClosed() && collider.OverlapPoint(new Vector2(gridPos.x, gridPos.y));
        })) return false;

        return !walls.HasTile(gridPos);
    }

    public void Recalculate() {
        grid = new List<TileData>();
        for (int x = floor.cellBounds.min.x; x < floor.cellBounds.max.x; x++) {
            for (int y = floor.cellBounds.min.y; y < floor.cellBounds.max.y; y++) {
                grid.Add(new TileData() {
                    x = x,
                    y = y,
                    cost = CanMove(new Vector3Int(x, y, 0)) ? 1 : 0
                });
            }
        }

    }
}
