using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 13, 2018
 * 
 * Author:      Zachary Schmalz
 * Version:     1.1.0
 * Date:        September 28, 2018
 *              Converted class to be static
 */

/// <summary>
/// The Sound class describes the properties and components of a sound visible in the Unity Inspector
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// A reference to the Unity AudioClip asset
    /// </summary>
    public AudioClip clip;
    /// <summary>
    /// A name given to the AudioClip to be referenced in code and used by various dictionarys
    /// </summary>
    public string referenceName;
    /// <summary>
    /// Does the sound loop
    /// </summary>
    public bool loop;
    /// <summary>
    /// Controls the volume of the AudioClip, used for volume normalization
    /// </summary>
    [Range(0, 1)] public float volume = 1.0f;
    /// <summary>
    /// Controls the pitch of the AudioClip
    /// </summary>
    [Range(-3, 3)] public float pitch = 1.0f;
}

/// <summary>
/// The SoundManager manages all Music, Effects, Dialogue, and other various AudioSources in the game.
/// </summary>
public class SoundManager : MonoBehaviour
{

    /// <summary>
    /// Master volume for all AudioSources in the game
    /// </summary>
    [Range(0, 1)] public float MasterVolume;
    /// <summary>
    /// Master volume for all music sources in the game
    /// </summary>
    [Range(0, 1)] public float MusicMasterVolume;
    /// <summary>
    /// Master volume for all sound effects in the game
    /// </summary>
    [Range(0, 1)] public float EffectsMasterVolume;
    /// <summary>
    /// Master volume for all dialogue in the game
    /// </summary>
    [HideInInspector, Range(0, 1)] public float DialogueMasterVolume;

    /// <summary>
    /// List of all music Sounds visible in the Inspector window
    /// </summary>
    public List<Sound> musicList;
    /// <summary>
    /// List of all effect Sounds visible in the Inspector window.
    /// </summary>
    public List<Sound> effectsList;
    /// <summary>
    /// List of all dialogue Sounds visible in the Inspector window
    /// </summary>
    [HideInInspector] public List<Sound> dialogueList;

    /// <summary>
    /// A Singleton reference to the SoundManager object
    /// </summary>
    private static SoundManager singleton;

    // Dictionary's for all different types of Sounds
    private static Dictionary<string, Sound> musicDictionary;
    private static Dictionary<string, Sound> effectsDictionary;
    private static Dictionary<string, Sound> dialogueDictionary;

    // Reference to the only music source that should be playing
    private static AudioSource musicSource;

    // List containing all Sounds that are actively playing
    private static List<AudioSource> sourcesPlaying;

    // Assign singleton value and create sound dictionaries
    public void Awake()
    {
        // Delete any extra copies of script not attached to the GameObject with the GameManager
        if (singleton == null && gameObject.GetComponent<GameManager>())
            singleton = this;

        else if (singleton != this)
        {
            Destroy(this);
            return;
        }

        CreateDictionaries();
        
        Debug.AudioLog("SoundManager Awake");
    }

    // Initialize the list of playing sources with the musicSource
    void Start()
    {
        sourcesPlaying = new List<AudioSource>() { musicSource };
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Begins a Coroutine that starts to play the music
    /// </summary>
    /// <param name="key">The sounds referenceName to search for in the music dictionary</param>
    /// <param name="fadeDuration">How long should the currently playing music fade out before palying</param>
    /// <param name="source">A reference to the AudioSource component</param>
    /// <remarks></remarks>
    public static void PlayMusic(string key, float fadeDuration = 0.0f, AudioSource source = null)
    {
        // Checks that the music dictionary contains the key
        if (musicDictionary.ContainsKey(key))
            singleton.StartCoroutine(PlayMusicCoroutine(key, fadeDuration, source));
        else
            Debug.AudioLog("MusicDictionary does not contain key: " + key);
    }

    /// <summary>
    /// Begins a Coroutine that starts to play an effect defined in the effects list
    /// </summary>
    /// <param name="key">The effect's reference Name to search in the effects dictionary</param>
    /// <param name="source">A reference to the AudioSource component</param>
    /// <remarks>Use this method for globally defined effects (i.e. effects defined in the effectList)
    /// If the <para name="source"> is null, a temporary AudioSource is created and initalized to the values
    /// defined in the sound class, and destroyed after the effect finishes playing. Otherwise, the manager
    /// uses the source provided to play the effect, allowing for a more customized AudioSource (such as 3D)</para></remarks>
    public static void PlayEffect(string key, AudioSource source = null)
    {
        // Checks that the effects dictionary contains the key
        if (effectsDictionary.ContainsKey(key))
            singleton.StartCoroutine(PlayEffectCoroutine(key, source));
        else
            Debug.AudioLog("EffectDictionary does not contain key: " + key);
    }

    /// <summary>
    /// Begins a Coroutine that starts to play an effect defined in external GameObjects
    /// </summary>
    /// <param name="source">The AudiSource component used to play the effect</param>
    /// <remarks>Use this method to play effects specific to GameObjects and AudioSources (i.e. custom defined AudioSources)
    /// Use this method if there is not a globally defined effect in the effectsList of this class</remarks>
    public static void PlayEffect(AudioSource source)
    {
        if (source && source.clip != null)
            singleton.StartCoroutine(PlayEffectCoroutine(source));
        else
            Debug.AudioLog("AudioSource and/or AudioClip is null");
    }

    static IEnumerator PlayMusicCoroutine(string key, float duration, AudioSource source)
    {
        // If a musicSource already exists and is playing
        if (musicSource && musicSource.isPlaying)
        {
            Debug.AudioLog("Fading music: " + musicSource.clip.name + " - " + duration + " seconds");
            // Begin fading the music
            if (duration > 0)
            {
                float startVolume = musicSource.volume;
                while (musicSource != null && musicSource.volume > 0)
                {
                    musicSource.volume -= startVolume * Time.deltaTime / duration;
                    yield return new WaitForEndOfFrame();
                }

                if (musicSource != null)
                {
                    musicSource.Stop();
                    Debug.AudioLog("Fade complete: " + musicSource.clip.name);
                }

                // If the source playing the music is attached to this gameObject, destroy it
                AudioSource[] sources = singleton.gameObject.GetComponents<AudioSource>();
                if (sources.Contains<AudioSource>(musicSource))
                {
                    Debug.AudioLog("Music Source: " + musicSource.clip.name + " - Destroyed");
                    Destroy(musicSource);
                }
            }
        }

        Sound sound = musicDictionary[key];

        // If no source is provided, create a temporary AudioSource component to play it
        if (source == null)
            musicSource = singleton.gameObject.AddComponent<AudioSource>();
        else
            musicSource = source;

        // Assign properties of the AudioSource to the values defined in the Sound object
        musicSource.clip = sound.clip;
        musicSource.volume = sound.volume * singleton.MusicMasterVolume * singleton.MasterVolume;
        musicSource.pitch = sound.pitch;
        musicSource.loop = sound.loop;
        musicSource.Play();
        sourcesPlaying[0] = musicSource;

        // Wait untill the music finishes playing, and destroy the AudioSource
        if (source == null)
        {
            Debug.AudioLog("Music Source: " + sound.referenceName + " - Playing");
            if (!sound.loop)
            {
                yield return new WaitUntil(() => musicSource == null || musicSource.isPlaying == false);
                Debug.AudioLog("Music Source: " + sound.referenceName + " - Destroyed");
                Destroy(musicSource);
            }
        }

        else
            Debug.AudioLog("External Music Source: " + sound.referenceName + " - Playing");
    }

    static IEnumerator PlayEffectCoroutine(string key, AudioSource source)
    {
        AudioSource effect;
        Sound sound = effectsDictionary[key];

        // If no source is provided, create a temporary source to play the effect
        if (source == null)
            effect = singleton.gameObject.AddComponent<AudioSource>();
        else
            effect = source;

        // Assign properties of the AudioSource to the values defined in the Sound object
        effect.clip = sound.clip;
        effect.volume = sound.volume * singleton.EffectsMasterVolume * singleton.MasterVolume;
        effect.pitch = sound.pitch;
        effect.loop = false;
        effect.Play();
        sourcesPlaying.Add(effect);
        Debug.AudioLog("Effect Source: " + sound.referenceName + " - Playing");

        // Remove AudioSource from list of playing sources after the effect finishes playing
        yield return new WaitUntil(() => effect == null || effect.time >= effect.clip.length);
        sourcesPlaying.Remove(effect);

        // Destroy the AudioSource if it is null
        if(source == null)
        {
            Debug.AudioLog("Effect Source: " + sound.referenceName + " - Destroyed");
            Destroy(effect);
        }

        else
            Debug.AudioLog("Effect Source: " + sound.referenceName + " - Stopped");
    }

    // This function is meant to play effects from AudioSources that have been defined, and whose clip is not in any dictionary
    static IEnumerator PlayEffectCoroutine(AudioSource source)
    {
        float initialVolume = source.volume;
        source.volume = initialVolume * singleton.EffectsMasterVolume * singleton.MasterVolume;
        source.Play();
        sourcesPlaying.Add(source);
        Debug.AudioLog("Effect Source: " + source.clip.name + " - Playing");

        yield return new WaitUntil(() => source.time >= source.clip.length);
        sourcesPlaying.Remove(source);
        source.volume = initialVolume;
    }

    /// <summary>
    /// Adjusts the volume of all sounds respective to their MasterVolumes parameters
    /// </summary>
    public static void AdjustVolume()
    {
        if (sourcesPlaying != null)
        {
            foreach (AudioSource source in sourcesPlaying)
            {
                if (source && source.isPlaying)
                {
                    Sound s;

                    if (musicDictionary.ContainsValue(s = singleton.musicList.Find(x => x.clip.name == source.clip.name)))
                        source.volume = musicDictionary[s.referenceName].volume * singleton.MusicMasterVolume * singleton.MasterVolume;

                    else if (effectsDictionary.ContainsValue(s = singleton.effectsList.Find(x => x.clip.name == source.clip.name)))
                        source.volume = effectsDictionary[s.referenceName].volume * singleton.EffectsMasterVolume * singleton.MasterVolume;

                    else if (dialogueDictionary.ContainsValue(s = singleton.dialogueList.Find(x => x.clip.name == source.clip.name)))
                        source.volume = dialogueDictionary[s.referenceName].volume * singleton.DialogueMasterVolume * singleton.MasterVolume;

                    else
                        source.volume = source.volume * singleton.MasterVolume;
                }
            }
        }
    }

    /// <summary>
    /// Pauses all currently playing AudioSources
    /// </summary>
    public static void PauseAll()
    {
        int count = 0;
        foreach (AudioSource s in sourcesPlaying)
        {
            if (s != null && s.isPlaying)
            {
                s.Pause();
                count++;
            }
        }
        Debug.AudioLog(count + " sounds paused");
    }

    /// <summary>
    /// UnPauses any Paused AudioSource
    /// </summary>
    public static void UnPauseAll()
    {
        int count = 0;
        foreach (AudioSource s in sourcesPlaying)
        {
            if (s != null && !s.isPlaying)
            {
                s.UnPause();
                count++;
            }
        }
        Debug.AudioLog(count + " sounds resumed");
    }

    /// <summary>
    /// Stops all AudioSources currently playing, and clears the list of playing AudioSources
    /// </summary>
    public static void StopAll()
    {
        int count = 0;
        foreach (AudioSource s in sourcesPlaying)
        {
            if (s != null && s.isPlaying)
            {
                s.Stop();
                count++;
            }
        }
        sourcesPlaying.Clear();
        musicSource = null;
        sourcesPlaying.Add(musicSource);

        // Remove any AudioSource attached to the GameObject
        foreach (AudioSource s in singleton.GetComponents<AudioSource>())
            Destroy(s);

        Debug.AudioLog(count + " sounds stopped");
    }

    // Function that is called when the script is updated in Unity, currently used for adjusting the volume using the inspector sliders
    private static void OnValidate()
    {
        AdjustVolume();
    }

    // Create the sound dictionaries
    private static void CreateDictionaries()
    {
        musicDictionary = new Dictionary<string, Sound>();
        effectsDictionary = new Dictionary<string, Sound>();
        dialogueDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in singleton.musicList)
            musicDictionary.Add(s.referenceName, s);
        foreach (Sound s in singleton.effectsList)
            effectsDictionary.Add(s.referenceName, s);
        foreach (Sound s in singleton.dialogueList)
            dialogueDictionary.Add(s.referenceName, s);
    }
}