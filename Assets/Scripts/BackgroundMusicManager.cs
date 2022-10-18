using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _intro;
    private AudioSource _backgroundNormal;
    public AudioClip spidersScared;
    public AudioClip spidersNormal;

    // Start is called before the first frame update
    void Start()
    {
        var audios = GetComponents<AudioSource>();
        _intro = audios[0];
        _backgroundNormal = audios[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!_intro.isPlaying && !_backgroundNormal.isPlaying)
        {
            _backgroundNormal.Play();
        }
    }

    public void SpidersScared()
    {
        _backgroundNormal.clip = spidersScared;
    }
    
    public void SpidersNormal()
    {
        _backgroundNormal.clip = spidersNormal;
    }
}
