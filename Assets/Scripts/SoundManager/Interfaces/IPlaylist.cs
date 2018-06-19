using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SM.Sound
{
    public interface IPlaylist : IEnumerable<AudioClip>, IEnumerable
    {
        AudioClip CurrentClip { get; set; }
        AudioClip DefaultClip { get; }
        int Count { get; }

        void Add(AudioClip clip);
        bool Remove(AudioClip clip);
        bool Contains(AudioClip clip);

        AudioClip FindByName(string clipName);
        AudioClip FindClosestByName(string clipName);
    }
}