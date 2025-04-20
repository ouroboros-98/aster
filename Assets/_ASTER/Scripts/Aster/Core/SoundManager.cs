namespace Aster.Core
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class SoundManager : MonoBehaviour
    {
        public Sound[] sounds;
        public static SoundManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
                s.source.pitch = s.pitch;
                s.source.spatialBlend = s.spatialBlend;
            }
        }

        void Start()
        {
            Play(("Background"));
        }

        public void Play(string name, bool randomPitch = false, bool isSpacial = false)
        {
        
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (randomPitch)
            {
                s.pitch = Random.Range(0.8f, 1.2f);
                s.source.pitch = s.pitch;
            }

            if (isSpacial)
            {
                s.source.spatialBlend = 1f;
            }
            else
            {
                s.source.spatialBlend = 0f;
            }
            s.source.Play();
        }
    }

}