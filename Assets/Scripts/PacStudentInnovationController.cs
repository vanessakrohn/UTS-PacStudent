using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacStudentInnovationController : MonoBehaviour
{
    public PacStudentController pacStudent;
    public ParticleSystem laserRay;
    public GameObject laser;
    private Text laserText;
    private float laserReadyTime;
    
    // Start is called before the first frame update
    void Start()
    {
        laserRay.Stop();
        laserText = laser.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (pacStudent.currentInput)
        {
            case LevelManager.Direction.Down:
                laserRay.transform.eulerAngles = new Vector3(90, 0, 0);
                break;
            case LevelManager.Direction.Right:
                laserRay.transform.eulerAngles = new Vector3(0, 90, 0);
                break;
            case LevelManager.Direction.Up:
                laserRay.transform.eulerAngles = new Vector3(-90, 0, 0);
                break;
            case LevelManager.Direction.Left:
                laserRay.transform.eulerAngles = new Vector3(0, -90, 0);
                break;
        }

        float currentTime = Time.time;
        if (currentTime < laserReadyTime)
        {
            float remainingTime = laserReadyTime - currentTime;
            laserText.text = "Laser ready in " + Mathf.Ceil(remainingTime);
        }
        else
        {
            laserText.text = "Press Space to activate laser!";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LaserRayCoroutine());
        }
    }

    private IEnumerator LaserRayCoroutine()
    {
        float currentTime = Time.time;
        if (currentTime >= laserReadyTime)
        {
            laserReadyTime = currentTime + 23;
            laserRay.Play();
            yield return new WaitForSeconds(3.0f);
            laserRay.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
    }

}
