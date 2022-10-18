using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private Tweener _tweener;
    private Camera _camera;
    private Renderer _renderer;
    public LevelManager levelManager;
    public ScoreController scoreController;
    private bool _maySpawn = false; 

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _renderer = gameObject.GetComponent<Renderer>();
        _tweener = gameObject.GetComponent<Tweener>();
        StartCoroutine(SpawnVegemiteCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tweener.TweenExists(transform) && _maySpawn)
        {
            SpawnVegemite();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _renderer.enabled = false;
            scoreController.score += 100;
        }
    }

    private void SpawnVegemite()
    {
        float randX = Random.Range(0.0f, 1.0f);
        float randY = Random.Range(0.0f, 1.0f);
        int randSide = Random.Range(0, 3);
        Vector3 randPos = Vector3.zero;
        Vector3 mirroredPoint = Vector3.zero;
        _renderer.enabled = true;
        switch (randSide)
        {
            case 0:
                randPos = _camera.ViewportToWorldPoint(new Vector3(0, randY, _camera.nearClipPlane));
                randPos.x -= 64.0f / 100.0f;
                mirroredPoint = _camera.ViewportToWorldPoint(new Vector3(1, 1-randY, _camera.nearClipPlane));
                mirroredPoint.x += 64.0f / 100.0f;
                break;
            case 1:
                randPos = _camera.ViewportToWorldPoint(new Vector3(randX, 0, _camera.nearClipPlane));
                randPos.y -= 64.0f / 100.0f;
                mirroredPoint = _camera.ViewportToWorldPoint(new Vector3(1-randX, 1, _camera.nearClipPlane));
                mirroredPoint.y += 64.0f / 100.0f;
                break;
            case 2:
                randPos = _camera.ViewportToWorldPoint(new Vector3(randX, 1, _camera.nearClipPlane));
                randPos.y += 64.0f / 100.0f;
                mirroredPoint = _camera.ViewportToWorldPoint(new Vector3(1-randX, 0, _camera.nearClipPlane));
                mirroredPoint.y -= 64.0f / 100.0f;
                break;
            case 3:
                randPos = _camera.ViewportToWorldPoint(new Vector3(1, randY, _camera.nearClipPlane));
                randPos.x += 64.0f / 100.0f;
                mirroredPoint = _camera.ViewportToWorldPoint(new Vector3(0, 1-randY, _camera.nearClipPlane));
                mirroredPoint.x -= 64.0f / 100.0f;
                break;
        }

        _tweener.AddTween(transform, randPos, mirroredPoint, 10.0f);
    }

    IEnumerator SpawnVegemiteCoroutine()
    {
        yield return new WaitForSeconds(10);
        _maySpawn = true;
    }
}