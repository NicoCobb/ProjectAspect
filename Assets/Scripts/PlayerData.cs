using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int health = 15;
    public int damageMax = 4;
    public int damageMin = 1;
    public Point coord = new Point(0,0);
    public Team team;
    // Start is called before the first frame update
    void Start()
    {
        team = (Team)Random.Range(0,1);
    }
}

public enum Direction {Up, Down, Left, Right, Stand};
public enum Team {Red, Blue};
