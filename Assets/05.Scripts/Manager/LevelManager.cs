using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingleTon<LevelManager>
{
    [SerializeField]
    private SpawnPoint[] spawnPoints;

    public SpawnPoint[] SpawnPoints
    {
        get
        {
            return spawnPoints;
        }

        set
        {
            spawnPoints = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
