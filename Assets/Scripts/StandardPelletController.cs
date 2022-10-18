using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class StandardPelletController : MonoBehaviour
{
    public LevelManager levelManager;
    public ScoreController scoreController;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        var indices = levelManager.getIndices(transform.position);
        int i = indices.i;
        int j = indices.j;
        levelManager.grid[i, j] = LevelManager.Tile.Empty;
        scoreController.score += 10;
    }
}
