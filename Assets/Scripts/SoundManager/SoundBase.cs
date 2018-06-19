using System;
using UnityEngine;
using System.Collections.Generic;
using CT.Config;
using DG.Tweening;
using Log.GDebug;

namespace SM.Sound
{
    public class SoundBase : MonoBehaviour, ISoundManager
    {
        public IPlaylist SFXPlaylist { get; set; }
        public IPlaylist MusicPlaylist { get; set; }

        private bool _muteMusic = false;
        public bool MuteMusic
        {
            get
            {
                return _muteMusic;
            }
            set
            {
                _muteMusic = value;
                if (value)
                    PauseMusic();
                else
                    ResumeMusic();
            }
        }

        private bool _muteSfx = false;
        public bool MuteSFX
        {
            get 
            { 
                return _muteSfx;
            }
            set
            {
                _muteSfx = value;

                if (_muteSfx)
                {
                    StopAllSFX();
                }
            }
        }

        public float MusicVolume { get; set; }
        public float FadeTime = 0.5f;
        public float SFXVolume = 1f;
        public bool MuteAllSound = false;
        public bool MuteSfx { get; set; }
        public int AudioPoolSize = 8;

        public AudioSource CurrentMusicAudioSource { get; set; }
        public List<AudioSource> AudioSourcePool { get; private set; }
        public AudioSource MusicAudioSourceA;
        public AudioSource MusicAudioSourceB;

        private Dictionary<string, string[]> _sfxSets = new Dictionary<string, string[]>();
        private Tweener _musicFadeOutTweener;
        private Tweener _musicFadeInTweener;

        public bool isMusic
        {
            get
            {
                if (null != CurrentMusicAudioSource)
                    return CurrentMusicAudioSource.isPlaying;

                return false;
            }
        }

        public bool isSFXA
        {
            get
            {
                if (null != MusicAudioSourceA)
                    return MusicAudioSourceA.isPlaying;

                return false;
            }
        }

        public bool isSFXB
        {
            get
            {
                if (null != MusicAudioSourceB)
                    return MusicAudioSourceB.isPlaying;

                return false;
            }
        }

        void OnDestroy()
        {
            if (_musicFadeOutTweener != null)
                _musicFadeOutTweener.Kill();

            if (_musicFadeInTweener != null)
                _musicFadeInTweener.Kill();

            _musicFadeOutTweener = null;
            _musicFadeInTweener = null;
        }

        void Awake()
        {

#if (TM_AUDIO_OFF)
		MuteAllSound = true;
#endif
            MusicVolume = SoundConfig.DEFAULT_MUSIC_VOLUME;

            //Create the music sources (2) for crossFading
            MusicAudioSourceA = gameObject.AddComponent<AudioSource>();
            MusicAudioSourceB = gameObject.AddComponent<AudioSource>();
            MusicAudioSourceA.rolloffMode = AudioRolloffMode.Linear;
            MusicAudioSourceB.rolloffMode = AudioRolloffMode.Linear;

            MusicAudioSourceA.volume = MusicAudioSourceB.volume = MusicVolume;
            MusicAudioSourceA.loop = MusicAudioSourceB.loop = true;

            CurrentMusicAudioSource = MusicAudioSourceA;

            //initialize the Sfx pool
            AudioSourcePool = new List<AudioSource>();
            int i = 0;
            while (i < SoundConfig.DEFAULT_POOL_SIZE)
            {
                AudioSourcePool.Add(gameObject.AddComponent<AudioSource>());
                AudioSourcePool[i].rolloffMode = AudioRolloffMode.Linear;
                ++i;
            }

            //init tweens so I dont have to keep on checking for nulls below
            //		_musicFadeInTweener = HOTween.To(_musicAudioSourceA, 0, new TweenParms().Prop("volume", MusicVolume));
            //		_musicFadeOutTweener = HOTween.To(_musicAudioSourceB, 0, new TweenParms().Prop("volume", MusicVolume));

            _musicFadeOutTweener = DOTween.To(() => MusicAudioSourceA.volume, (val) => MusicAudioSourceA.volume = val, MusicVolume, 0);
            _musicFadeOutTweener = DOTween.To(() => MusicAudioSourceB.volume, (val) => MusicAudioSourceB.volume = val, MusicVolume, 0);

           
            // Init Sound Manager

            var sfx = Resources.LoadAll<AudioClip>(SoundConfig.FX);
            SFXPlaylist = new Playlist(sfx);

            var music = Resources.LoadAll<AudioClip>(SoundConfig.MUSIC);
            MusicPlaylist = new Playlist(music);

        }

        private AudioSource GetSourceFromPool()
        {
            foreach (AudioSource adSource in AudioSourcePool)
            {
                if (adSource != null && !adSource.isPlaying)
                    return adSource;
            }

           GDebug.Log("Audio Source Pool passed limit of " + AudioPoolSize + ", stealing sound from first source", this, LogCategory.SOUND_MANAGER);
            return AudioSourcePool[0];
        }

        #region Play Music	  

        public void PlayMusic(string musicClipName)
        {
            PlayMusic(musicClipName, SoundConfig.DEFAULT_MUSIC_VOLUME);
        }

        public virtual void PlayMusic(string musicClipName, float volume)
        {
            MusicVolume = volume;
            if (!MuteAllSound)
            {
                AudioClip musicClip = MusicPlaylist.FindByName(musicClipName);

                if (musicClip == null)
                {
                    GDebug.LogError("NO SOUNDTRACK " + musicClipName,this, LogCategory.SOUND_MANAGER);
                    GDebug.Log("Tried to play music : " + musicClipName + " but soundManager could not find it!", this, LogCategory.SOUND_MANAGER);
                    return;
                }

                //we have music playing already
                GDebug.Log("playing: " + musicClip.name, this, LogCategory.SOUND_MANAGER);
                DoMusicPlay(musicClip, volume);
            }
        }

        private void DoMusicCrossFade(AudioClip musicClip, float endVolume)
        {
            DoMusicCrossFade(musicClip, 0f, endVolume);
        }

        private void DoMusicCrossFade(AudioClip musicClip, float startVolume, float endVolume)
        {
            GDebug.Log("crossfading from: " + CurrentMusicAudioSource.clip.name + " to: " + musicClip.name, this, LogCategory.SOUND_MANAGER);

            //clean up faders
            _musicFadeInTweener.Kill();
            _musicFadeOutTweener.Kill();

            FadeOutMusic(CurrentMusicAudioSource);

            AudioSource nextMusicAudioSource = (CurrentMusicAudioSource == MusicAudioSourceA) ? MusicAudioSourceB : MusicAudioSourceA;
            nextMusicAudioSource.clip = musicClip;
            nextMusicAudioSource.volume = startVolume;
            if (!_muteMusic)
                nextMusicAudioSource.Play();
            _musicFadeInTweener = DOTween.To(() => nextMusicAudioSource.volume, (val) => nextMusicAudioSource.volume = val, endVolume, FadeTime).OnComplete(MusicCrossFadeComplete);

            CurrentMusicAudioSource = nextMusicAudioSource;
        }

        private void MusicCrossFadeComplete()
        {
            GDebug.Log("Done with crossfade current music is now: " + CurrentMusicAudioSource.clip.name + " ,vol:" + CurrentMusicAudioSource.volume, this, LogCategory.SOUND_MANAGER);
        }

        private void DoMusicPlay(AudioClip musicClip, float startVolume, float endVolume)
        {
            GDebug.Log("playing music: " + musicClip.name, this, LogCategory.SOUND_MANAGER);

            if (CurrentMusicAudioSource != null)
            {
                CurrentMusicAudioSource.clip = musicClip;
                CurrentMusicAudioSource.volume = startVolume;

                if (!_muteMusic)
                    CurrentMusicAudioSource.Play();

                DOTween.Kill(_musicFadeOutTweener);

                _musicFadeInTweener = DOTween.To(() => CurrentMusicAudioSource.volume, (val) => CurrentMusicAudioSource.volume = val, endVolume, FadeTime).SetEase(Ease.InCubic);
            }
        }

        private void DoMusicPlay(AudioClip musicClip, float volume)
        {
            DoMusicPlay(musicClip, 0f, volume);
        }

        public virtual void ResumeMusic()
        {
            if (CurrentMusicAudioSource != null && MusicVolume > 0)
                CurrentMusicAudioSource.Play();
        }

        public virtual void PauseMusic()
        {
            if (CurrentMusicAudioSource != null)
                CurrentMusicAudioSource.Pause();
        }

        public void StopMusic()
        {
            if (CurrentMusicAudioSource != null)
                CurrentMusicAudioSource.Stop();
        }

        public void FadeOutMusic()
        {
            FadeOutMusic(CurrentMusicAudioSource);
        }

        public void FadeOutMusic(AudioSource musicSource)
        {
            if (musicSource == null || musicSource.clip == null)
                return;
            GDebug.Log("fading out music: " + musicSource.clip.name, this, LogCategory.SOUND_MANAGER);

            _musicFadeOutTweener = DOTween.To(() => musicSource.volume, (val) => musicSource.volume = val, 0f, FadeTime).OnComplete(() => this.DoneMusicFade(musicSource));
            //        _musicFadeOutTweener = HOTween.To(musicSource, FadeTime, new TweenParms().Prop("volume", 0f)._onComplete(this.doneMusicFade, musicSource));
        }

        public void FadeOutMusic(float targetVolume)
        {
            FadeOutMusic(CurrentMusicAudioSource, targetVolume);
        }

        public void FadeOutMusic(AudioSource musicSource, float targetVolume)
        {
            if (musicSource == null || musicSource.clip == null)
                return;
            GDebug.Log("fading out music: " + musicSource.clip.name, this, LogCategory.SOUND_MANAGER);

            _musicFadeOutTweener = DOTween.To(() => musicSource.volume, (val) => musicSource.volume = val, targetVolume, FadeTime).OnComplete(() => this.DoneMusicFade(musicSource));
        }

        public void FadeMusic(float volume, float duration)
        {
            // currentMusicAudioSource may be null
            if (CurrentMusicAudioSource == null)
                return;

            if (_muteMusic)
            {
                CurrentMusicAudioSource.Pause();
                return;
            }

            if (!CurrentMusicAudioSource.isPlaying)
                CurrentMusicAudioSource.Play();

            if (_musicFadeOutTweener != null)
                _musicFadeOutTweener.Kill();

            if (duration <= 0)
            {
                CurrentMusicAudioSource.volume = volume;
                if (volume < .1f)
                    CurrentMusicAudioSource.Pause();
            }
            else
            {
                _musicFadeOutTweener = DOTween.To(() => CurrentMusicAudioSource.volume, (val) => CurrentMusicAudioSource.volume = val, volume, duration).OnComplete(() => this.DoneMusicFade(CurrentMusicAudioSource));
            }
        }


        public void FadeOutSfx(string sfxName, float duration = 1f, float delay = 0f)
        {
            AudioSource sfxSource = GetPlayingSFXSource(SFXPlaylist.FindByName(sfxName));
            FadeSfx(sfxSource, 0f, duration, delay);
        }

        public void FadeSfx(AudioSource sfxAudioSource, float volume = 0f, float duration = 1f, float delay = 0f)
        {
            DOTween.To(() => sfxAudioSource.volume, (val) => sfxAudioSource.volume = val, volume, duration).SetDelay(delay);
        }

        public void DoneMusicFade(AudioSource fadedTargetAudioSource)
        {
            if (fadedTargetAudioSource == null || fadedTargetAudioSource.clip == null)
                return;

            if (fadedTargetAudioSource.volume < .1f)
            {
                GDebug.Log("done music fade stoping music: " + fadedTargetAudioSource.clip.name, this, LogCategory.SOUND_MANAGER);
                fadedTargetAudioSource.Pause();
            }
            else
            {
                GDebug.Log("music Fade is done, but volume is above .1(" + fadedTargetAudioSource.volume + "} so wasnt killed.", this, LogCategory.SOUND_MANAGER);
            }
        }

        #endregion

        #region Play Sound Effects
        public void CreateSet(string setName, string[] clipNames)
        {
            _sfxSets.Add(setName, clipNames);
        }

        public void PlayRandomSFXFromSet(string setName)
        {
            PlayRandomSFXFromSet(setName, SFXVolume);
        }

        public void PlayRandomSFXFromSet(string setName, float volume)
        {
            if (!MuteAllSound && !MuteSfx)
            {
                AudioClip clip = SFXPlaylist.FindByName(_sfxSets["setName"][UnityEngine.Random.Range(0, _sfxSets["setName"].Length)]);
                PlaySFX(clip.name, volume);
            }
        }

        public void PlayPanoramaSound(string clipName, float pan)
        {
            AudioClip audioClip = SFXPlaylist.FindClosestByName(clipName);
            AudioSource currSound = GetPlayingSFXSource(audioClip);
            if (currSound != null)
                currSound.panStereo = pan; // pan from -1 to 1
        }

        public void PlaySFX(string clipName)
        {
            PlaySFX(clipName, SFXVolume);
        }

        public void PlaySFX(string clipName, float volume)
        {
            PlaySFX(clipName, volume, InterruptionType.DontCare);
        }

        public void PlaySFX(string clipName, float volume, bool loop)
        {
            PlaySFX(clipName, volume, InterruptionType.DontCare, new string[0], loop);
        }

        public void PlaySFX(string clipName, float volume, InterruptionType interruptType)
        {
            PlaySFX(clipName, volume, interruptType, new string[0]);
        }

        public void PlaySFX(string clipName, float volume, InterruptionType interruptType, string[] alsoInterrupt, bool loop = false)
        {
            if (string.IsNullOrEmpty(clipName))
                return;

            AudioClip clip = SFXPlaylist.FindByName(clipName);

            if (clip == SFXPlaylist.DefaultClip)
            {
                clip = SFXPlaylist.FindClosestByName(clipName);
                PlaySFX(clip, volume, interruptType, new List<string>(alsoInterrupt), loop);
            }
            else
            {
                PlaySFX(clip, volume, interruptType, new List<string>(alsoInterrupt), loop);
            }
        }

        public void PlaySFX(AudioClip soundClip, float volume)
        {
            PlaySFX(soundClip, volume, InterruptionType.DontCare, new List<string>());
        }

        public void PlaySFX(AudioClip soundClip, float volume, InterruptionType interruptType, string[] alsoInterrupt)
        {
            PlaySFX(soundClip, volume, InterruptionType.DontCare, new List<string>(alsoInterrupt));
        }

        public void PlaySFX(AudioClip soundClip, float volume, InterruptionType interruptType, List<string> alsoInterrupt, bool loop = false)
        {
            GDebug.Log("try play " + soundClip.name + " with volume " + volume + " interrupt " + interruptType, this, LogCategory.SOUND_MANAGER);

            if (!MuteAllSound && !MuteSfx)
            {
                switch (interruptType)
                {
                    case InterruptionType.DontCare:
                        Play(soundClip, volume, loop);
                        break;
                    case InterruptionType.DontInterrupt:
                        if (!SFXIsPlaying(soundClip))
                            Play(soundClip, volume, loop);
                        break;
                    case InterruptionType.Interrupt:
                        if (SFXIsPlaying(soundClip))
                        {
                            GetPlayingSFXSource(soundClip).Stop();
                            StopPlayingSoundList(alsoInterrupt, soundClip.name);
                            Play(soundClip, volume, loop);
                        }
                        else
                        {
                            if (soundClip != null)
                                StopPlayingSoundList(alsoInterrupt, soundClip.name);
                            Play(soundClip, volume, loop);
                        }
                        break;
                    case InterruptionType.DontInterruptButInterruptOthers:
                        if (!SFXIsPlaying(soundClip))
                            Play(soundClip, volume, loop);
                        StopPlayingSoundList(alsoInterrupt, soundClip.name);
                        break;
                }
            }
        }

        private void StopPlayingSoundList(List<string> alsoInterrupt, string callingSoundName)
        {
            if (alsoInterrupt.Count > 0)
            {
                foreach (string soundName in alsoInterrupt)
                {
                    GDebug.Log(callingSoundName + " : " + soundName, this, LogCategory.SOUND_MANAGER);

                    if (callingSoundName.Contains(soundName)) //dont stop the same sound you just tried to play even if its in the list
                    {
                        continue;
                    }
                    else
                    {
                        AudioClip audioClip = SFXPlaylist.FindClosestByName(soundName);
                        if (SFXIsPlaying(audioClip))
                        {
                            GDebug.Log("stopping sound fx:" + audioClip.name, this, LogCategory.SOUND_MANAGER);
                            GetPlayingSFXSource(audioClip).Stop();
                        }
                    }
                }
            }
        }

        public bool SFXIsPlaying(AudioClip soundClip)
        {
            if (null == soundClip) return false;

            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if (audioSource != null && audioSource.clip == soundClip && audioSource.isPlaying)
                    return true;
            }

            return false;
        }

        public void StopSFX(string clipName)
        {
            if (String.IsNullOrEmpty(clipName)) return;

            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if (audioSource != null && audioSource.clip != null && audioSource.clip.name == clipName && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        public void StopSFX(AudioClip clip)
        {
            if (null == clip) return;

            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if (audioSource != null && audioSource.clip != null && audioSource.clip == clip && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        public AudioSource GetPlayingSFXSource(AudioClip soundClip)
        {
            foreach (AudioSource audioSource in AudioSourcePool)
            {
                if (audioSource != null && audioSource.clip == soundClip)
                    return audioSource;
            }

            return null;
        }

        public AudioSource GetPlayingMusicSource()
        {
            return CurrentMusicAudioSource;
        }

        public void PlaySFX(AudioClip soundClip)
        {
            if (!MuteAllSound && !MuteSfx)
            {
                PlaySFX(soundClip, SFXVolume, InterruptionType.DontCare, new List<string>());
            }
        }

        public void StopAllSFX()
        {
            foreach (AudioSource audioSource in AudioSourcePool)
            {
                audioSource.Stop();
            }
        }

        private void Play(AudioClip clip, float volume, bool loop = false)
        {

            GDebug.Log("playing sound fx:" + clip.name, this, LogCategory.SOUND_MANAGER);
            AudioSource source = GetSourceFromPool();

            source.volume = volume;
            source.clip = clip;
            source.Play();
            source.loop = loop;
        }
        #endregion
    }
}
