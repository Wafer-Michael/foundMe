using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using UnityEngine.XR;

public class Tester_PlayerInputController : MonoBehaviour
{
    PlayerInputer m_inputer;

    [SerializeField]
    GameObject m_prefab;

    [SerializeField]
    OVRHand m_ovrHand;

    [SerializeField]
    float m_rayDistance = 100.0f;

    [SerializeField]
    float m_forcePower = 10.0f;

    [SerializeField]
    LineRenderer m_lineRenderer;

    [SerializeField]
    TextMeshProUGUI m_debugText;    //DebugLender用のテクスチャ

    [SerializeField]
    GameObject m_camera;

    string m_displayCountString = "";

    struct LineRendererData {
        public Vector3 startPosition;  //開始位置
        public Vector3 endPosition;    //終了位置

        public LineRendererData(Vector3 startPosition, Vector3 endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
    }

    void Start()
    {
        //マルチディスプレイの有効化
        int maxDisplayCount = 2;
        int displayCount = 0;
        for (int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            displayCount++;
        }

        m_displayCountString = displayCount.ToString(); //ディスプレイの数を取得

        //カメラのミラーをoff
        XRSettings.gameViewRenderMode = GameViewRenderMode.None;

        //その他コンポーネント
        m_inputer = GetComponent<PlayerInputer>();
        //m_lineRenderer = GetComponent<LineRenderer>();
        //m_lineRenderer.useWorldSpace = true;     // ローカル座標
    }

    void Update()
    {
        m_lineRenderer.SetPositions(CalculateRendererPositions());
        m_lineRenderer.enabled = m_ovrHand.IsPointerPoseValid;    //有効な状態のときのみ表示

        if (m_inputer.IsPinch())
        {
            RayHit();
            //var position = m_ovrHand.PointerPose.position + m_ovrHand.PointerPose.forward * 2.0f;
            //Instantiate(m_prefab, position, Quaternion.identity);
        }
    }

    //LineRender用の開始位置と終了位置を返す
    LineRendererData CalculateRenderData()
    {
        LineRendererData data;

        var basePosition = m_ovrHand.PointerPose.position + m_camera.transform.position;
        data.startPosition = basePosition;
        data.endPosition = basePosition + m_ovrHand.PointerPose.forward * m_rayDistance;

        //Debug.Log("StartPosition:" + data.startPosition);
        m_debugText.text = m_displayCountString + "\n" + data.startPosition.ToString();

        return data;
    }

    //LineRendererの表示用にポジション配列を返す。
    Vector3[] CalculateRendererPositions()
    {
        LineRendererData rendererData = CalculateRenderData();

        var positions = new Vector3[] {
            rendererData.startPosition,
            rendererData.endPosition
        };

        return positions;
    }

    void RayHit()
    {
        //var data = CalculateRenderData();
        var ray = new Ray(m_ovrHand.PointerPose.position + m_camera.transform.position, m_ovrHand.PointerPose.forward * m_rayDistance);

        if(Physics.Raycast(ray, out var hitInfo, m_rayDistance))    
        {
            var otherObject = hitInfo.collider.gameObject;
            var rigit = otherObject.GetComponent<Rigidbody>();

            var forceDirection = otherObject.transform.position - m_ovrHand.PointerPose.position;
            rigit?.AddForce(forceDirection.normalized * m_forcePower);
        }

    }
    
}
