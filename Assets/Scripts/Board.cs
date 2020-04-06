using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public int xSize;
    public int ySize;
    public Tilemap tilemap;
    int xScale = 1;
    int yScale = 1;

    // stores the IDs of each item on a given x,y coordinate
    LinkedList<GamePiece>[,] tiles;

    // Start is called before the first frame update
    void Start() {
        tiles = new LinkedList<GamePiece>[xSize,ySize];

        TileBase dark = tilemap.GetTile(new Vector3Int(0, 0, 0));
        TileBase light = tilemap.GetTile(new Vector3Int(1, 0, 0));

        Vector3Int[] positions = new Vector3Int[xSize * ySize];
        TileBase[] tileArray = new TileBase[xSize * ySize];
    
        int i=0;
        for (int x=0; x<xSize; x++) {
            for (int y=0; y<ySize; y++) {
                tiles[x,y] = new LinkedList<GamePiece>();

                positions[i] = new Vector3Int(x, y, 0);
                if ((x+y) % 2 == 0) {
                    tileArray[i] = light;
                } else {
                    tileArray[i] = dark;
                }
                i++;
            }
        }

        tilemap.ClearAllTiles();
        tilemap.SetTiles(positions, tileArray); 

        Refresh();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public Point PosToCoord(Point pos) {
        return new Point(pos.X / xScale, pos.Y / yScale);
    }

    public Point CoordToPos(Point coord) {
        return new Point(coord.X * xScale, coord.Y * yScale);
    }

    public void Register(GamePiece gp, Point coord) {
        print(tiles[0,0]);
        print(coord.X);
        print(tiles[coord.X, coord.Y]);
        tiles[coord.X, coord.Y].AddFirst(gp);
    }

    public Point MovePiece(Point oldCoord, Point newCoord, GamePiece piece) {
        if (newCoord.X >= xSize || newCoord.Y >= ySize || newCoord.X < 0 || newCoord.Y < 0) {
            return oldCoord;
        }
        tiles[oldCoord.X, oldCoord.Y].Remove(piece);
        tiles[newCoord.X, newCoord.Y].AddFirst(piece);
        return newCoord;
    }

    void Refresh() {
        for (int x=0; x<xSize; x++) {
            for (int y=0; y<ySize; y++) {
                foreach (GamePiece gp in tiles[x,y]) {
                    gp.SetPosition(new Point(x, y));
                }
            }
        }
    }
}
