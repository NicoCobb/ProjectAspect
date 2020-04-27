using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public int xSize;
    public int ySize;
    public Tilemap tilemap;
    public GameObject kingPrefab;
    public GameObject enemyPrefab;
    public UnityHttpClient network;
    int xScale = 1;
    int yScale = 1;
    private const int movesPerTurn = 5;

    GamePiece king;
    GamePiece enemy;

    // stores the IDs of each item on a given x,y coordinate
    List<GamePiece>[,] tiles;
    List<GamePiece> pieces;

    // Start is called before the first frame update
    void Start() {
        tiles = new List<GamePiece>[xSize,ySize];
        pieces = new List<GamePiece>();

        TileBase dark = tilemap.GetTile(new Vector3Int(0, 0, 0));
        TileBase light = tilemap.GetTile(new Vector3Int(1, 0, 0));

        Vector3Int[] positions = new Vector3Int[xSize * ySize];
        TileBase[] tileArray = new TileBase[xSize * ySize];
    
        int i=0;
        for (int x=0; x<xSize; x++) {
            for (int y=0; y<ySize; y++) {
                tiles[x,y] = new List<GamePiece>();

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

        populateBoard();
        Refresh();
    }

    public void populateBoard() {
        GameObject player = Instantiate(kingPrefab, transform.position, transform.rotation);
        GameObject twitch = Instantiate(enemyPrefab, transform.position, transform.rotation);

        king = player.GetComponent<GamePiece>();
        king.board = this;
        king.SetPosition(new Point(0,0));

        enemy = twitch.GetComponent<GamePiece>();
        enemy.board = this;
        enemy.SetPosition(new Point(9,9));

        Register(king, king.data.coord);
        Register(enemy, enemy.data.coord);
        network.sendMinimapArray(SerializeBoard());
    }


    public void populateTwitchMoves() {
        print(UnityHttpListener.TwitchMoves);
        print(enemy);
        enemy.moveList = UnityHttpListener.TwitchMoves;
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
        tiles[coord.X, coord.Y].Add(gp);
        pieces.Add(gp);
    }

    public Point MovePiece(Point oldCoord, Point newCoord, GamePiece piece) {
        if (newCoord.X >= xSize || newCoord.Y >= ySize || newCoord.X < 0 || newCoord.Y < 0) {
            return oldCoord;
        }
        tiles[oldCoord.X, oldCoord.Y].Remove(piece);
        tiles[newCoord.X, newCoord.Y].Add(piece);
        return newCoord;
    }

    private string SerializePiece(GamePiece gp) {
        string type = "0";
        string id = "";
        if (gp != null) {
            // TODO when ally types are in
            // if (typeof(gp) == typeof(ally)) type = "1";
            if (gp.GetType() == enemy.GetType()) type = "2";
            if (gp.GetType() == king.GetType()) type = "3";
            id = gp.data.id;
        }
        return "{\"type\": " + type +  ", \"id\": \"" + id + "\"}";
    }

    public string SerializeBoard() {
        string str = "[";
        for (int x=0; x<xSize; x++) {
            str += "[";
            for (int y=0; y<ySize; y++) {
                str += "[";
                if (tiles[x,y].Count == 0) {
                    str += SerializePiece(null);
                }
                for (int i=0; i<tiles[x,y].Count; i++) {
                    str += SerializePiece(tiles[x,y][i]);
                    if (i != tiles[x,y].Count -1) {
                        str += ",";
                    }
                }
                str += "]";
                if (y != ySize -1) {
                    str += ",";
                }
            }
            str += "]";
            if (x != xSize -1) {
                str += ",";
            }
        }
        return str + "]";
    }

    public IEnumerator StartTurn() {
        for(int i = 0; i < movesPerTurn; i++) {
            populateTwitchMoves();
            movePieces();
            checkCombat();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void movePieces() {
        foreach (GamePiece gp in pieces) {
            gp.MoveStep();
        }
        network.sendMinimapArray(SerializeBoard());
    }

    private void checkCombat() {
        foreach (GamePiece gp in pieces) {
            //TODO: make this properly run combat for more than 2 on a square
            Point loc = gp.data.coord;

            if (tiles[loc.X, loc.Y].Count > 1) {
                foreach (GamePiece enemy in tiles[loc.X, loc.Y]) {
                    if(enemy != gp && enemy.data.team != gp.data.team)
                        gp.CombatCheck(enemy);
                }
            }
        }
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
