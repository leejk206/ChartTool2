using System;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource bgmSource;
    public float skipSeconds = 10f;

    Action _keypadKeyAction;

    CameraMover cameraMover;

    private void Awake()
    {
        bgmSource = gameObject.GetComponent<AudioSource>();
        if (bgmSource == null)
        {
            Debug.LogError("[BGMController] AudioSource 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        cameraMover = GameObject.Find("Main Camera").GetComponent<CameraMover>();

        bgmSource.clip = audioClip;

        if (cameraMover == null)
        {
            Debug.LogError("[BGMController] CameraMover를 찾을 수 없습니다.");
        }

        if (bgmSource.clip == null)
        {
            Debug.LogWarning("[BGMController] AudioSource에 clip이 할당되어 있지 않습니다.");
        }

        Managers.Input.KeyAction -= SkipForward10s;
        Managers.Input.KeyAction -= SkipBackward10s;
        Managers.Input.KeyAction -= SkipForward1s;
        Managers.Input.KeyAction -= SkipBackward1s;
        Managers.Input.KeyAction -= PlayOrPause;
        _keypadKeyAction = () =>
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SkipForward10s();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SkipBackward10s();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SkipForward1s();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SkipBackward1s();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayOrPause();
            }

        };
        Managers.Input.KeyAction += _keypadKeyAction;
    }

    public void PlayOrPause()
    {
        cameraMover.Play();
    }

    public void PlayOrPauseBGM()
    {
        if (!bgmSource.isPlaying)
        {
            if (bgmSource.clip == null)
            {
                Debug.LogError("[BGMController] 재생 시도 실패: clip이 null입니다.");
                return;
            }

            bgmSource.Play();
            Debug.Log("[BGMController] BGM 재생 시작");
        }
        else
        {
            bgmSource.Pause();
            Debug.Log("[BGMController] BGM 일시정지");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.time = 0f;
        cameraMover.Reset();
    }

    public void SkipForward10s()
    {
        float newTime = bgmSource.time + 10f;
        bgmSource.time = Mathf.Min(newTime, bgmSource.clip.length);
        cameraMover.SkipForward10s();
    }

    public void SkipForward1s()
    {
        float newTime = bgmSource.time + 1f;
        bgmSource.time = Mathf.Min(newTime, bgmSource.clip.length);
        cameraMover.SkipForward1s();
    }

    public void SkipBackward10s()
    {
        float newTime = bgmSource.time - 10f;
        bgmSource.time = Mathf.Max(newTime, 0f);
        cameraMover.SkipBackward10s();
    }

    public void SkipBackward1s()
    {
        float newTime = bgmSource.time - 1f;
        bgmSource.time = Mathf.Max(newTime, 0f);
        cameraMover.SkipBackward1s();
    }


}