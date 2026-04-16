// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [Serializable]
// public class AudioInfoWwise
// {
//     public string audioName;
//     public uint playingID; // Wwise 播放 ID
// }

// public class AudioManager : SingletonPersistent<AudioManager>
// {
//     public List<AudioInfoWwise> bgmInfoList = new List<AudioInfoWwise>();
//     public List<AudioInfoWwise> sfxInfoList = new List<AudioInfoWwise>();

//     // 对应 Wwise 中的 RTPC 参数名称
//     private const string MAIN_VOLUME_RTPC = "MainVolume";
//     private const string BGM_VOLUME_RTPC = "BGMVolume";
//     private const string SFX_VOLUME_RTPC = "SFXVolume";

//     protected override void Awake()
//     {
//         base.Awake();
//         // 初始化音量（如果 Wwise 项目中设置了这些 RTPC）
//         float main = PlayerPrefs.GetFloat("MainVolume", 100f);
//         ChangeMainVolume(main);
//     }

//     private void Start()
//     {
//         // 初始播放指定的 BGM
//         PlayBgm("Play_Music_Drums_Test");
//     }

//     /// <summary>
//     /// 播放 BGM (Wwise Event)
//     /// </summary>
//     public void PlayBgm(string eventName)
//     {
//         // 如果正在播放相同的 BGM，可以根据需求选择是否跳过
//         if (bgmInfoList.Exists(x => x.audioName == eventName)) return;

//         // PostEvent 返回一个 PlayingID
//         uint id = AkSoundEngine.PostEvent(eventName, gameObject);
        
//         if (id != AkSoundEngine.AK_INVALID_PLAYING_ID)
//         {
//             bgmInfoList.Add(new AudioInfoWwise { audioName = eventName, playingID = id });
//             Debug.Log($"Wwise BGM Started: {eventName}");
//         }
//     }

//     /// <summary>
//     /// 停止 BGM
//     /// </summary>
//     public void StopBgm(string eventName, int fadeMs = 500)
//     {
//         var info = bgmInfoList.Find(x => x.audioName == eventName);
//         if (info != null)
//         {
//             // 使用 ExecuteActionOnEvent 来实现停止和淡出
//             AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, fadeMs, AkCurveInterpolation.AkCurveInterpolation_Linear);
//             bgmInfoList.Remove(info);
//         }
//     }

//     /// <summary>
//     /// 播放音效
//     /// </summary>
//     public void PlaySfx(string eventName)
//     {
//         uint id = AkSoundEngine.PostEvent(eventName, gameObject);
//         if (id != AkSoundEngine.AK_INVALID_PLAYING_ID)
//         {
//             AudioInfoWwise info = new AudioInfoWwise { audioName = eventName, playingID = id };
//             sfxInfoList.Add(info);
//             // 监控音效结束以清理列表
//             StartCoroutine(WaitSfxEnd(id, info));
//         }
//     }

//     // 监控播放结束的简易协程
//     private IEnumerator WaitSfxEnd(uint playingID, AudioInfoWwise info)
//     {
//         // 注意：Wwise 建议通过 Callback 监控，这里是简单的逻辑示意
//         yield return new WaitForSeconds(1.0f); // 这是一个占位，实际建议用 Callback
//         sfxInfoList.Remove(info);
//     }

//     /// <summary>
//     /// 修改全局音量 (通过 RTPC)
//     /// </summary>
//     public void ChangeMainVolume(float value)
//     {
//         // 假设 Wwise RTPC 范围是 0-100
//         AkSoundEngine.SetRTPCValue(MAIN_VOLUME_RTPC, value);
//         PlayerPrefs.SetFloat("MainVolume", value);
//     }

//     public void ChangeBgmVolume(float value)
//     {
//         AkSoundEngine.SetRTPCValue(BGM_VOLUME_RTPC, value);
//     }
// }
