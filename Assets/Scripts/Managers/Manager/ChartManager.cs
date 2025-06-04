using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartManager
{
    public List<Dictionary<int,GameObject>> Notes;        // 실제 생성된 노트 게임오브젝트 참조
    public List<NormalNoteData> NormalNotes;                   // 일반 노트 데이터
    public List<HoldNoteData> HoldNotes;                     // 홀드 노트 데이터
    public List<SlideNoteData> SlideNotes;                    // 슬라이드 노트 데이터
    public List<UpFlickNoteData> UpFlickNotes;                    // up 플릭 노트 데이터
    public List<DownFlickNoteData> DownFlickNotes;                    // down 플릭 노트 데이터


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

