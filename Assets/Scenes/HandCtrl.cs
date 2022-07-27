using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCtrl : MonoBehaviour
{
    public bool m_isRay = false; //��ǰ�Ƿ����߿�������q�����ߣ��ٰ��ر�
    public bool m_lastPress = false; //��һ���Ƿ���
    public Vector3 m_rayEndPos = Vector3.zero; //����ĩ��λ��
    Vector3 m_lastRayEndPos = Vector3.zero; //��һ������ĩ��λ�ã�Ϊ���жϹ��������¼�
    private LineRenderer _stylusBeamRenderer = null;
    private float _stylusBeamLength = 10;//���߳��ȣ�Ϊ��ײ���ĵ㵽����ͷ����
    float m_disClick = 0.009f;//���ε����λ��
    Transform m_trans;


    //��ײ����UI����������״̬��ʾ��
    public GameObject m_curHitObj;
    public GameObject m_lastHitObj;

    //��ײ���Ĺ�����
    public GameObject m_curHitScroll;
    public float m_scrollSensitive = 20.0f;

    float m_moveSpeed = 5.0f;


    //������ӦUI���������
    public GameObject m_lastHitUI;
    public Vector3 m_lastHitUIPos;
    // Start is called before the first frame update
    void Start()
    {
        m_trans = this.transform;
        LineInit();
    }

    // Update is called once per frame
    void Update()
    {
        SelfMoveCtrl();
        RayInputKeyCtrl();
        bool isPress = false;

        if (Input.GetKey(KeyCode.E)) //��ס�Ǹ������Ĺ���
        {
            isPress = true;
        }

        if (m_isRay == true)
        {
            RaycastHit hit;
            //���������ί��Ҫȥ��ui����ק���ƶ�
            if (Physics.Raycast(m_trans.position, m_trans.forward, out hit, 1000.0f, 1 << LayerMask.NameToLayer("BoxUI")))
            {
                _stylusBeamLength = hit.distance;
                m_curHitObj = hit.transform.gameObject;
                m_rayEndPos = hit.point;
                if (isPress == true && m_lastPress == false)
                {
                    m_lastHitUI = hit.transform.gameObject;
                    m_lastHitUIPos = hit.point;
                }
                else if (isPress == false && m_lastPress == true) //���ֲ���Ӧ���
                {
                    if (m_lastHitUI == hit.transform.gameObject)
                    {
                        if (Vector3.Distance(m_rayEndPos, m_lastHitUIPos) < m_disClick)
                        {
                            UseEventDelegate(hit);
                        }
                    }
                }
            }
            else
            {
                m_curHitObj = null;
                m_lastHitUI = null;
            }

            //�����㴦��
            if (isPress && !m_lastPress && m_curHitScroll == null)
            {
                Debug.Log("���Ե��������");
                RaycastHit hitScroll;
                if (Physics.Raycast(m_trans.position, m_trans.forward, out hitScroll, 1000.0f, (1 << LayerMask.NameToLayer("BoxScroll"))))
                {
                    Debug.Log("�㵽�˹�����");
                    m_curHitScroll = hitScroll.transform.gameObject;
                }
                else {
                    m_curHitScroll = null;
                }
            }
            else if (isPress && m_curHitScroll != null)
            {
                RaycastHit hitScroll;
                if (Physics.Raycast(m_trans.position, m_trans.forward, out hitScroll, 1000.0f, (1 << LayerMask.NameToLayer("BoxScroll"))))
                {
                    if (m_curHitScroll == hitScroll.transform.gameObject)
                    {
                        m_rayEndPos = hitScroll.point;
                        Vector3 dis = m_rayEndPos - m_lastRayEndPos;
                        if (dis.sqrMagnitude > 0)
                        {
                            Debug.Log("�������ƶ�");
                            UpdateContentPos(m_curHitScroll.transform, dis);
                        }
                    }
                }
                else {
                    m_curHitScroll = null;
                }
            }


            UpdateStylusBeam(m_trans.position, m_trans.forward);
        }
        else
        {
            _stylusBeamRenderer.enabled = false;
        }

        if (m_curHitObj != m_lastHitObj)
        {
            if (m_lastHitObj != null)
            {
                UseHoverExitDelegate(m_lastHitObj);
            }

            if (m_curHitObj != null)
            {
                UseHoverEnterDelegate(m_curHitObj);
            }
        }


        m_lastPress = isPress;
        m_lastHitObj = m_curHitObj;
        m_lastRayEndPos = m_rayEndPos;
    }

    void RayInputKeyCtrl()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_isRay = !m_isRay;
        }
    }


    void SelfMoveCtrl()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 newPos = new Vector3(h, v, 0);
        m_trans.position = m_trans.position + newPos * Time.deltaTime * m_moveSpeed;
    }
    /// <summary>
    /// ���߳�ʼ��
    /// </summary>
    void LineInit()
    {
        _stylusBeamRenderer = gameObject.AddComponent<LineRenderer>();
        _stylusBeamRenderer.material = new Material(Shader.Find("Standard"));
        _stylusBeamRenderer.startWidth = 0.05f;
        _stylusBeamRenderer.endWidth = 0.05f;
        _stylusBeamRenderer.SetColors(Color.blue, Color.blue);

    }


    /// <summary>
    /// ������ʾ������
    /// </summary>
    /// <param name="stylusPosition"></param>
    /// <param name="stylusDirection"></param>
    /// <param name="qua"></param>
    private void UpdateStylusBeam(Vector3 stylusPosition, Vector3 stylusDirection)
    {
        if (_stylusBeamRenderer != null && m_isRay == true)
        {
            _stylusBeamRenderer.enabled = true;


            float stylusBeamWidth = 10;
            float stylusBeamLength = _stylusBeamLength;

            _stylusBeamRenderer.SetPosition(0, stylusPosition);
            _stylusBeamRenderer.SetPosition(1, stylusPosition + (stylusDirection * stylusBeamLength));
        }
    }


    void UseEventDelegate(RaycastHit hitInfo)
    {
        if (hitInfo.transform != null)
        {
            Debug.Log("pen click:" + hitInfo.transform.name);
            if (KEventDelegate.IsHaveClick(hitInfo.transform.gameObject) == true)
            {
                KEventDelegate.Get(hitInfo.transform.gameObject).OnClick(hitInfo.transform.gameObject);
            }
        }
    }

   
    void UseHoverDelegate(RaycastHit hitInfo)
    {
        if (hitInfo.transform != null)
        {
            //Debug.Log("pen hover:" + hitInfo.transform.name);
            if (KEventDelegate.IsHaveHoverStay(hitInfo.transform.gameObject) == true)
            {
                KEventDelegate.Get(hitInfo.transform.gameObject).OnHoverStay(hitInfo.transform.gameObject);
            }
        }
    }

    void UseHoverExitDelegate(GameObject obj)
    {
        if (obj != null)
        {
            //Debug.Log("pen hover:" + hitInfo.transform.name);
            if (KEventDelegate.IsHaveHoverExit(obj) == true)
            {
                KEventDelegate.Get(obj).OnHoverExit(obj);
            }
        }
    }

    void UseHoverEnterDelegate(GameObject obj)
    {
        if (obj != null)
        {
            //Debug.Log("pen hover:" + hitInfo.transform.name);
            if (KEventDelegate.IsHaveHoverEnter(obj) == true)
            {
                KEventDelegate.Get(obj).OnHoverEnter(obj);
            }
        }
    }

    //������һ֡��ֵ�����¹�����λ��
    void UpdateContentPos(Transform trans,Vector3 dis)
    {
        ScrollRect scroll = trans.GetComponent<ScrollRect>();
        Transform content = scroll.content.transform;
        if (scroll.horizontal == true)
        {
            content.localPosition = content.localPosition + new Vector3(dis.x * m_scrollSensitive , 0, 0);
        }

        if (scroll.vertical)
        {
            content.localPosition = content.localPosition + new Vector3(0, dis.y * m_scrollSensitive , 0);
        }

    }

}
