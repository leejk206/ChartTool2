using UnityEngine;

public class NoteEditor : MonoBehaviour
{

    LineSpawner lineSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
