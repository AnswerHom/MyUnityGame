using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : BaseManager<MusicManager> {
    public static string ROOT_PATH = "Music/";
    //背景音乐
    private AudioSource _backSource;
    private float _backVolume = 1;
    private GameObject _musicPlayer;
    //音效
    private List<AudioSource> _soundSourceList;
    private GameObject _soundPlayer;
    private float _soundVolume = 1;

    public MusicManager()
    {
        _soundSourceList = new List<AudioSource>();
        MonoManager.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = _soundSourceList.Count - 1; i >= 0; i--)
        {
           AudioSource source = _soundSourceList[i];
            if (!source.isPlaying)
            {
                _soundSourceList.RemoveAt(i);
                GameObject.Destroy(source);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="path"></param>
    public void PlayBackMusic(string path)
    {
        ResManager.GetInstance().LoadResAsync<AudioClip>(MusicManager.ROOT_PATH + path, (clip) =>
        {
            if (!_backSource)
            {
                _musicPlayer = new GameObject("MusicPlayer");
                _backSource = _musicPlayer.AddComponent<AudioSource>();
            }
            _backSource.clip = clip;
            _backSource.volume = _backVolume;
            _backSource.loop = true;
            _backSource.Play();
        });
    }

    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBackMusic()
    {
        if (_backSource)
        {
            _backSource.Stop();
        }
    }

    /// <summary>
    /// 暂停播放背景音乐
    /// </summary>
    public void PauseBackMusic()
    {
        if (_backSource)
        {
            _backSource.Pause();
        }
    }

    /// <summary>
    /// 设置背景音乐音量大小
    /// </summary>
    /// <param name="value"></param>
    public void SetBackVolume(float value)
    {
        _backVolume = value;
        if (_backSource)
        {
            _backSource.volume = value;
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="loop"></param>
    /// <param name="func"></param>
    public void PlaySound(string path,bool loop = false,UnityAction func = null)
    {
        ResManager.GetInstance().LoadResAsync<AudioClip>(MusicManager.ROOT_PATH + path, (clip) =>
        {
            if (!_soundPlayer)
            {
                _soundPlayer = new GameObject("SoundPlayer");
            }
            AudioSource soundSource = _soundPlayer.AddComponent<AudioSource>();
            soundSource.clip = clip;
            soundSource.volume = _soundVolume;
            soundSource.loop = loop;
            soundSource.Play();
            if (func != null)
                func();
            _soundSourceList.Add(soundSource);
        });
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (_soundSourceList.Contains(source)) {
            _soundSourceList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    /// <param name="value"></param>
    public void SetSoundVolume(float value)
    {
        _soundVolume = value;
        for (int i = 0; i < _soundSourceList.Count; i++)
        {
            AudioSource sound = _soundSourceList[i];
            sound.volume = value;
        }
    }
}
