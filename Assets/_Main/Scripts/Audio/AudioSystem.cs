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

[System.Serializable]
public struct AudioClipData
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool loop;
    [SerializeField, Range(0f, 1f)] private float volume;

    public AudioClip Clip => clip;
    public bool Loop => loop;
    public float Volume => volume;

    public AudioClipData(AudioClip clip, bool loop = false, float volume = 1f)
    {
        this.clip = clip;
        this.loop = loop;
        this.volume = volume;
    }
}

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance { get; private set; }

    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private AudioClipData winClip;
    [SerializeField] private AudioClipData loseClip;
    [SerializeField] private AudioClipData bgm;
    [SerializeField] private AudioClipData ropeAmbientClip;

    [Header("Skill Check SFX")]
    [SerializeField] private AudioClipData skillCheckGoodClip;
    [SerializeField] private AudioClipData skillCheckGreatClip;
    [SerializeField] private AudioClipData skillCheckPerfectClip;
    [SerializeField] private AudioClipData skillCheckMissClip;

    [Header("Gameplay SFX")]
    [SerializeField] private AudioClipData personSaved;
    [SerializeField] private AudioClipData personFell;

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
                PlayAudio(winClip);
                break;
            case AudioType.Lose:
                PlayAudio(loseClip);
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
        ambientSource.clip = ropeAmbientClip.Clip;
        ambientSource.volume = ropeAmbientClip.Volume;
        ambientSource.Play();
    }

    public void StopRopeAmbient()
    {
        if (ambientSource == null || !ambientSource.isPlaying) return;

        ambientSource.Stop();
    }

    private void PlaySFX(AudioClipData clipData)
    {
        if (sfxSource == null || clipData.Clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f); // Slightly randomize pitch for variety
        sfxSource.PlayOneShot(clipData.Clip, clipData.Volume);
    }

    private async void PlayAudio(AudioClipData clipData)
    {
        if (source == null || clipData.Clip == null) return;

        source.loop = clipData.Loop;
        await FadeChange(clipData);
    }

    // /// <summary>
    // /// Interrupts the current bgm temporarily to play a new clip
    // /// then returns to the previous bgm after the new clip finishes playing.
    // /// </summary>
    // /// <param name="clipData"></param>
    // private async void InterruptBGM(AudioClipData clipData)
    // {
    //     if (source == null || clipData.Clip == null) return;

    //     source.loop = false;
    //     await FadeChange(clipData);

    //     // wait for the clip to finish playing
    //     while (source.isPlaying)
    //     {
    //         await Task.Yield();
    //     }

    //     source.loop = true;
    //     PlayAudio(bgm);
    // }

    private async Task FadeChange(AudioClipData newClipData)
    {
        if (newClipData.Clip == source.clip) return;

        fadeIn.endValue = newClipData.Volume;

        if (source.isPlaying)
        {
            fadeOut.startValue = source.volume;

            await Tween.AudioVolume(source, fadeOut);
            source.Stop();
        }

        source.clip = newClipData.Clip;
        source.Play();
        await Tween.AudioVolume(source, fadeIn);
    }
}
