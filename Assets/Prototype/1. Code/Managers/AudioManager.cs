using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _loopSFXSource;

    public AudioSource MusicSource { get => _musicSource; set => _musicSource = value; }
    public AudioSource SfxSource { get => _sfxSource; set => _sfxSource = value; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip pClip, bool pLoop = true)
    {
        if (_musicSource.clip == pClip)
            return;

        _musicSource.clip = pClip;
        _musicSource.loop = pLoop;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip pClip, bool pLoop = false)
    {
        if (pLoop)
        {
            if (_loopSFXSource.clip == pClip && _loopSFXSource.isPlaying)
                return;

            _loopSFXSource.clip = pClip;
            _loopSFXSource.loop = true;
            _loopSFXSource.Play();
        }
        else
        {
            _sfxSource.PlayOneShot(pClip);
        }
    }

    public void StopSFX()
    {
        _sfxSource.Stop();
    }

    public void StopLoopingSFX()
    {
        _loopSFXSource.loop = false;
        _loopSFXSource.Stop();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}
