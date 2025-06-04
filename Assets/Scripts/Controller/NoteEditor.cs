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
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddNormalNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddHoldNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddSlideNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddUpFlickNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
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
        Camera cam = Camera.main;
        Vector3 mouseScreenPos = Input.mousePosition;

        // 화면 좌표를 월드 좌표(z=0 평면)로 변환
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

        if (index == -1)
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
            Managers.Chart.NormalNotes.RemoveAt(index);
            Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
        }



        Managers.Chart.SaveChart();


    }

    public void AddHoldNote()
    {


    }

    public void AddSlideNote()
    {

    }

    public void AddUpFlickNote()
    {

    }

    public void AddDownFlickNote()
    {


    }
}
