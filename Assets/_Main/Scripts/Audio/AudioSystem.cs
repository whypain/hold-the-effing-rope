using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public enum AudioType
{
    Win,
    Lose,
    BGM,
    SkillCheckGood,
    SkillCheckGreat,
    SkillCheckPerfect,
    SkillCheckMiss,
    PersonSaved,
    PersonFell
}

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance { get; private set; }

    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip ropeAmbientClip;

    [Header("Skill Check SFX")]
    [SerializeField] private AudioClip skillCheckGoodClip;
    [SerializeField] private AudioClip skillCheckGreatClip;
    [SerializeField] private AudioClip skillCheckPerfectClip;
    [SerializeField] private AudioClip skillCheckMissClip;

    [Header("Gameplay SFX")]
    [SerializeField] private AudioClip personSaved;
    [SerializeField] private AudioClip personFell;

    [Header("Sources")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;

    private TweenSettings<float> fadeIn;
    private TweenSettings<float> fadeOut;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        source ??= GetComponent<AudioSource>();
        source.loop = true;

        fadeIn = new TweenSettings<float>(
            startValue: 0f,
            endValue: 1f,
            duration: fadeDuration
        );
        fadeOut = new TweenSettings<float>(
            endValue: 0f,
            duration: fadeDuration
        );
    }

    public void Play(AudioType type)
    {
        switch (type)
        {
            case AudioType.Win:
                PlayAudio(winClip, loop: false);
                break;
            case AudioType.Lose:
                PlayAudio(loseClip, loop: false);
                break;
            case AudioType.BGM:
                PlayAudio(bgm);
                break;
            case AudioType.SkillCheckGood:
                PlaySFX(skillCheckGoodClip);
                break;
            case AudioType.SkillCheckGreat:
                PlaySFX(skillCheckGreatClip);
                break;
            case AudioType.SkillCheckPerfect:
                PlaySFX(skillCheckPerfectClip);
                break;
            case AudioType.SkillCheckMiss:
                PlaySFX(skillCheckMissClip);
                break;
            case AudioType.PersonSaved:
                PlaySFX(personSaved);
                break;
            case AudioType.PersonFell:
                PlaySFX(personFell);
                break;
        }
    }

    public void PlayRopeAmbient()
    {
        if (ambientSource == null || ambientSource.isPlaying) return;

        ambientSource.loop = true;
        ambientSource.clip = ropeAmbientClip;
        ambientSource.Play();
    }

    public void StopRopeAmbient()
    {
        if (ambientSource == null || !ambientSource.isPlaying) return;

        ambientSource.Stop();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    private async void PlayAudio(AudioClip clip, bool loop = true)
    {
        if (source == null || clip == null) return;

        source.loop = loop;
        await FadeChange(clip);
    }

    /// <summary>
    /// Interrupts the current bgm temporarily to play a new clip
    /// then returns to the previous bgm after the new clip finishes playing.
    /// </summary>
    /// <param name="clip"></param>
    private async void InterruptBGM(AudioClip clip)
    {
        if (source == null || clip == null) return;

        source.loop = false;
        await FadeChange(clip);

        // wait for the clip to finish playing
        while (source.isPlaying)
        {
            await Task.Yield();
        }

        source.loop = true;
        PlayAudio(bgm);
    }

    private async Task FadeChange(AudioClip newClip)
    {
        if (newClip == source.clip) return;

        fadeIn.endValue = source.volume;

        if (source.isPlaying)
        {
            fadeOut.startValue = source.volume;

            await Tween.AudioVolume(source, fadeOut);
            source.Stop();
        }

        source.clip = newClip;
        source.Play();
        await Tween.AudioVolume(source, fadeIn);
    }
}
