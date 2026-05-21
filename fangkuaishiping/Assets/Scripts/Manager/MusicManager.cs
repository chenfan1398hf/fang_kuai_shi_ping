using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : BaseManager<MusicManager>
{
    
    private const string musicPath = "Music/";
    private AudioSource bkMusic = null;
    private float bkValue = 1f;

    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float yxValue = 1f;

    public MusicManager()
    {
        GameManager.instance.AddUpdateListener(CheckUpdate);
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="_name">音乐名</param>
    public void PlayBkMusic(string _name)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //背景音乐较大异步加载背景音乐
        GameManager.instance.StartCoroutine(LoadBkMusic(bkMusic, musicPath+ _name));
    }

    IEnumerator LoadBkMusic(AudioSource _audio, string _path)
    {
        ResourceRequest req = Resources.LoadAsync<AudioClip>(_path);
        yield return req;

        _audio.clip = req.asset as AudioClip;
        _audio.volume = bkValue;
        _audio.loop = true;
        _audio.Play();

    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBkMusic()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Stop();
    }
    /// <summary>
    /// 改变音量大小
    /// </summary>
    /// <param name="_value"></param>
    public void ChangeBkValue(float _value)
    {
        bkValue = _value;
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.volume = bkValue;
    }




    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_name"></param>
    public void PlaySound(string _name)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }
        var audio = soundObj.AddComponent<AudioSource>();
        AudioClip clip = ResourcesLoad.instance.Load<AudioClip>(musicPath + _name);
        audio.clip = clip;
        audio.volume = yxValue;
        audio.loop = false;
        audio.Play();

        soundList.Add(audio);
    }
    /// <summary>
    /// 关闭音效
    /// </summary>
    /// <param name="_audioSource"></param>
    public void StopSound(AudioSource _audioSource)
    {
        if (soundList.Contains(_audioSource))
        {
            soundList.Remove(_audioSource);
            _audioSource.Stop();
            GameObject.Destroy(_audioSource);
        }
    }
    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="_value"></param>
    public void ChangeSoundValue(float _value)
    {
        yxValue = _value;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = yxValue;
        }

    }
    /// <summary>
    /// 播放完的音效移除
    /// </summary>
    private void CheckUpdate()
    {
        for (int i = soundList.Count -1; i >= 0; --i)
        {
            if (! soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
}
