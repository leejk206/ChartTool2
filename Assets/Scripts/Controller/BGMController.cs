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
            Debug.LogError("[BGMController] AudioSource ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void Start()
    {
        cameraMover = GameObject.Find("Main Camera").GetComponent<CameraMover>();

        bgmSource.clip = audioClip;

        if (cameraMover == null)
        {
            Debug.LogError("[BGMController] CameraMover�� ã�� �� �����ϴ�.");
        }

        if (bgmSource.clip == null)
        {
            Debug.LogWarning("[BGMController] AudioSource�� clip�� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
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
                Debug.LogError("[BGMController] ��� �õ� ����: clip�� null�Դϴ�.");
                return;
            }

            bgmSource.Play();
            Debug.Log("[BGMController] BGM ��� ����");
        }
        else
        {
            bgmSource.Pause();
            Debug.Log("[BGMController] BGM �Ͻ�����");
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