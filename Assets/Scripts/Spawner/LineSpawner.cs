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
    [HideInInspector]
    public float beatSpacing; // ÇÑ ¹ÚÀÚ °£°Ý
    [HideInInspector]
    private int totalBeats = 10;

    List<GameObject> lines;
    List<GameObject> verticalLines;
    List<GameObject> halfLines;
    List<GameObject> quaterLines;
    List<GameObject> eighthLines;
    List<GameObject> sixteenthLines;

    Text showHalfButtonText;
    Text showQuaterButtonText;
    Text showEighthButtonText;
    Text showSixteenthButtonText;


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

            // Viewport ÁÂÇ¥ ¡æ ¿ùµå ÁÂÇ¥·Î º¯È¯
            Vector3 viewportPos = new Vector3(normalizedX, 0.5f, cam.nearClipPlane);
            Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
            worldPos.z = 0;

            if (i % 3 == 0)
            {
                Instantiate(verticalLinePrefab, worldPos, Quaternion.identity, verticalLineRoot.transform);
            }
            else
            {
                Instantiate(hiddenVerticalLinePrefab, worldPos, Quaternion.identity, verticalLineRoot.transform);
            }
        }

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
            showHalfButtonText.text = "1/2 Ç¥½Ã";
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
            showHalfButtonText.text = "1/2 ¼û±è";
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
            showQuaterButtonText.text = "1/4 Ç¥½Ã";
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
            showQuaterButtonText.text = "1/4 ¼û±è";
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
            showEighthButtonText.text = "1/8 Ç¥½Ã";
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
            showEighthButtonText.text = "1/8 ¼û±è";
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
            showSixteenthButtonText.text = "1/16 Ç¥½Ã";
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
            showSixteenthButtonText.text = "1/16 ¼û±è";
        }
    }
    #endregion

}