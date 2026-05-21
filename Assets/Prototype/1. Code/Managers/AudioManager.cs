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

    public void PlayMusic(AudioClip pClip, bool pLoop = true, float pVolume = 0.35f)
    {
        if (_musicSource.clip == pClip)
            return;

        _musicSource.clip = pClip;
        _musicSource.loop = pLoop;
        _musicSource.volume = pVolume;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip pClip, bool pLoop = false, float pVolume = 0.35f)
    {
        if (pLoop)
        {
            if (_loopSFXSource.clip == pClip && _loopSFXSource.isPlaying)
                return;

            _loopSFXSource.clip = pClip;
            _loopSFXSource.loop = true;
            _loopSFXSource.volume = pVolume;
            _loopSFXSource.Play();
        }
        else
        {
            _sfxSource.clip = pClip;
            _sfxSource.volume = pVolume;
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
