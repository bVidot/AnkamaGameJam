using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static SoundManager _instance;

    public static SoundManager GetInstance()
    {
        if(!_instance)
        {
            GameObject soundManager = new GameObject("SoundManager");
            _instance = soundManager.AddComponent<SoundManager>();
            _instance.Initialize();
        }

        return _instance;
    }

    const float MaxVolume_BGM = 1f;
    const float MaXVolume_SFX = 1f;
    static float currentVolume_BGM = 1f;
    static float currentVolume_SFX = 1f;
    static bool isMuted = false;

    List<AudioSource> sfxSources;
    AudioSource bgmSource;

    void Initialize()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = GetBGMVolume();
        DontDestroyOnLoad(gameObject);
    }

   /**
    * volume Getter
    * */
    static float GetBGMVolume()
    {
        return isMuted ? 0f : MaxVolume_BGM * currentVolume_BGM;
    }

    static float GetSFXVolume()
    {
        return isMuted ? 0f : MaXVolume_SFX * currentVolume_SFX;
    }

    // BGM
    void FadeBGMOut(float duration)
    {
        SoundManager soundManager = GetInstance();
        float delay = 0f;
        float toVolume = 0f;

        if (soundManager.bgmSource.clip != null)
            StartCoroutine(FadeBGM(toVolume, delay, duration));

    }

    void FadeBGMIn(AudioClip bgmClip, float delay, float duration)
    {
        SoundManager soundManager = GetInstance();
        soundManager.bgmSource.clip = bgmClip;
        soundManager.bgmSource.Play();

        float toVolume = GetBGMVolume();

        StartCoroutine(FadeBGM(toVolume, delay, duration));
    }

    IEnumerator FadeBGM(float fadeToVolume, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        SoundManager soundManager = GetInstance();
        float elapsed = 0f;
        while(duration > 0)
        {
            float t = elapsed / duration;
            float volume = Mathf.Lerp(0f, fadeToVolume * currentVolume_BGM, t);
            soundManager.bgmSource.volume = volume;

            elapsed += Time.deltaTime;
            yield return 0;
        }
    }

    public static void PlayBGM(AudioClip bgmClip, bool fade, float fadeDuration)
    {
        SoundManager soundManager = GetInstance();

        if(fade)
        {
            if (soundManager.bgmSource.isPlaying)
            {
                soundManager.FadeBGMOut(fadeDuration / 2);
                soundManager.FadeBGMIn(bgmClip, fadeDuration / 2, fadeDuration / 2);
            }
            else
            {
                float delay = 0f;
                soundManager.FadeBGMIn(bgmClip, delay, fadeDuration);
            }
        }else
        {
            soundManager.bgmSource.volume = GetBGMVolume();
            soundManager.bgmSource.clip = bgmClip;
            soundManager.bgmSource.Play();
        }
        
    }

    public static void StopBGM(bool fade, float fadeDuration)
    {
        SoundManager soundManager = GetInstance();
        if(soundManager.bgmSource.isPlaying)
        {
            if (fade)
                soundManager.FadeBGMOut(fadeDuration);
            else
                soundManager.bgmSource.Stop();
        }
    }

    // SFX
    AudioSource GetSFXSource()
    {
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = GetSFXVolume();

        if (sfxSources == null)
            sfxSources = new List<AudioSource>();

        sfxSources.Add(sfxSource);

        return sfxSource;
    }

    IEnumerator RemoveSFXSource(AudioSource sfxSource, float fixedLenght = 0f)
    {
        yield return new WaitForSeconds(fixedLenght <= 0f ? sfxSource.clip.length : fixedLenght);
        sfxSources.Remove(sfxSource);
        Destroy(sfxSource);
    }

    public static void PlaySFX(AudioClip sfxClip)
    {
        SoundManager soundManager = GetInstance();
        AudioSource source = soundManager.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.Play();

        soundManager.StartCoroutine(soundManager.RemoveSFXSource(source));
    }

    public static void PlaySFXRandomized(AudioClip sfxClip)
    {
        SoundManager soundManager = GetInstance();
        AudioSource source = soundManager.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.pitch = Random.Range(0.85f, 1.2f);
        source.Play();

        soundManager.StartCoroutine(soundManager.RemoveSFXSource(source));
    }

    public static void PlaySFXFixedDuration(AudioClip sfxClip, float duration, float volumeMultiplier = 1f)
    {
        SoundManager soundManager = GetInstance();
        AudioSource source = soundManager.GetSFXSource();
        source.volume = GetSFXVolume() * volumeMultiplier;
        source.clip = sfxClip;
        source.loop = true;
        source.Play();

        soundManager.StartCoroutine(soundManager.RemoveSFXSource(source, duration));
    }

    public static void DisableSoundImmediate()
    {
        SoundManager soundManager = GetInstance();
        soundManager.StopAllCoroutines();
        if(soundManager.sfxSources != null)
        {
            foreach(AudioSource source in soundManager.sfxSources)
            {
                source.volume = 0f;
            }
        }

        soundManager.bgmSource.volume = 0f;
        isMuted = true;
    }

    public static void EnableSoundImmediate()
    {
        SoundManager soundManager = GetInstance();
        soundManager.StopAllCoroutines();
        if (soundManager.sfxSources != null)
        {
            foreach (AudioSource source in soundManager.sfxSources)
            {
                source.volume = GetSFXVolume();
            }
        }

        soundManager.bgmSource.volume = GetBGMVolume();
        isMuted = false;
    }

    public static void SetGlobalVolume(float volume)
    {
        currentVolume_BGM = volume;
        currentVolume_SFX = volume;
        AdjustSoundImmediate();
    }

    public static void SetSFXVolume(float volume)
    {
        currentVolume_SFX = volume;
        AdjustSoundImmediate();
    }

    public static void AdjustSoundImmediate()
    {
        SoundManager soundManager = GetInstance();
        if(soundManager.sfxSources != null)
        {
            foreach(AudioSource source in soundManager.sfxSources)
            {
                source.volume = GetSFXVolume();
            }
        }

        soundManager.bgmSource.volume = GetBGMVolume();
    }
}
