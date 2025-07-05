using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class ChartManager
{
    public List<Dictionary<int, GameObject>> Notes;        // ���� ������ ��Ʈ ���ӿ�����Ʈ ����
    public List<NormalNoteData> NormalNotes;                   // �Ϲ� ��Ʈ ������
    public List<HoldNoteData> HoldNotes;                     // Ȧ�� ��Ʈ ������
    public List<SlideNoteData> SlideNotes;                    // �����̵� ��Ʈ ������
    public List<FlickNoteData> FlickNotes;                    // �ø� ��Ʈ ������

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
    }

    public void SaveChart()
    {
        ChartData data = new ChartData
        {
            NormalNotes = this.NormalNotes,
            HoldNotes = this.HoldNotes,
            SlideNotes = this.SlideNotes,
            FlickNotes = this.FlickNotes,
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("Assets//MyChart.json", json);
    }

    public void LoadChart()
    {
        isLoaded = true;

        LineSpawner lineSpawner = GameObject.Find("LineSpawner").GetComponent<LineSpawner>();

        string path = "Assets//MyChart.json";
        if (!File.Exists(path))
        {
            Debug.LogError("Chart file not found: " + path);
            return;
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

            verticalDict.Clear(); // ��ųʸ� ���ε� ���
        }


        // ���� ������ �ʱ�ȭ
        Managers.Chart.NormalNotes.Clear();
        Managers.Chart.SlideNotes.Clear();
        Managers.Chart.HoldNotes.Clear();
        Managers.Chart.FlickNotes.Clear();
        Managers.Chart.Notes.Clear();
        for (int i = 0; i < 22; i++)
        {
            Notes.Add(new Dictionary<int, GameObject>());
        }

        // �ҷ��� ������ �ݿ�

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
}

[System.Serializable]
public class ChartData
{
    public List<NormalNoteData> NormalNotes;
    public List<HoldNoteData> HoldNotes;
    public List<SlideNoteData> SlideNotes;
    public List<FlickNoteData> FlickNotes;
}

