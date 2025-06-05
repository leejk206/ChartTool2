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
    public List<UpFlickNoteData> UpFlickNotes;                    // up �ø� ��Ʈ ������
    public List<DownFlickNoteData> DownFlickNotes;                    // down �ø� ��Ʈ ������

    public bool isLoaded;


    public void Init()
    {
        NormalNotes = new();
        HoldNotes = new();
        SlideNotes = new();
        UpFlickNotes = new();
        DownFlickNotes = new();
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
            UpFlickNotes = this.UpFlickNotes,
            DownFlickNotes = this.DownFlickNotes
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
        Managers.Chart.DownFlickNotes.Clear();
        Managers.Chart.UpFlickNotes.Clear();
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
            scale.x = lineSpawner.lineGap;
            noteGO.transform.localScale = scale;

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
            scale.x = lineSpawner.lineGap;
            noteGO.transform.localScale = scale;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }

        foreach (var note in data.UpFlickNotes)
        {
            Managers.Chart.UpFlickNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/UpFlickNote");
            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap;
            noteGO.transform.localScale = scale;

            Managers.Chart.Notes[note.line].Add(note.position, noteGO);
        }

        foreach (var note in data.DownFlickNotes)
        {
            Managers.Chart.DownFlickNotes.Add(note);

            Vector3 notePos = new Vector3(
                lineSpawner.verticalLines[note.line].transform.position.x + (lineSpawner.lineGap / 2),
                lineSpawner.lines[note.position].transform.position.y, 0f);
            GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/DownFlickNote");
            GameObject noteGO = GameObject.Instantiate(notePrefab);
            noteGO.transform.position = notePos;

            Vector3 scale = noteGO.transform.localScale;
            scale.x = lineSpawner.lineGap;
            noteGO.transform.localScale = scale;

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
    public List<UpFlickNoteData> UpFlickNotes;
    public List<DownFlickNoteData> DownFlickNotes;
}

