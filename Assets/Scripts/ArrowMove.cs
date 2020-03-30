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
        if (Input.GetKeyUp(KeyCode.UpArrow)) { 
            self.Move(0, 1);
        } else if (Input.GetKeyUp(KeyCode.DownArrow)) { 
            self.Move(0, -1);
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)) { 
            self.Move(-1, 0);
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) { 
            self.Move(1, 0);
        } 
    }
}
