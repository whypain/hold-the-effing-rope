using UnityEngine;


public class AudioSystem : MonoBehaviour
{
    private static AudioSource _sfxSource;

    private void Awake()
    {
        _sfxSource ??= GetComponent<AudioSource>();
    }

    public static void PlaySFX(AudioClip clip)
    {
        if (_sfxSource == null || clip == null) return;
        _sfxSource.PlayOneShot(clip);
    }
}
