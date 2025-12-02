using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Linq;

public class ChartManager
{
    public List<Dictionary<int, GameObject>> Notes;        // 실제 생성된 노트 게임오브젝트 참조
    public List<NormalNoteData> NormalNotes;                   // 일반 노트 데이터
    public List<HoldNoteData> HoldNotes;                     // 홀드 노트 데이터
    public List<SlideNoteData> SlideNotes;                    // 슬라이드 노트 데이터
    public List<FlickNoteData> FlickNotes;                    // 플릭 노트 데이터

    public Dictionary<(int, int), GameObject> LinkedLines;

    public bool isLoaded;

    public void Init()
    {
        NormalNotes = new();
        HoldNotes = new();
        SlideNotes = new();
        FlickNotes = new();
        Notes = new();
        for (int i = 0; i < 22; i++)
        {
            Notes.Add(new Dictionary<int, GameObject>());
        }

        isLoaded = false;
        LinkedLines = new();
    }

    public void SaveChart()
    {
        #region Sort
        List<NormalNoteData> sortedNormalNotes = NormalNotes
        .OrderBy(note => note.position)
        .ThenBy(note => note.line) 
        .ToList();

        List<HoldNoteData> sortedHoldNotes = HoldNotes
        .OrderBy(note => note.position)
        .ThenBy(note => note.line)
        .ToList();

        int cnt = 0;
        for (int i = 0; i < sortedHoldNotes.Count; i++)
        {
            HoldNoteData holdnotedata = sortedHoldNotes[i];
            if (holdnotedata.noteType == 0)
            {
                holdnotedata.count = cnt; // count 값 재설정
                cnt++;
                sortedHoldNotes[i] = holdnotedata; // struct 업데이트 반영
            }
        }

        List<SlideNoteData> sortedSlideNotes = SlideNotes
        .OrderBy(note => note.position)
        .ThenBy(note => note.line)
        .ToList();

        List<FlickNoteData> sortedFlickNotes = FlickNotes
        .OrderBy(note => note.position)
        .ThenBy(note => note.line)
        .ToList();
        #endregion

        ChartData data = new ChartData
        {
            NormalNotes = sortedNormalNotes,
            HoldNotes = sortedHoldNotes,
            SlideNotes = sortedSlideNotes,
            FlickNotes = sortedFlickNotes,
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("Assets//daki-월광소녀.json", json);

        LoadChart();
    }


    public void LoadChart()
    {
        isLoaded = true;

        LineSpawner lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();

        string path = "Assets//daki-월광소녀.json";
        if (!File.Exists(path))
        {
            Debug.LogError("Chart file not found: " + path);
            // 디렉터리 없으면 생성
            File.WriteAllText(path, "{}");
        }

        string json = File.ReadAllText(path);
        ChartData data = JsonUtility.FromJson<ChartData>(json);

        if (data == null)
        {
            Debug.LogError("Failed to parse chart JSON.");
            return;
        }

        foreach (var verticalDict in Notes)
        {
            foreach (var obj in verticalDict.Values)
            {
                if (obj != null)
                    GameObject.Destroy(obj);
            }

            verticalDict.Clear(); // 딕셔너리 내부도 비움
        }


        // 기존 데이터 초기화
        Managers.Chart.NormalNotes.Clear();
        Managers.Chart.SlideNotes.Clear();
        Managers.Chart.HoldNotes.Clear();
        Managers.Chart.FlickNotes.Clear();
        Managers.Chart.Notes.Clear();
        for (int i = 0; i < 22; i++)
        {
            Notes.Add(new Dictionary<int, GameObject>());
        }

        // 불러온 데이터 반영

        #region RenderNotes
        foreach (var note in data.NormalNotes)
        {
            Managers.Chart.NormalNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap * note.length;
            noteGO.transform.localScale = scale;

            Vector3 newPos = noteGO.transform.position;
            newPos.x += (lineSpawner.lineGap / 2) * (note.length - 1);

            noteGO.transform.position = newPos;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }

        foreach (var note in data.HoldNotes)
        {
            Managers.Chart.HoldNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab;

            if (note.noteType == 0)
                notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteStart");
            else if(note.noteType == 1)
                notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteMid");
            else
                notePrefab = Resources.Load<GameObject>("Prefabs/Notes/HoldNoteEnd");

            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap * note.length;
            noteGO.transform.localScale = scale;

            Vector3 newPos = noteGO.transform.position;
            newPos.x += (lineSpawner.lineGap / 2) * (note.length - 1);

            noteGO.transform.position = newPos;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }

        foreach (var note in data.SlideNotes)
        {
            Managers.Chart.SlideNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/SlideNote");
            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap * note.length;
            noteGO.transform.localScale = scale;

            Vector3 newPos = noteGO.transform.position;
            newPos.x += (lineSpawner.lineGap / 2) * (note.length - 1);

            noteGO.transform.position = newPos;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }

        foreach (var note in data.FlickNotes)
        {
            Managers.Chart.FlickNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab;

            if (note.direction == 0)
                notePrefab = Resources.Load<GameObject>("Prefabs/Notes/UpFlickNote");
            else
                notePrefab = Resources.Load<GameObject>("Prefabs/Notes/DownFlickNote");

            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap * note.length;
            noteGO.transform.localScale = scale;

            Vector3 newPos = noteGO.transform.position;
            newPos.x += (lineSpawner.lineGap / 2) * (note.length - 1);

            noteGO.transform.position = newPos;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }
        #endregion

        
    }

    public void RenderLinkedLines()
    {
        LineSpawner lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();
        #region RenderLinkedLines
        foreach (var item in HoldNotes)
        {
            if (item.noteType == 0)
            {
                var target1 = HoldNotes.FirstOrDefault(h => h.noteType == 1 && h.count == item.count);
                bool found1 = HoldNotes.Any(h => h.noteType == 1 && h.count == item.count);

                if (found1)
                {
                    Vector3 linkedLineStartPosition = new Vector3(
                    lineSpawner.verticalLines[item.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[item.position].transform.position.y, 0f);

                    Vector3 linkedLineEndPosition = new Vector3(
                    lineSpawner.verticalLines[target1.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[target1.position].transform.position.y, 0f);

                    GameObject lineObj = new GameObject("LinkLine");
                    LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, linkedLineStartPosition);
                    lineRenderer.SetPosition(1, linkedLineEndPosition);

                    lineRenderer.widthMultiplier = 0.1f;

                    lineRenderer.startColor = Color.gray;
                    lineRenderer.endColor = Color.gray;

                    if(!Managers.Chart.LinkedLines.ContainsKey((item.line, item.position)))
                    {
                        Managers.Chart.LinkedLines.Add((item.line, item.position), lineObj);
                    }
                }


                var target2 = HoldNotes.FirstOrDefault(h => h.noteType == 2 && h.count == item.count);
                bool found2 = HoldNotes.Any(h => h.noteType == 2 && h.count == item.count);
                if (found2)
                {
                    Vector3 linkedLineStartPosition = new Vector3(
                    lineSpawner.verticalLines[item.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[item.position].transform.position.y, 0f);

                    Vector3 linkedLineEndPosition = new Vector3(
                    lineSpawner.verticalLines[target2.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[target2.position].transform.position.y, 0f);

                    GameObject lineObj = new GameObject("LinkLine");
                    LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, linkedLineStartPosition);
                    lineRenderer.SetPosition(1, linkedLineEndPosition);

                    lineRenderer.widthMultiplier = 0.1f;

                    lineRenderer.startColor = Color.gray;
                    lineRenderer.endColor = Color.gray;

                    if (!Managers.Chart.LinkedLines.ContainsKey((item.line, item.position)))
                    {
                        Managers.Chart.LinkedLines.Add((item.line, item.position), lineObj);
                    }
                }
            }
            else if (item.noteType == 1)
            {
                var target2 = HoldNotes.FirstOrDefault(h => h.noteType == 1 && h.count == item.count);
                bool found2 = HoldNotes.Any(h => h.noteType == 2 && h.count == item.count);
                if (found2)
                {
                    Vector3 linkedLineStartPosition = new Vector3(
                    lineSpawner.verticalLines[item.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[item.position].transform.position.y, 0f);

                    Vector3 linkedLineEndPosition = new Vector3(
                    lineSpawner.verticalLines[target2.line].transform.position.x + (lineSpawner.lineGap / 2),
                    lineSpawner.lines[target2.position].transform.position.y, 0f);

                    GameObject lineObj = new GameObject("LinkLine");
                    LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, linkedLineStartPosition);
                    lineRenderer.SetPosition(1, linkedLineEndPosition);

                    lineRenderer.widthMultiplier = 0.1f;

                    lineRenderer.startColor = Color.gray;
                    lineRenderer.endColor = Color.gray;

                    if (!Managers.Chart.LinkedLines.ContainsKey((item.line, item.position)))
                    {
                        Managers.Chart.LinkedLines.Add((item.line, item.position), lineObj);
                    }
                }
            }
        }
        #endregion
    }
}

[System.Serializable]
public class ChartData
{
    public List<NormalNoteData> NormalNotes;
    public List<HoldNoteData> HoldNotes;
    public List<SlideNoteData> SlideNotes;
    public List<FlickNoteData> FlickNotes;
}

