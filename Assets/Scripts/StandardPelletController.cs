using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class StandardPelletController : MonoBehaviour
{
    public LevelManager levelManager;
    public ScoreController scoreController;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            var indices = levelManager.getIndices(transform.position);
            int i = indices.i;
            int j = indices.j;
            levelManager.grid[i, j] = LevelManager.Tile.Empty;
            scoreController.score += 10;
            
            if (AllPelletsEaten())
            {
                gameManager.GameOver();
            }
        }
    }

    private bool AllPelletsEaten()
    {
        for (var i = 0; i < levelManager.grid.GetLength(0); i++)
        {
            for (var j = 0; j < levelManager.grid.GetLength(1); j++)
            {
                if (levelManager.grid[i, j] == LevelManager.Tile.StandardPellet)
                {
                    return false;
                }
            }
        }
        return true;
    }
}