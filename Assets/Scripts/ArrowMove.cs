using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    GamePiece self;

    // Start is called before the first frame update
    void Start() {
        self = gameObject.GetComponent(typeof(GamePiece)) as GamePiece;
    }

    // Update is called once per frame
    void Update() {
        if(self.moveList.Count < 5) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { 
                self.moveList.AddLast(Direction.Up);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) { 
                self.moveList.AddLast(Direction.Down);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) { 
                self.moveList.AddLast(Direction.Left);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) { 
                self.moveList.AddLast(Direction.Right);
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                self.moveList.AddLast(Direction.Stand);
            }
        }
    }
}
