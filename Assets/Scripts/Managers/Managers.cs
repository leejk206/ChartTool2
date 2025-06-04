using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{

    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    InputManager _input = new InputManager();
    ChartManager _chart = new ChartManager();

    public static InputManager Input { get { return Instance._input; } }
    public static ChartManager Chart { get { return Instance._chart; } }

    void Start()
    {
        Init();   
    }

    static void Init()
    {
        if (s_instance != null)
            return;

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            // �����տ��� �ε��ؼ� �ν��Ͻ�ȭ
            go = Resources.Load<GameObject>("Prefabs/@Managers");
            go = Instantiate(go);
            go.name = "@Managers";

            // �� ��ȯ �� �ı����� �ʵ���
        }
        // s_instance ����
        s_instance = go.GetComponent<Managers>();

        s_instance._chart.Init();

        DontDestroyOnLoad(go);
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
