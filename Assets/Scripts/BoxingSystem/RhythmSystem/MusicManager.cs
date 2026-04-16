using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    // 存储从 Wwise 回调传回的实时节拍数据
    private double _lastBeatDspTime;
    private double _beatInterval; 

    void Awake() => Instance = this;

    // 当 AudioManager 播放音乐时，确保包含这个回调
    public void InitBeatData(float bpm)
    {
        _beatInterval = 60d / bpm;
        _lastBeatDspTime = AudioSettings.dspTime;
    }

    // Wwise 每跳一拍，都会触发这个由 AudioManager 传过来的函数
    public void OnWwiseBeatFired()
    {
        _lastBeatDspTime = AudioSettings.dspTime;
        // Debug.Log("节拍对齐！");
    }

    public float GetBeatOffset()
    {
        double elapsed = AudioSettings.dspTime - _lastBeatDspTime;
        // 算出当前时间距离上一拍和下一拍哪个更近
        double offset = elapsed / _beatInterval;
        if (offset > 0.5) offset -= 1.0; 

        return (float)offset;
    }
}