using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRayController : MonoBehaviour
{
    public GhostController[] spiders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        switch (other.name)
        {
            case "Spider1":
                StartCoroutine(spiders[0].DeadCoroutine());
                break;
            case "Spider2":
                StartCoroutine(spiders[1].DeadCoroutine());
                break;
            case "Spider3":
                StartCoroutine(spiders[2].DeadCoroutine());
                break;
            case "Spider4":
                StartCoroutine(spiders[3].DeadCoroutine());
                break;
        }
    }
}
