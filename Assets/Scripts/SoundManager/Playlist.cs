using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SM.Sound
{
    public class Playlist : IPlaylist
    {
        private IList<AudioClip> _clips;

        public AudioClip DefaultClip
        {
            get;
            private set;
        }

        public AudioClip CurrentClip
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return _clips.Count;
            }
        }

        public Playlist()
        {
            _clips = new List<AudioClip>();
            CurrentClip = new AudioClip();
            DefaultClip = new AudioClip();
        }

        public Playlist(IEnumerable<AudioClip> clips)
        {
            if (null == clips)
                throw new System.ArgumentNullException("clips");

            _clips = new List<AudioClip>(clips);
            CurrentClip = new AudioClip();
            DefaultClip = new AudioClip();
        }

        public Playlist(IEnumerable<AudioClip> clips, AudioClip defaultClip)
        {
            if (null == clips)
                throw new System.ArgumentNullException("clips");
            if (null == defaultClip)
                throw new System.ArgumentNullException("defaultClip");

            _clips = new List<AudioClip>(clips);
            CurrentClip = new AudioClip();
            DefaultClip = defaultClip;
        }

        public void Add(AudioClip clip)
        {
            _clips.Add(clip);
        }

        public bool Remove(AudioClip clip)
        {
            return _clips.Remove(clip);
        }

        public bool Contains(AudioClip clip) 
        {
            return _clips.Contains(clip);
        }

        public AudioClip FindByName(string clipName)
        {
            if (null == clipName)
                throw new System.ArgumentNullException("name");

            foreach (AudioClip clip in _clips)
            {
                if (clip != null && clip.name == clipName)
                    return clip;
            }

            return DefaultClip;
        }

        public AudioClip FindClosestByName(string clipName)
        {
            if (null == clipName)
                throw new System.ArgumentNullException("clipName");
        
            List<AudioClip> randomSet = new List<AudioClip>();

            foreach (AudioClip clip in _clips)
            {
                if (clip != null && clip.name.IndexOf(clipName, System.StringComparison.Ordinal) > -1)
                    randomSet.Add(clip);
            }

            if (randomSet.Count > 0)
            {
                return randomSet[Random.Range(0, randomSet.Count)];
            }

            return DefaultClip;
        }

        #region IEnumerable

        public IEnumerator GetEnumerator()
        {
            return _clips.GetEnumerator();
        }

        IEnumerator<AudioClip> IEnumerable<AudioClip>.GetEnumerator()
        {
            return _clips.GetEnumerator();
        }

        #endregion
    }
}
