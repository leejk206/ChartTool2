using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteEditor : MonoBehaviour
{
    Action _keypadKeyAction;

    LineSpawner lineSpawner;

    int leftVerticalIndex;
    int closestHorizontalIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Managers.Input.KeyAction -= AddNormalNote;
        Managers.Input.KeyAction -= AddHoldNote;
        Managers.Input.KeyAction -= AddSlideNote;
        Managers.Input.KeyAction -= AddFlickNote;
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
                AddFlickNote();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                IncreaseNoteLength();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                DecreaseNoteLength();
            }
            }
            ;
            Managers.Input.KeyAction += _keypadKeyAction;
            lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();
        }
        


    // Update is called once per frame
    void GetMousePosition()
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

        for (leftVerticalIndex = 0; leftVerticalIndex < lineSpawner.verticalLines.Count - 1; leftVerticalIndex++)
        {
            if (lineSpawner.verticalLines[leftVerticalIndex].transform.position.x < mouseWorldPos.x &&
                lineSpawner.verticalLines[leftVerticalIndex + 1].transform.position.x >= mouseWorldPos.x)
                break;
        }



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
    }

    public void AddNormalNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();


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

            Managers.Chart.NormalNotes.Add(new NormalNoteData(closestHorizontalIndex, leftVerticalIndex, 1));
        }
        else
        {
            RemoveNoteFromJson(leftVerticalIndex, closestHorizontalIndex);
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
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();

        if (Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            if (Managers.Chart.HoldNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.HoldNotes.FindIndex(note =>
                    note.position == closestHorizontalIndex &&
                    note.line == leftVerticalIndex);
                // HoldNote 종류 변경 로직
                HoldNoteData result = Managers.Chart.HoldNotes.FirstOrDefault(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                if (result.noteType == 0)
                {
                    result.noteType = 1;
                    Managers.Chart.HoldNotes[index] = result;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        GameObject.Destroy(obj);
                        Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                    }

                    Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                    );
                    GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteMid");
                    GameObject note = Instantiate(notePrefab);
                    note.transform.position = notePos;

                    Vector3 scale = note.transform.localScale;
                    scale.x = lineSpawner.lineGap * result.length;
                    note.transform.localScale = scale;

                    Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);
                }
                else if (result.noteType == 1)
                {
                    result.noteType = 2;
                    Managers.Chart.HoldNotes[index] = result;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        GameObject.Destroy(obj);
                        Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                    }

                    Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                    );
                    GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteEnd");
                    GameObject note = Instantiate(notePrefab);
                    note.transform.position = notePos;

                    Vector3 scale = note.transform.localScale;
                    scale.x = lineSpawner.lineGap * result.length;
                    note.transform.localScale = scale;

                    Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);
                }
                else if (result.noteType == 2)
                {
                    result.noteType = 0;
                    Managers.Chart.HoldNotes[index] = result;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        GameObject.Destroy(obj);
                        Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                    }

                    Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                    );
                    GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteStart");
                    GameObject note = Instantiate(notePrefab);
                    note.transform.position = notePos;

                    Vector3 scale = note.transform.localScale;
                    scale.x = lineSpawner.lineGap * result.length;
                    note.transform.localScale = scale;

                    Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);
                }
            }
            else
            {
                RemoveNoteFromJson(leftVerticalIndex, closestHorizontalIndex);
                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    GameObject.Destroy(obj);
                    Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                }
            }
        }
        else
        {
            int index = Managers.Chart.HoldNotes.FindIndex(note =>
            note.position == closestHorizontalIndex &&
            note.line == leftVerticalIndex);

            if (!Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
            {
                Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                );
                GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteStart");
                GameObject note = Instantiate(notePrefab);
                note.transform.position = notePos;

                Vector3 scale = note.transform.localScale;
                scale.x = lineSpawner.lineGap;
                note.transform.localScale = scale;

                Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);

                Managers.Chart.HoldNotes.Add(new HoldNoteData(closestHorizontalIndex, leftVerticalIndex, 0, 0, 1));
            }
        }
        Managers.Chart.SaveChart();
    }

    public void AddSlideNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();

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

            Managers.Chart.SlideNotes.Add(new SlideNoteData(closestHorizontalIndex, leftVerticalIndex, 1));
        }
        else
        {
            RemoveNoteFromJson(leftVerticalIndex, closestHorizontalIndex);
            if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
            {
                GameObject.Destroy(obj);
                Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
            }
        }



        Managers.Chart.SaveChart();
    }

    public void AddFlickNote()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();

        if (Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {
            if (Managers.Chart.FlickNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.FlickNotes.FindIndex(note =>
                    note.position == closestHorizontalIndex &&
                    note.line == leftVerticalIndex);
                // FlickNote 종류 변경 로직
                FlickNoteData result = Managers.Chart.FlickNotes.FirstOrDefault(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                if (result.direction == 0) // Up Flick Note인 경우
                {
                    result.direction = 1;
                    Managers.Chart.FlickNotes[index] = result;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        GameObject.Destroy(obj);
                        Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                    }

                    Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                    );
                    GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/DownFlickNote");
                    GameObject note = Instantiate(notePrefab);
                    note.transform.position = notePos;

                    Vector3 scale = note.transform.localScale;
                    scale.x = lineSpawner.lineGap * result.length;
                    note.transform.localScale = scale;

                    Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);
                }
                else if (result.direction == 1)
                {
                    result.direction = 0;
                    Managers.Chart.FlickNotes[index] = result;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        GameObject.Destroy(obj);
                        Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                    }

                    Vector3 notePos = new Vector3(
                    lineSpawner.verticalLines[leftVerticalIndex].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[closestHorizontalIndex].transform.position.y,
                    0f
                    );
                    GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/UpFlickNote");
                    GameObject note = Instantiate(notePrefab);
                    note.transform.position = notePos;

                    Vector3 scale = note.transform.localScale;
                    scale.x = lineSpawner.lineGap * result.length;
                    note.transform.localScale = scale;

                    Managers.Chart.Notes[leftVerticalIndex].Add(closestHorizontalIndex, note);
                }
            }
            else
            {
                RemoveNoteFromJson(leftVerticalIndex, closestHorizontalIndex);
                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    GameObject.Destroy(obj);
                    Managers.Chart.Notes[leftVerticalIndex].Remove(closestHorizontalIndex);
                }
            }
        }
        else
        {
            int index = Managers.Chart.FlickNotes.FindIndex(note =>
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

                Managers.Chart.FlickNotes.Add(new FlickNoteData(closestHorizontalIndex, leftVerticalIndex, 1, 0));
            }
        }
        Managers.Chart.SaveChart();
    }

    public void IncreaseNoteLength()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();

        if (Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {

            if (Managers.Chart.NormalNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.NormalNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                NormalNoteData updated = Managers.Chart.NormalNotes[index];
                if (updated.length <= 21)
                    updated.length += 1;
                Managers.Chart.NormalNotes[index] = updated;

                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    Vector3 scale = obj.transform.localScale;
                    scale.x = lineSpawner.lineGap * updated.length;
                    obj.transform.localScale = scale;

                    Vector3 newPos = obj.transform.position;
                    newPos.x += (lineSpawner.lineGap / 2);

                    obj.transform.position = newPos;
                }
            }
            else if (Managers.Chart.HoldNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.HoldNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                HoldNoteData updated = Managers.Chart.HoldNotes[index];
                if (updated.length <= 21)
                    updated.length += 1;
                Managers.Chart.HoldNotes[index] = updated;

                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    Vector3 scale = obj.transform.localScale;
                    scale.x = lineSpawner.lineGap * updated.length;
                    obj.transform.localScale = scale;

                    Vector3 newPos = obj.transform.position;
                    newPos.x += (lineSpawner.lineGap / 2);

                    obj.transform.position = newPos;
                }
            }
            else if (Managers.Chart.SlideNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.SlideNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                SlideNoteData updated = Managers.Chart.SlideNotes[index];
                if (updated.length <= 21)
                    updated.length += 1;
                Managers.Chart.SlideNotes[index] = updated;

                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    Vector3 scale = obj.transform.localScale;
                    scale.x = lineSpawner.lineGap * updated.length;
                    obj.transform.localScale = scale;

                    Vector3 newPos = obj.transform.position;
                    newPos.x += (lineSpawner.lineGap / 2);

                    obj.transform.position = newPos;
                }
            }
            else if (Managers.Chart.FlickNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.FlickNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                FlickNoteData updated = Managers.Chart.FlickNotes[index];
                if (updated.length <= 21)
                    updated.length += 1;
                Managers.Chart.FlickNotes[index] = updated;

                if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                {
                    Vector3 scale = obj.transform.localScale;
                    scale.x = lineSpawner.lineGap * updated.length;
                    obj.transform.localScale = scale;

                    Vector3 newPos = obj.transform.position;
                    newPos.x += (lineSpawner.lineGap / 2);

                    obj.transform.position = newPos;
                }
            }
            Managers.Chart.SaveChart();
        }
        else return;
    }

    public void DecreaseNoteLength()
    {
        if (Managers.Chart.isLoaded == false)
        {
            Managers.Chart.LoadChart();
        }

        GetMousePosition();

        if (Managers.Chart.Notes[leftVerticalIndex].ContainsKey(closestHorizontalIndex))
        {

            if (Managers.Chart.NormalNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.NormalNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                NormalNoteData updated = Managers.Chart.NormalNotes[index];
                if (updated.length > 1)
                {
                    updated.length -= 1;
                    Managers.Chart.NormalNotes[index] = updated;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        Vector3 scale = obj.transform.localScale;
                        scale.x = lineSpawner.lineGap * updated.length;
                        obj.transform.localScale = scale;

                        Vector3 newPos = obj.transform.position;
                        newPos.x -= (lineSpawner.lineGap / 2);

                        obj.transform.position = newPos;
                    }
                }
            }
            else if (Managers.Chart.HoldNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.HoldNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                HoldNoteData updated = Managers.Chart.HoldNotes[index];
                if (updated.length > 1)
                {
                    updated.length -= 1;
                    Managers.Chart.HoldNotes[index] = updated;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        Vector3 scale = obj.transform.localScale;
                        scale.x = lineSpawner.lineGap * updated.length;
                        obj.transform.localScale = scale;

                        Vector3 newPos = obj.transform.position;
                        newPos.x -= (lineSpawner.lineGap / 2);

                        obj.transform.position = newPos;
                    }
                }
            }
            else if (Managers.Chart.SlideNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.SlideNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                SlideNoteData updated = Managers.Chart.SlideNotes[index];
                if (updated.length > 1)
                {
                    updated.length -= 1;
                    Managers.Chart.SlideNotes[index] = updated;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        Vector3 scale = obj.transform.localScale;
                        scale.x = lineSpawner.lineGap * updated.length;
                        obj.transform.localScale = scale;

                        Vector3 newPos = obj.transform.position;
                        newPos.x -= (lineSpawner.lineGap / 2);

                        obj.transform.position = newPos;
                    }
                }
            }
            else if (Managers.Chart.FlickNotes.Any(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex))
            {
                int index = Managers.Chart.FlickNotes.FindIndex(note => note.position == closestHorizontalIndex && note.line == leftVerticalIndex);
                FlickNoteData updated = Managers.Chart.FlickNotes[index];
                if (updated.length > 1)
                {
                    updated.length -= 1;
                    Managers.Chart.FlickNotes[index] = updated;

                    if (Managers.Chart.Notes[leftVerticalIndex].TryGetValue(closestHorizontalIndex, out GameObject obj) && obj != null)
                    {
                        Vector3 scale = obj.transform.localScale;
                        scale.x = lineSpawner.lineGap * updated.length;
                        obj.transform.localScale = scale;

                        Vector3 newPos = obj.transform.position;
                        newPos.x -= (lineSpawner.lineGap / 2);

                        obj.transform.position = newPos;
                    }
                }
            }

            Managers.Chart.SaveChart();
        }
        else return;
    }

    public void RemoveNoteFromJson(int leftVerticalIndex, int closestHorizontalIndex)
    {
        int pos = closestHorizontalIndex;
        int line = leftVerticalIndex;

        int index;

        index = Managers.Chart.NormalNotes.FindIndex(note => note.position == pos && note.line == line);
        if (index != -1)
        {
            Managers.Chart.NormalNotes.RemoveAt(index);
            return;
        }

        index = Managers.Chart.HoldNotes.FindIndex(note => note.position == pos && note.line == line);
        if (index != -1)
        {
            Managers.Chart.HoldNotes.RemoveAt(index);
            return;
        }

        index = Managers.Chart.SlideNotes.FindIndex(note => note.position == pos && note.line == line);
        if (index != -1)
        {
            Managers.Chart.SlideNotes.RemoveAt(index);
            return;
        }

        index = Managers.Chart.FlickNotes.FindIndex(note => note.position == pos && note.line == line);
        if (index != -1)
        {
            Managers.Chart.FlickNotes.RemoveAt(index);
            return;
        }

    }
}
