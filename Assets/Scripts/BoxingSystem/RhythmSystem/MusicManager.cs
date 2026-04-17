using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    [Header("音乐设置")]
    public string musicEventName = "Play_Music_Drums_Test";
    public float bpm = 120f;

    private double _beatInterval;
    private uint _playingID;
    private double _lastBeatDspTime;

    void Awake() => Instance = this;

    void Start()
    {
        _beatInterval = 60d / bpm;
        // 核心修正：必须主动播放才能开启节拍监听
        PlayMusic();
    }

    void PlayMusic()
    {
        // 这里的 (uint)AkCallbackType.AK_MusicSyncBeat 是关键，它告诉 Wwise 每拍都通知 Unity
        _playingID = AkUnitySoundEngine.PostEvent(musicEventName, gameObject,
            (uint)AkCallbackType.AK_MusicSyncBeat,
            MusicCallback, null);

        if (_playingID == AkUnitySoundEngine.AK_INVALID_PLAYING_ID)
        {
            Debug.LogError($"播放失败！请检查 Event '{musicEventName}' 是否在 SoundBank 中");
        }
    }

    private void MusicCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        if (in_type == AkCallbackType.AK_MusicSyncBeat)
        {
            // 只有这里跑通了，偏移量才会变化
            _lastBeatDspTime = AudioSettings.dspTime;
            Debug.Log("<color=yellow>节拍回调触发成功！</color>");
        }
    }

    public float GetBeatOffset()
    {
        // 如果 _lastBeatDspTime 没被回调更新过，这里算出来就会很奇怪或始终为0
        double elapsed = AudioSettings.dspTime - _lastBeatDspTime;
        double offsetInBeats = elapsed / _beatInterval;

        if (offsetInBeats > 0.5) offsetInBeats -= 1.0;

        return (float)offsetInBeats;
    }

    void OnDestroy()
    {
        if (_playingID != 0) AkUnitySoundEngine.StopPlayingID(_playingID);
    }
}