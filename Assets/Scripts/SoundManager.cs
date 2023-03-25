using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SniperHunter
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager sharedInstance = null;
        [SerializeField] private AudioSource sfxAuidoSource;
        //[SerializeField] private AudioSource backgroundAudioSource;

        //public AudioClip backgroudMusic;
        public AudioClip ScopeOn;
        public AudioClip ScopeOff;
        public List<AudioClip> bulletSounds;
        //public AudioClip tapToStart;
        public AudioClip missionComplete;
        //public AudioClip missionFailed;
        //public AudioClip warningAlert;
        public AudioClip headShot;
        //public List<AudioClip> enemyShots;

        //public AudioClip btnTap;
        //public AudioClip coinFly;

        public List<AudioClip> killSFX = new List<AudioClip>();
        //public AudioClip missShot;
        //public AudioClip oppnentBullethit;
        //public AudioClip victoryGunfireSFX;
        //public AudioClip zoomBar;

        public static SoundManager SharedManager()
        {
            return sharedInstance;
        }

        private void Awake()
        {
            if (sharedInstance == null)
            {
                sharedInstance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }

        private void Start()
        {
            //PlayBackGroundAudio();
        }

        //public void PlayRandomEnemyShot(AudioSource audioSource)
        //{
        //    audioSource.PlayOneShot(GetRandomEnemyShots());
        //    PlaySFX(GetRandomEnemyShots());
        //}

        //private AudioClip GetRandomEnemyShots()
        //{
        //    return enemyShots[Random.Range(0, enemyShots.Count)];
        //}

        //public void PlayBackGroundAudio()
        //{
        //    backgroundAudioSource.clip = backgroudMusic;
        //    backgroundAudioSource.Play();
        //    backgroundAudioSource.loop = true;
        //}

        public void PlaySFX(AudioClip audioClip)
        {
            sfxAuidoSource.PlayOneShot(audioClip);
        }

        //public void PlayBulletSound(string soundId)
        //{
        //    for (int i = 0; i < bulletSounds.Count; i++)
        //    {
        //        if (bulletSounds[i] != null)
        //        {
        //            if (bulletSounds[i].name == soundId)
        //            {
        //                sfxAuidoSource.PlayOneShot(bulletSounds[i]);
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}