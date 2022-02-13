using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.SpatialBlend;
            s.source.maxDistance = s.MaxDistance;
            s.source.rolloffMode = AudioRolloffMode.Custom;
        }
    }

    void start()
    {
    }
    
    //Plays a sound effect using the name of the sound
    //Example: FindObjectOfType<Audio Manager>().Play(“SoundName”);
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Stops playing a sound effect using the name of the sound
    //Example: FindObjectOfType<Audio Manager>().Stop(“SoundName”);
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    //Sets the volume of a sound effect
    //Example: FindObjectOfType<Audio Manager>().setVolume(“SoundName”, 1);
    public void setVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }

    //Fades the sound using an interpolated float value
    //Example: FindObjectOfType<Audio Manager>().setVolume(“SoundName”, 0, 0.5);
    public void fadeSound(string name, float targetVolume, float lerpDuration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeAudioSource.StartFade(s, lerpDuration, targetVolume));
    }

    //Checks to see if a sound with a specific name is currently playing
    //Example: FindObjectOfType<Audio Manager>().isPlaying(“SoundName”);
    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        return s.source.isPlaying;
    }
}
