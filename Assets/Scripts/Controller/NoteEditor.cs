using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteEditor : MonoBehaviour
{
    Action _keypadKeyAction;

    LineSpawner lineSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Managers.Input.KeyAction -= AddNormalNote;
        Managers.Input.KeyAction -= AddHoldNote;
        Managers.Input.KeyAction -= AddSlideNote;
        Managers.Input.KeyAction -= AddUpFlickNote;
        Managers.Input.KeyAction -= AddDownFlickNote;
        Managers.Input.KeyAction -= _keypadKeyAction;

        _keypadKeyAction = () =>
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Q))
            {
                AddNormalNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.W))
            {
                AddHoldNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.E))
            {
                AddSlideNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.R))
            {
                AddUpFlickNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.T))
            {
                AddDownFlickNote();
            }
        };
        Managers.Input.KeyAction += _keypadKeyAction;
        lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNormalNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        Camera cam = Camera.main;
        Vector3 mouseScreenPos = Input.mousePosition;

        // ȭ�� ��ǥ�� ���� ��ǥ(z=0 ���)�� ��ȯ
        float zDistance = Mathf.Abs(cam.transform.position.z);
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            zDistance
        ));
        mouseWorldPos.z = 0f;

        int leftVerticalIndex;

        for (leftVerticalIndex = 0; leftVerticalIndex < lineSpawner.verticalLines.Count - 1; leftVerticalIndex++)
        {
            if (lineSpawner.verticalLines[leftVerticalIndex].transform.position.x < mouseWorldPos.x &&
                lineSpawner.verticalLines[leftVerticalIndex + 1].transform.position.x >= mouseWorldPos.x)
                break;
        }

        int closestHorizontalIndex;

        for (closestHorizontalIndex = 0; closestHorizontalIndex < lineSpawner.lines.Count - 1; closestHorizontalIndex++)
        {
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y >= mouseWorldPos.y) break;
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y < mouseWorldPos.y &&
                lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y >= mouseWorldPos.y)
            {
                float deltaDown = mouseWorldPos.y - lineSpawner.lines[closestHorizontalIndex].transform.position.y;
                float deltaUp = lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y - mouseWorldPos.y;

                if (deltaDown >= deltaUp)
                {
                    closestHorizontalIndex++;
                }
                break;
            }
        }

        int index = Managers.Chart.NormalNotes.FindIndex(note =>
    note.position == closestHorizontalIndex &&
    note.line == leftVerticalIndex);

        if (!Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                0f
            );
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
            GameObject note = Instantiate(notePrefab);
            note.transform.position = notePos;

            Vector3 scale = note.transform.localScale;
            scale.x = lineSpawner.lineGap;
            note.transform.localScale = scale;

            Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);

            Managers.Chart.NormalNotes.Add(new NormalNoteData(closestHorizontalIndex, leftVerticalIndex));
        }
        else
        {
            Managers.Chart.NormalNotes.RemoveAll(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
            if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
            {
                GameObject.Destroy(obj);
                Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
            }
        }



        Managers.Chart.SaveChart();


    }

    public void AddHoldNote()
    {


    }

    public void AddSlideNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        Camera cam = Camera.main;
        Vector3 mouseScreenPos = Input.mousePosition;

        // ȭ�� ��ǥ�� ���� ��ǥ(z=0 ���)�� ��ȯ
        float zDistance = Mathf.Abs(cam.transform.position.z);
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            zDistance
        ));
        mouseWorldPos.z = 0f;

        int leftVerticalIndex;

        for (leftVerticalIndex = 0; leftVerticalIndex < lineSpawner.verticalLines.Count - 1; leftVerticalIndex++)
        {
            if (lineSpawner.verticalLines[leftVerticalIndex].transform.position.x < mouseWorldPos.x &&
                lineSpawner.verticalLines[leftVerticalIndex + 1].transform.position.x >= mouseWorldPos.x)
                break;
        }

        int closestHorizontalIndex;

        for (closestHorizontalIndex = 0; closestHorizontalIndex < lineSpawner.lines.Count - 1; closestHorizontalIndex++)
        {
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y >= mouseWorldPos.y) break;
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y < mouseWorldPos.y &&
                lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y >= mouseWorldPos.y)
            {
                float deltaDown = mouseWorldPos.y - lineSpawner.lines[closestHorizontalIndex].transform.position.y;
                float deltaUp = lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y - mouseWorldPos.y;

                if (deltaDown >= deltaUp)
                {
                    closestHorizontalIndex++;
                }
                break;
            }
        }

        int index = Managers.Chart.SlideNotes.FindIndex(note =>
    note.position == closestHorizontalIndex &&
    note.line == leftVerticalIndex);

        if (!Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                0f
            );
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/SlideNote");
            GameObject note = Instantiate(notePrefab);
            note.transform.position = notePos;

            Vector3 scale = note.transform.localScale;
            scale.x = lineSpawner.lineGap;
            note.transform.localScale = scale;

            Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);

            Managers.Chart.SlideNotes.Add(new SlideNoteData(closestHorizontalIndex, leftVerticalIndex));
        }
        else
        {
            Managers.Chart.SlideNotes.RemoveAll(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
            if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
            {
                GameObject.Destroy(obj);
                Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
            }
        }



        Managers.Chart.SaveChart();
    }

    public void AddUpFlickNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }
        
        Camera cam = Camera.main;
        Vector3 mouseScreenPos = Input.mousePosition;

        // ȭ�� ��ǥ�� ���� ��ǥ(z=0 ���)�� ��ȯ
        float zDistance = Mathf.Abs(cam.transform.position.z);
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            zDistance
        ));
        mouseWorldPos.z = 0f;

        int leftVerticalIndex;

        for (leftVerticalIndex = 0; leftVerticalIndex < lineSpawner.verticalLines.Count - 1; leftVerticalIndex++)
        {
            if (lineSpawner.verticalLines[leftVerticalIndex].transform.position.x < mouseWorldPos.x &&
                lineSpawner.verticalLines[leftVerticalIndex + 1].transform.position.x >= mouseWorldPos.x)
                break;
        }

        int closestHorizontalIndex;

        for (closestHorizontalIndex = 0; closestHorizontalIndex < lineSpawner.lines.Count - 1; closestHorizontalIndex++)
        {
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y >= mouseWorldPos.y) break;
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y < mouseWorldPos.y &&
                lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y >= mouseWorldPos.y)
            {
                float deltaDown = mouseWorldPos.y - lineSpawner.lines[closestHorizontalIndex].transform.position.y;
                float deltaUp = lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y - mouseWorldPos.y;

                if (deltaDown >= deltaUp)
                {
                    closestHorizontalIndex++;
                }
                break;
            }
        }

        int index = Managers.Chart.UpFlickNotes.FindIndex(note =>
    note.position == closestHorizontalIndex &&
    note.line == leftVerticalIndex);

        if (!Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                0f
            );
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/UpFlickNote");
            GameObject note = Instantiate(notePrefab);
            note.transform.position = notePos;

            Vector3 scale = note.transform.localScale;
            scale.x = lineSpawner.lineGap;
            note.transform.localScale = scale;

            Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);

            Managers.Chart.UpFlickNotes.Add(new UpFlickNoteData(closestHorizontalIndex, leftVerticalIndex));
        }
        else
        {
            Managers.Chart.UpFlickNotes.RemoveAll(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
            if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
            {
                GameObject.Destroy(obj);
                Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
            }
        }
        Managers.Chart.SaveChart();
    }

    public void AddDownFlickNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }
        
        Camera cam = Camera.main;
        Vector3 mouseScreenPos = Input.mousePosition;

        // ȭ�� ��ǥ�� ���� ��ǥ(z=0 ���)�� ��ȯ
        float zDistance = Mathf.Abs(cam.transform.position.z);
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            zDistance
        ));
        mouseWorldPos.z = 0f;

        int leftVerticalIndex;

        for (leftVerticalIndex = 0; leftVerticalIndex < lineSpawner.verticalLines.Count - 1; leftVerticalIndex++)
        {
            if (lineSpawner.verticalLines[leftVerticalIndex].transform.position.x < mouseWorldPos.x &&
                lineSpawner.verticalLines[leftVerticalIndex + 1].transform.position.x >= mouseWorldPos.x)
                break;
        }

        int closestHorizontalIndex;

        for (closestHorizontalIndex = 0; closestHorizontalIndex < lineSpawner.lines.Count - 1; closestHorizontalIndex++)
        {
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y >= mouseWorldPos.y) break;
            if (lineSpawner.lines[closestHorizontalIndex].transform.position.y < mouseWorldPos.y &&
                lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y >= mouseWorldPos.y)
            {
                float deltaDown = mouseWorldPos.y - lineSpawner.lines[closestHorizontalIndex].transform.position.y;
                float deltaUp = lineSpawner.lines[closestHorizontalIndex + 1].transform.position.y - mouseWorldPos.y;

                if (deltaDown >= deltaUp)
                {
                    closestHorizontalIndex++;
                }
                break;
            }
        }

        int index = Managers.Chart.DownFlickNotes.FindIndex(note =>
    note.position == closestHorizontalIndex &&
    note.line == leftVerticalIndex);

        if (!Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                0f
            );
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/DownFlickNote");
            GameObject note = Instantiate(notePrefab);
            note.transform.position = notePos;

            Vector3 scale = note.transform.localScale;
            scale.x = lineSpawner.lineGap;
            note.transform.localScale = scale;

            Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);

            Managers.Chart.DownFlickNotes.Add(new DownFlickNoteData(closestHorizontalIndex, leftVerticalIndex));
        }
        else
        {
            Managers.Chart.DownFlickNotes.RemoveAll(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
            if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
            {
                GameObject.Destroy(obj);
                Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
            }
        }
        Managers.Chart.SaveChart();

    }
}
