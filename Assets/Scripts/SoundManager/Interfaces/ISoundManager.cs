using System.Collections.Generic;
using UnityEngine;

namespace SM.Sound
{
    public interface ISoundManager
    {
        IPlaylist SFXPlaylist { get; }
        IPlaylist MusicPlaylist { get; }

        bool MuteSfx { get; set; }
        bool MuteMusic { get; set; }
        float MusicVolume { get; set; }

        bool isMusic { get; }
        bool isSFXA { get; }
        bool isSFXB { get; }

        AudioSource CurrentMusicAudioSource { get; set; }
        List<AudioSource> AudioSourcePool { get; }

        void PlayMusic(string musicClipName);
        void PlayMusic(string musicClipName, float volume);
        void StopMusic();
        void ResumeMusic();
        void FadeOutMusic();
        void FadeOutMusic(AudioSource musicSource);
        void FadeOutMusic(float targetVolume);
        void FadeOutMusic(AudioSource musicSource, float targetVolume);
        void FadeMusic(float volume, float duration);

        void CreateSet(string setName, string[] clipNames);
        void PlayRandomSFXFromSet(string setName);
        void PlayRandomSFXFromSet(string setName, float volume);
        void PlayPanoramaSound(string clipName, float pan);

        void PlaySFX(string clipName);
        void PlaySFX(string clipName, float volume);
        void PlaySFX(string clipName, float volume, bool loop);
        void PlaySFX(string clipName, float volume, InterruptionType interruptType);
        void PlaySFX(string clipName, float volume, InterruptionType interruptType, string[] alsoInterrupt,bool loop = false);
        
        void PlaySFX(AudioClip soundClip);
        void PlaySFX(AudioClip soundClip, float volume);
        void PlaySFX(AudioClip soundClip, float volume, InterruptionType interruptType, string[] alsoInterrupt);
        void PlaySFX(AudioClip soundClip, float volume, InterruptionType interruptType, List<string> alsoInterrupt, bool loop = false);

        bool SFXIsPlaying(AudioClip soundClip);
        void StopSFX(string clipName);
        void StopSFX(AudioClip clip);

        AudioSource GetPlayingSFXSource(AudioClip soundClip);
        AudioSource GetPlayingMusicSource();

        void StopAllSFX();
    }
}
