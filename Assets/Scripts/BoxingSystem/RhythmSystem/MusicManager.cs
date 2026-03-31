using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    public AudioSource audioSource;
    public float bpm = 120f;

    private double _startTime;
    private double _beatInterval;

    void Awake() => Instance = this;

    void Start()
    {
        _beatInterval = 60d / bpm;
        _startTime = AudioSettings.dspTime;
        audioSource.Play();
    }

    public double GetCurrentBeat() => (AudioSettings.dspTime - _startTime) / _beatInterval;

    // 返回当前偏移量：负数=按早了，正数=按晚了
    public float GetBeatOffset()
    {
        double currentBeat = GetCurrentBeat();
        return (float)(currentBeat - System.Math.Round(currentBeat));
    }
}