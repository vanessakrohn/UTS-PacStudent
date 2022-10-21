using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPelletController : MonoBehaviour
{
    public LevelManager levelManager;
    public SpiderManager spiderManager;

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
            var indices = levelManager.GetIndices(transform.position);
            int i = indices.i;
            int j = indices.j;
            levelManager.grid[i, j] = LevelManager.Tile.Empty;
            spiderManager.ScaredState();
        }
    }
}
