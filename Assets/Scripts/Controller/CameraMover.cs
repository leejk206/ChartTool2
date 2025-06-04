using UnityEngine;
using UnityEngine.UI;

public class CameraMover : MonoBehaviour
{
    public float BPM = 120f;
    public float speed; // units per second
    public float beatsPerSecond;
    private bool isPlaying;

    BGMController bgmController;

    LineSpawner lineSpawner;

    Text playButtonText;

    public float scrollSpeed = 10f;

    void Start()
    {
        lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();
        beatsPerSecond = BPM / 60;
        speed = 16 * beatsPerSecond;
        isPlaying = false;

        playButtonText = GameObject.Find("PlayButtonText").GetComponent<Text>();
        bgmController = GameObject.Find("BGMController").GetComponent<BGMController>();
    }

    void Update()
    {
        if (isPlaying)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f) // 민감도 필터
            {
                Vector3 pos = transform.position;
                pos.y += scroll * scrollSpeed;
                transform.position = pos;
            }
        }
        
    }

    public void Play()
    {
        if (isPlaying)
        {
            isPlaying = false;
            playButtonText.text = "Play";
            bgmController.PlayOrPauseBGM();
        }

        else
        {
            isPlaying = true;
            playButtonText.text = "Pause";
            bgmController.PlayOrPauseBGM();
        }
    }

    public void SkipForward10s()
    {
        transform.position += Vector3.up * speed * 10f;
    }

    public void SkipForward1s()
    {
        transform.position += Vector3.up * speed * 1f;
    }

    public void SkipBackward10s()
    {
        float newy = Mathf.Max(0, (transform.position - Vector3.up * speed * 10f).y);
        transform.position = new Vector3(0, newy, -10);
    }

    public void SkipBackward1s()
    {
        float newy = Mathf.Max(0, (transform.position - Vector3.up * speed * 1f).y);
        transform.position = new Vector3(0, newy, -10);
    }

    public void Reset()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        // todo bgm 초기화

        if (isPlaying)
        {
            isPlaying = false;
            playButtonText.text = "Play";
            bgmController.PlayOrPauseBGM();
        }
    }
}
