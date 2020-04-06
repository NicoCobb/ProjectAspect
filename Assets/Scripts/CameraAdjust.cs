using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public GameObject board;

    // private int cameraSize;
    // Start is called before the first frame update
    void Start()
    {
        Camera currentCam = GetComponent<Camera>();
        Board gameBoard = board.GetComponent<Board>();
        currentCam.orthographicSize = gameBoard.xSize - 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
