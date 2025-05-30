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

    public static InputManager Input { get { return Instance._input; } }

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
            // 프리팹에서 로드해서 인스턴스화
            go = Resources.Load<GameObject>("Prefabs/@Managers");
            go = Instantiate(go);
            go.name = "@Managers";

            // 씬 전환 시 파괴되지 않도록
        }
        // s_instance 세팅
        s_instance = go.GetComponent<Managers>();


        DontDestroyOnLoad(go);
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
