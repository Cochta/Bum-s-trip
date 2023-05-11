using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandeler : MonoBehaviour
{
    private static SoundHandeler _instance;
    public static SoundHandeler Instance { get { return _instance; } }

    [SerializeField] private List<AudioClip> _bumShort;
    [SerializeField] private List<AudioClip> _bumLong;
    [SerializeField] private List<AudioClip> _bumExtraLong;
    [SerializeField] private List<AudioClip> _bumScream;
    [SerializeField] private AudioClip _bumCrazy;

    [SerializeField] private AudioClip _ambientMusic;
    [SerializeField] private AudioClip _bossMusic;

    [SerializeField] private AudioSource _enemies;
    [SerializeField] private AudioSource _environement;
    [SerializeField] private AudioSource _bum;

    private void Awake()
    {
        _instance = this;
    }
    public void StopMusic()
    {
        _environement.Stop();
    }
    public void PlayNormalMusic()
    {
        _environement.clip = _ambientMusic;
        _environement.Play();
    }
    public void PlayBossMusic()
    {
        _environement.clip = _bossMusic;
        _environement.Play();
    }
    public void PlayBumGetItem()
    {
        _bum.clip = _bumLong[0];
        _bum.Play();
    }
    public void PlayBumMove()
    {
        _bum.clip = _bumShort[2];
        _bum.Play();
    }
    public void PlayBumAttack()
    {
        _bum.clip = _bumShort[1];
        _bum.Play();
    }
    public void PlayBumBuff()
    {
        _bum.clip = _bumShort[0];
        _bum.Play();
    }

    public void PlayBumCrazy()
    {
        _bum.clip = _bumCrazy;
        _bum.Play();
    }
    public void PlayBumBadEvent()
    {
        _bum.clip = _bumScream[0];
        _bum.Play();
    }
    public void PlayBumGoodEvent()
    {
        _bum.clip = _bumScream[1];
        _bum.Play();
    }
}
