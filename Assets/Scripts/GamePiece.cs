using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public Board board;
    public int startX;
    public int startY;
    Point coord;

    // Start is called before the first frame update
    void Start() {
        this.coord = new Point(startX, startY);
        print(this);
        // board.Register(this, this.coord);
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetPosition(Point newCoord) {
        this.coord = newCoord;
        Point pos = board.CoordToPos(newCoord);
        transform.position = new Vector2(pos.X, pos.Y);
    }

    public void Move(int deltaX, int deltaY) {
        Point newCoord = board.MovePiece(this.coord, new Point(this.coord.X+deltaX, this.coord.Y+deltaY), this);
        if (this.coord != newCoord) {
            this.SetPosition(newCoord);
        }
    }
}
