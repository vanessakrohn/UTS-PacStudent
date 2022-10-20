using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _intro;
    private AudioSource _background;
    public AudioClip spidersScared;
    public AudioClip spidersNormal;
    public AudioClip spiderDead;
    private AudioClip _previousClip;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        var audios = GetComponents<AudioSource>();
        _intro = audios[0];
        _background = audios[1];
        _previousClip = spidersNormal;
        _intro.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_intro.isPlaying && !_background.isPlaying && !gameManager.isPaused)
        {
            _background.loop = true;
            _background.clip = _previousClip;
            _background.Play();
        }
    }

    public void SpidersScared()
    {
        _intro.Pause();
        _background.clip = spidersScared;
        _previousClip = spidersScared;
    }

    public void SpidersNormal()
    {
        _intro.Pause();
        _background.clip = spidersNormal;
        _previousClip = spidersNormal;
    }

    public void SpiderDead()
    {
        _intro.Pause();
        _background.clip = spiderDead;
        _background.loop = false;
        _background.Play();
    }
}