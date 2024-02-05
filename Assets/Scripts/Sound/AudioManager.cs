using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class AudioManager : Everest.Singleton<AudioManager>
{
    public Sound[] sounds;

    // public static AudioManager instance;

    protected override void Awake()
    {
        base.Awake();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlayClickBtn();
        }
    }

    private void OnEnable()
    {
        //EventDispatcher.Instance.RegisterListener(EventID.Mute, MuteSound);
    }

    private void OnDisable()
    {
        //EventDispatcher.Instance.RemoveListener(EventID.Mute, MuteSound);
    }

    public void Play(SoundID name)
    {

        //switch (name)
        //{
        //    case SoundID.BGM_main:
        //    case SoundID.BGM_battleIntro:
        //    case SoundID.BGM_battle:
        //        if (!SettingData.MusicEnable) return;
        //        break;
        //    default:
        //        if (!SettingData.SoundEnable) return;
        //        break;
        //}

        var s = Array.Find(sounds, sound => sound.name == name.ToString());
        if (s == null)
        {
            //Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        //Debug.Log($"Sound: {name} played.");
        s.source.Play();
    }

    public void Stop(SoundID name)
    {
        var s = Array.Find(sounds, sound => sound.name == name.ToString());
        if (s == null)
        {
            //Debug.LogWarning($"Sound: {name} not found!");
            return;
        }

        s.source.Stop();
    }

    public void StopAllSound()
    {
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void PlayClickBtn()
    {
        Play(SoundID.click);
    }
}

public enum SoundID
{
    day,
    night,
    yard_BG_1,
    yard_BG_2,
    yard_BG_night,
    buffalo,
    fox,
    click,
    door,
    walk
}