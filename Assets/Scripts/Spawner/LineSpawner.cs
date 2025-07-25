using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSpawner : MonoBehaviour
{
    public GameObject verticalLinePrefab;
    public GameObject hiddenVerticalLinePrefab;
    public GameObject verticalLineRoot;

    public GameObject wholeLinePrefab;
    public GameObject wholeLineRoot;
    public GameObject halfLinePrefab;
    public GameObject halfLineRoot;
    public GameObject quaterLinePrefab;
    public GameObject quaterLineRoot;
    public GameObject eighthLinePrefab;
    public GameObject eighthLineRoot;
    public GameObject sixteenthLinePrefab;
    public GameObject sixteenthLineRoot;
    public float beatSpacing; // �� ���� ����
    [HideInInspector]
    private int totalBeats = 600;

    public List<GameObject> lines;
    public List<GameObject> verticalLines;
    List<GameObject> halfLines;
    List<GameObject> quaterLines;
    List<GameObject> eighthLines;
    List<GameObject> sixteenthLines;

    Text showHalfButtonText;
    Text showQuaterButtonText;
    Text showEighthButtonText;
    Text showSixteenthButtonText;

    public float lineGap;


    void Start()
    {
        verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/VerticalLine");
        hiddenVerticalLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/HiddenVerticalLine");
        verticalLineRoot = GameObject.Find("VerticalLines");

        wholeLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/WholeLine");
        wholeLineRoot = GameObject.Find("WholeLines");
        halfLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/HalfLine");
        halfLineRoot = GameObject.Find("HalfLines");
        quaterLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/QuaterLine");
        quaterLineRoot = GameObject.Find("QuaterLines");
        eighthLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/EighthLine");
        eighthLineRoot = GameObject.Find("EighthLines");
        sixteenthLinePrefab = Resources.Load<GameObject>("Prefabs/Lines/SixteenthLine");
        sixteenthLineRoot = GameObject.Find("SixteenthLines");

        lines = new();
        verticalLines = new();
        halfLines = new();
        quaterLines = new();
        eighthLines = new();
        sixteenthLines = new();

        SpawnLine();

        showHalfButtonText = GameObject.Find("ShowHalfButton").GetComponent<Text>();
        showQuaterButtonText = GameObject.Find("ShowQuaterButton").GetComponent<Text>();
        showEighthButtonText = GameObject.Find("ShowEighthButton").GetComponent<Text>();
        showSixteenthButtonText = GameObject.Find("ShowSixteenthButton").GetComponent<Text>();

    }

    public void SpawnLine()
    {
        Camera cam = Camera.main;

        for (int i = 0; i < 22; i++)
        {
            float normalizedX = (float)i / (22 - 1);

            // Viewport ��ǥ �� ���� ��ǥ�� ��ȯ
            Vector3 viewportPos = new Vector3(normalizedX, 0.5f, cam.nearClipPlane);
            Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
            worldPos.z = 0;
            GameObject line;

            if (i % 3 == 0)
            {
                line = Instantiate(verticalLinePrefab, worldPos, Quaternion.identity, verticalLineRoot.transform);
            }
            else
            {
                line = Instantiate(hiddenVerticalLinePrefab, worldPos, Quaternion.identity, verticalLineRoot.transform);
            }
            verticalLines.Add(line);
        }
        lineGap = verticalLines[1].transform.position.x - verticalLines[0].transform.position.x;

        #region Whole
        beatSpacing = 16f;
        for (int i = 0; i < totalBeats; i++)
        {
            Vector3 pos = new Vector3(0, i * beatSpacing, 0);
            GameObject line = Instantiate(wholeLinePrefab, pos, Quaternion.identity, wholeLineRoot.transform);
            lines.Add(line);
        }
        #endregion

        #region Half
        for (int i = 0; i < totalBeats * 2; i++)
        {
            if (i % 2 != 0)
            {
                Vector3 pos = new Vector3(0, i * (beatSpacing) / 2, 0);
                GameObject line = Instantiate(halfLinePrefab, pos, Quaternion.identity, halfLineRoot.transform);
                lines.Add(line);
                halfLines.Add(line);
            }
        }
        #endregion

        #region Quater
        for (int i = 0; i < totalBeats * 4; i++)
        {
            if (i % 2 != 0)
            {
                Vector3 pos = new Vector3(0, i * (beatSpacing) / 4, 0);
                GameObject line = Instantiate(quaterLinePrefab, pos, Quaternion.identity, quaterLineRoot.transform);
                lines.Add(line);
                quaterLines.Add(line);
            }
        }
        #endregion

        #region Eighth
        for (int i = 0; i < totalBeats * 8; i++)
        {
            if (i % 2 != 0)
            {
                Vector3 pos = new Vector3(0, i * (beatSpacing) / 8, 0);
                GameObject line = Instantiate(eighthLinePrefab, pos, Quaternion.identity, eighthLineRoot.transform);
                lines.Add(line);
                eighthLines.Add(line);
            }
        }
        #endregion

        #region Sixteenth
        for (int i = 0; i < totalBeats * 16; i++)
        {
            if (i % 2 != 0)
            {
                Vector3 pos = new Vector3(0, i * (beatSpacing) / 16, 0);
                GameObject line = Instantiate(sixteenthLinePrefab, pos, Quaternion.identity, sixteenthLineRoot.transform);
                lines.Add(line);
                sixteenthLines.Add(line);
            }
        }
        #endregion

        lines.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
    }

    #region ShowLine
    bool isShowHalf = true;
    public void ShowHalf()
    {
        if (isShowHalf)
        {
            isShowHalf = false;
            foreach (var line in halfLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
            showHalfButtonText.text = "1/2 ǥ��";
        }
        else
        {
            isShowHalf = true;
            foreach (var line in halfLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
            showHalfButtonText.text = "1/2 ����";
        }
    }

    bool isShowQuater = true;
    public void ShowQuater()
    {
        if (isShowQuater)
        {
            isShowQuater = false;
            foreach (var line in quaterLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
            showQuaterButtonText.text = "1/4 ǥ��";
        }
        else
        {
            isShowQuater = true;
            foreach (var line in quaterLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
            showQuaterButtonText.text = "1/4 ����";
        }
    }

    bool isShowEIghth = true;
    public void ShowEighth()
    {

        if (isShowEIghth)
        {
            isShowEIghth = false;
            foreach (var line in eighthLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
            showEighthButtonText.text = "1/8 ǥ��";
        }
        else
        {
            isShowEIghth = true;
            foreach (var line in eighthLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
            showEighthButtonText.text = "1/8 ����";
        }
    }

    bool isShowSixteenth = true;
    public void ShowSixteenth()
    {

        if (isShowSixteenth)
        {
            isShowSixteenth = false;
            foreach (var line in sixteenthLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
            showSixteenthButtonText.text = "1/16 ǥ��";
        }
        else
        {
            isShowSixteenth = true;
            foreach (var line in sixteenthLines)
            {
                SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
            showSixteenthButtonText.text = "1/16 ����";
        }
    }
    #endregion

    public void LoadChart()
    {
        Managers.Chart.LoadChart();
    }

}