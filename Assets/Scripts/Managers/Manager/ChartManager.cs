using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartManager
{
    public List<Dictionary<int,GameObject>> Notes;        // ���� ������ ��Ʈ ���ӿ�����Ʈ ����
    public List<NormalNoteData> NormalNotes;                   // �Ϲ� ��Ʈ ������
    public List<HoldNoteData> HoldNotes;                     // Ȧ�� ��Ʈ ������
    public List<SlideNoteData> SlideNotes;                    // �����̵� ��Ʈ ������
    public List<UpFlickNoteData> UpFlickNotes;                    // up �ø� ��Ʈ ������
    public List<DownFlickNoteData> DownFlickNotes;                    // down �ø� ��Ʈ ������


    public void Init()
    {
        Notes = new();
        NormalNotes = new();
        HoldNotes = new();
        SlideNotes = new();
        UpFlickNotes = new();
        DownFlickNotes = new();
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
        File.WriteAllText("Assets//sample_chart.json", json);
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

