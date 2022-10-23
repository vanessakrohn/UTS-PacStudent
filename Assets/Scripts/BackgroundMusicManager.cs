using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _intro;
    private AudioSource _background;
    public AudioClip spidersScared;
    public AudioClip spidersNormal;
    public AudioClip spiderDead;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        var audios = GetComponents<AudioSource>();
        _intro = audios[0];
        _background = audios[1];

        _intro.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_intro.isPlaying && !_background.isPlaying && !gameManager.isPaused)
        {
            _background.Play();
        }

        if (gameManager.isPaused)
        {
            _background.Pause();
        }
    }

    public void SpidersScared()
    {
        _intro.Pause();
        _background.clip = spidersScared;

    }

    public void SpidersNormal()
    {
        _intro.Pause();
        _background.clip = spidersNormal;
   
    }

    public void SpiderDead()
    {
        _intro.Pause();
        _background.clip = spiderDead;
    }
}