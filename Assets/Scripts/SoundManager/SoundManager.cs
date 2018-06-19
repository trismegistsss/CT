using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SM.Sound
{
    public class SoundManager : SoundBase
    {
        public void FadeSFX(string clipName, float fadeTime, float volume)
        {
            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if ((audioSource.clip != null) && (audioSource.clip.name == clipName && audioSource.isPlaying))
                {
                    DOTween.To(() => audioSource.volume, p => audioSource.volume = p, volume, fadeTime);
                }
            }
        }

        public bool IsMusicPlaying(string musicName)
        {
            return IsMusicName(musicName) && CurrentMusicAudioSource;
        }

        public bool IsMusicName(string musicName)
        {
            return CurrentMusicAudioSource != null && 
                   CurrentMusicAudioSource.clip != null && 
                   CurrentMusicAudioSource.clip.name == musicName;
        }

        public void AddMusic(AudioClip clip)
        {
            if (!MusicPlaylist.Contains(clip))
            {
                MusicPlaylist.Add(clip);
            }
        }

        public void AddSfx(AudioClip clip)
        {
            if (!SFXPlaylist.Contains(clip))
            {
                SFXPlaylist.Add(clip);
            }
        }

        public void PlayMusicWithFade(string musicClipName, float volume)
        {
            PlayMusic(musicClipName);
            FadeMusic(volume, 0f);
        }

        public bool IsAnySfxPlaying()
        {
            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnyMusicPlaying()
        {
            AudioSource src = GetPlayingMusicSource();

            if (src != null)
            {
                if (src.isPlaying)
                    return true;
            }
            return false;
        }

        public bool HasSfxByName(string name)
        {
            foreach (AudioClip clip in SFXPlaylist)
                if (clip.name == name)
                    return true;

            return false;
        }
    }
}
