using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
    
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            Button button = GameObject.FindWithTag("ExitButton").GetComponent<Button>();
            button.onClick.AddListener(LoadStartScene);
        }
        
    }

}
