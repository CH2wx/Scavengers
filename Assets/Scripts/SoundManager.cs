/// <summary>
/// 文档作用：控制音效的播放
/// 作者：陈鸿
/// 编辑时间：9/8
/// 备注：
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip [] clips)   //params允许程序员传递用逗号分隔的相同的类型的参数列表
    {
        int randomIndex = Random.Range(0, clips.Length);    //该变量用于在数组中随机选择一个音频剪辑并播放
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
