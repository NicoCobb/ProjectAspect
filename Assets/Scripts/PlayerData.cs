using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int health = 15;
    public int damageMax = 4;
    public int damageMin = 1;
    public Point coord;
    public Team team;
    // Start is called before the first frame update
    void Start()
    {
        team = (Team)Random.Range(0,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Direction {Up, Down, Left, Right};
public enum Team {Red, Blue};
