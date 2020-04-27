using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public Board board;
    public int startX;
    public int startY;
    [HideInInspector]
    public PlayerData data;
    public LinkedList<Direction> moveList = new LinkedList<Direction>();

    // Start is called before the first frame update
    void Awake() {
        // moveList = new LinkedList<Direction>();
        data = gameObject.GetComponent(typeof(PlayerData)) as PlayerData;
        print(data);
        data.coord = new Point(startX, startY);
        print(this);
        // moveList.AddLast(Direction.Right);
        // moveList.AddLast(Direction.Left);
        // moveList.AddLast(Direction.Right);
        // moveList.AddLast(Direction.Left);
        // moveList.AddLast(Direction.Right);
        // SetPosition(new Point(0,0));
    }
    public void SetPosition(Point newCoord) {
        print(data);
        data.coord = newCoord;
        Point pos = board.CoordToPos(newCoord);
        transform.position = new Vector2(pos.X, pos.Y);
    }

    public void SetMoveSteps(LinkedList<Direction> moves) {
        moveList = moves;
    }

    public void MoveStep() {
        if(moveList != null) { 
            Direction currentStep = moveList.First.Value;
            moveList.RemoveFirst();

            switch(currentStep) {
                case Direction.Up:
                    Move(0,1);
                    break;
                case Direction.Left:
                    Move(-1,0);
                    break;
                case Direction.Down:
                    Move(0,-1);
                    break;
                case Direction.Right:
                    Move(1,0);
                    break;
                case Direction.Stand:
                    Move(0,0);
                    break;
                default:
                    print("no movement");
                    break;
            }
        }
    }
    public void Move(int deltaX, int deltaY) {
        int newX = data.coord.X+deltaX;
        int newY = data.coord.Y+deltaY;

        //account for wraparound/overflow. 
        //this can be adjusted if wraparound is no longer wanted
        if(newX < 0) {
            newX = board.xSize - 1;
        } else if (newX >= board.xSize) {
            newX = 0;
        }
        if(newY < 0) {
            newY = board.ySize - 1;
        } else if (newY >= board.ySize) {
            newY = 0;
        }

        Point newCoord = board.MovePiece(data.coord, new Point(newX,newY), this);
        if (data.coord != newCoord) {
            this.SetPosition(newCoord);
        }
    }

    public void CombatCheck(GamePiece enemy) {
        int enemyAtk = Random.Range(enemy.data.damageMin, enemy.data.damageMax);
        int friendlyAtk =  Random.Range(data.damageMin, data.damageMax);

        data.health -= enemyAtk;
        enemy.data.health -= friendlyAtk;
        
        enemy.IsAlive();
        IsAlive();
    }

    public void IsAlive() {
        if(data.health <= 0) {
            //death animation or whatever here
            Destroy(this);
            Application.Quit();
        }
    }
}
