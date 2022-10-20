using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _intro;
    private AudioSource _background;
    public AudioClip spidersScared;
    public AudioClip spidersNormal;
    public AudioClip spiderDead;
    private AudioClip _previousClip;

    // Start is called before the first frame update
    void Start()
    {
        var audios = GetComponents<AudioSource>();
        _intro = audios[0];
        _background = audios[1];
        _previousClip = spidersNormal;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_intro.isPlaying && !_background.isPlaying)
        {
            _background.loop = true;
            _background.clip = _previousClip;
            _background.Play();
        }
    }

    public void SpidersScared()
    {
        _background.clip = spidersScared;
        _previousClip = spidersScared;
    }

    public void SpidersNormal()
    {
        _background.clip = spidersNormal;
        _previousClip = spidersNormal;
    }

    public void SpiderDead()
    {
        _background.clip = spiderDead;

        _background.loop = false;
        _background.Play();
    }
}