using UnityEngine;
using System.Collections;

public enum enumCameraStep { None, Menu, Start, Standby, Playing, End, Moving, PKMenu, PKStart, PKStandby, PKPlaying, PKEnd, TrainingMenu, TrainingStart, TrainingStandby, TrainingPlaying, TrainingEnd };
public enum enumCameraType { None, Focus,Path,PlayerFocus,PlayerPath };

public class CameraProcess : MonoBehaviour
{
    //鏡頭明暗
    public Texture2D textureBG = null;
    private int FadeDir = 0;
    private float FadeTime = 0;
    private float FadeStartTime = 0;

    //鏡頭移動
    public bool IsRefresh = false;
    public enumCameraStep CameraStep = enumCameraStep.None;

    public enumCameraType CamType = enumCameraType.None;
    public float NowTime = 0; //目前移動時間
    public float TotalTime = 1; //總移動時間
    public float MidPercent = 0; //中間點的時間
    public Vector3 LookPosition = new Vector3(); //相機焦點位置
    public Vector3 MovePosition = new Vector3();//相機移動位置
    public float SmoothTime = 0; //平滑移動時間
    public Vector3 StartPosition = new Vector3(0, 0, 0);
    public Vector3 MidPosition = new Vector3(0, 0, 0);
    public Vector3 EndPosition = new Vector3(0, 0, 0);
    private CameraData[] CameraDatas = new CameraData[20];
    private GameObject Player = null;
    private Vector3 CameraNowMovePosition;

    public static void Init()
    {
        GameObject obj = GameObject.Find("GameCamera");
        if (!obj)
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("PrefabUI/GameCamera")); //player1的透明人物替身
            obj.name = "GameCamera";
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        textureBG = (Texture2D)Resources.Load("Photos/BK");
    }

    public void SetCameraStep(enumCameraStep Step)
    {
        SetCameraStep(Step, null);
    }

    public void SetCameraStep(enumCameraStep Step, GameObject player)
    {
        FadeStop();
        if (Step == enumCameraStep.Start) FadeIn(1);
        CameraStep = Step;
        Player = player;
        if (CameraStep == enumCameraStep.None) return; 
        CameraData NowCameraData = CameraDatas[(int)Step];
        CamType = NowCameraData.CamType;
        LookPosition = NowCameraData.LookPosition;
        MovePosition = NowCameraData.MovePosition;
        SmoothTime = NowCameraData.SmoothTime;
        StartPosition = NowCameraData.MoveStartPosition;
        MidPosition = NowCameraData.MoveMidPosition;
        EndPosition = NowCameraData.MoveEndPosition;
        TotalTime = NowCameraData.TotalTime;
        NowTime = 0;
        MidPercent = NowCameraData.MidPercent;
    }

    public void Update()
    {
       // Debug.Log("CameraStep=" + CameraStep);
        if (CameraStep == enumCameraStep.None) return;
        if (IsRefresh)
        {
            FadeIn(3);
            IsRefresh = false;
            SetCameraStep(CameraStep, Player);
        }
    }

    public Vector3 GetMovePostion(float t)
    {
        Vector3 StartPos=StartPosition;
        Vector3 MidPos=MidPosition;
        Vector3 EndPos = EndPosition;
        if (Player != null)
        {
            StartPos = Player.gameObject.transform.TransformPoint(StartPosition);
            MidPos = Player.gameObject.transform.TransformPoint(MidPosition);
            EndPos = Player.gameObject.transform.TransformPoint(EndPosition);
        }
        Vector3 pos;
        if (t < MidPercent)
        {
            t = t / MidPercent;
            pos = getSmoothPosition(StartPos, StartPos, MidPos, EndPos, t);
        }
        else
        {
            t = t - MidPercent;
            t = t / (1 - MidPercent);
            pos = getSmoothPosition(StartPos, MidPos, EndPos, EndPos, t);
        }
        return pos;
    }

    private Vector3 getSmoothPosition(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        float tension = 0.5f; //0.5f equivale a catmull-rom

        Vector3 T1 = tension * (P2 - P0);
        Vector3 T2 = tension * (P3 - P1);

        float Blend1 = 2 * t3 - 3 * t2 + 1;
        float Blend2 = -2 * t3 + 3 * t2;
        float Blend3 = t3 - 2 * t2 + t;
        float Blend4 = t3 - t2;

        return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
    }


    void OnGUI()
    {
        if (FadeDir == 0) return;
        GUI.depth = 2;

        float alpha = (Time.time - FadeStartTime);
        if (alpha > FadeTime) alpha = FadeTime;
        alpha = alpha / FadeTime;
        alpha = Mathf.Pow(alpha, 2);
        if (FadeDir < 0) alpha = 1 - alpha;
        alpha = Mathf.Clamp01(alpha);
        //Debug.Log("alpha=" + alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureBG);
    }

    public void FadeStop()
    {
        FadeDir = 0;
    }

    public void FadeIn(float t)
    {
        FadeDir = -1;
        FadeTime = t;
        FadeStartTime = Time.time;
    }

    public void FadeOut(float t)
    {
        FadeDir = 1;
        FadeTime = t;
        FadeStartTime = Time.time;
    }

}

public class CameraData
{
    public enumCameraType CamType = 0;
    public Vector3 LookPosition = new Vector3(); //相機焦點位置
    public Vector3 MovePosition = new Vector3();//相機移動位置
    public float SmoothTime = 0; //平滑移動時間

    public Vector3 MoveStartPosition = new Vector3(0, 0, 0);
    public Vector3 MoveMidPosition = new Vector3(0, 0, 0);
    public Vector3 MoveEndPosition = new Vector3(0, 0, 0);
    public float TotalTime = 1; //總移動時間
    public float MidPercent = 0; //中間點的比例

    public CameraData()
    {
    }

    public CameraData(enumCameraType nType, Vector3 LookPos, Vector3 MovePos, float fSmoothTime)
    {
        CamType = nType;
        LookPosition = LookPos;
        MovePosition = MovePos;
        SmoothTime = fSmoothTime;
    }

    public CameraData(enumCameraType nType, Vector3 LookPos, Vector3 StartPos, Vector3 MidPos, Vector3 EndPos, float fTotalTime, float fMidPercent)
    {
        CamType = nType;
        LookPosition = LookPos;
        MoveStartPosition = StartPos;
        MoveMidPosition = MidPos;
        MoveEndPosition = EndPos;
        TotalTime = fTotalTime;
        MidPercent = fMidPercent;
    }

    public void Copy(CameraData cam)
    {
        CamType = cam.CamType;
        LookPosition = cam.LookPosition;
        MoveStartPosition = cam.MoveStartPosition;
        MoveMidPosition = cam.MoveMidPosition;
        MoveEndPosition = cam.MoveEndPosition;
        TotalTime = cam.TotalTime;
        MovePosition = cam.MovePosition;
        SmoothTime = cam.SmoothTime;
        MidPercent = cam.MidPercent;
    }
}