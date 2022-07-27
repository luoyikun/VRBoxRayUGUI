using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public GameObject m_tmp;
    public Transform m_content;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;i < 30; i++)
        {
            GameObject obj = GameObject.Instantiate(m_tmp);
            obj.transform.parent = m_content;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.name = i.ToString();
            obj.transform.Find("Text").GetComponent<Text>().text = i.ToString();
            KEventDelegate.Get(obj).onClick = OnBtnClick;
            KEventDelegate.Get(obj).onHoverEnter = OnHoverEnter;
            KEventDelegate.Get(obj).onHoverExit = OnHoverExit;
        }
    }

    void OnBtnClick(GameObject obj)
    {
        Debug.Log("��ť�󶨵�ί�У�"+obj.name);
    }

    void OnHoverEnter(GameObject obj)
    {
        obj.GetComponent<Image>().color = obj.GetComponent<Button>().colors.highlightedColor;
        Debug.Log("��ťѡ�У�" + obj.name);
    }

    void OnHoverExit(GameObject obj)
    {
        obj.GetComponent<Image>().color = obj.GetComponent<Button>().colors.normalColor;
        Debug.Log("��ťѡ���˳���" + obj.name);
    }

}
