using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SM.Sound
{
    public class SoundController : MonoBehaviour
    {
        [Inject] private SoundManager _soundManager;

        public bool loadOnce = false;

        public List<AudioClip> sfxClips = new List<AudioClip>();
        public List<AudioClip> musicClips = new List<AudioClip>();

        public List<string> sfxClipsNames = new List<string>();
        public List<string> musicClipsNames = new List<string>();

        void Awake()
        {
            for (int i = 0; i < sfxClips.Count; i++)
            {
                if (sfxClips[i] == null)
                    continue;

                if (!_soundManager.SFXPlaylist.Contains(sfxClips[i]))
                    _soundManager.SFXPlaylist.Add(sfxClips[i]);
            }

            for (int i = 0; i < musicClips.Count; i++)
            {
                if (musicClips[i] == null)
                    continue;

                if (!_soundManager.MusicPlaylist.Contains(musicClips[i]))
                    _soundManager.MusicPlaylist.Add(musicClips[i]);
            }
        }

        void OnDisable()
        {
            if (_soundManager == null)
                return;

            if (loadOnce) //do not unload
                return;

            //        #if UNITY_EDITOR
            for (int i = 0; i < sfxClips.Count; i++)
            {
                if (sfxClips[i] == null)
                    continue;

                if (_soundManager.SFXPlaylist.Contains(sfxClips[i]))
                    _soundManager.SFXPlaylist.Remove(sfxClips[i]);

            }
            for (int i = 0; i < musicClips.Count; i++)
            {
                if (musicClips[i] == null)
                    continue;

                if (_soundManager.MusicPlaylist.Contains(musicClips[i]))
                    _soundManager.MusicPlaylist.Remove(musicClips[i]);
            }
            //        #else
            AudioClip clip;
            for (int i = 0; i < sfxClipsNames.Count; i++)
            {
                clip = _soundManager.SFXPlaylist.FindByName(sfxClipsNames[i]);
                _soundManager.StopSFX(sfxClipsNames[i]);
                if (_soundManager.SFXPlaylist.Contains(clip))
                    _soundManager.SFXPlaylist.Remove(clip);

            }
            for (int i = 0; i < musicClipsNames.Count; i++)
            {
                clip = _soundManager.MusicPlaylist.FindByName(musicClipsNames[i]);
                if (_soundManager.MusicPlaylist.Contains(clip))
                    _soundManager.MusicPlaylist.Remove(clip);
            }
            //		#endif
        }
    }
}
