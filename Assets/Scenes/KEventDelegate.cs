using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KEventDelegate : MonoBehaviour, IPointerClickHandler
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;

    public VoidDelegate onHoverStay;
    public VoidDelegate onHoverExit;
    public VoidDelegate onHoverEnter;

    static public bool IsHaveClick(GameObject go)
    {
        bool isHave = false;
        if (go.GetComponent<KEventDelegate>() != null && go.GetComponent<KEventDelegate>().onClick != null)
        {
            isHave = true;
        }
        return isHave;
    }

    static public bool IsHaveHoverEnter(GameObject go)
    {
        bool isHave = false;
        if (go.GetComponent<KEventDelegate>() != null && go.GetComponent<KEventDelegate>().onHoverEnter != null)
        {
            isHave = true;
        }
        return isHave;
    }

    static public bool IsHaveHoverExit(GameObject go)
    {
        bool isHave = false;
        if (go.GetComponent<KEventDelegate>() != null && go.GetComponent<KEventDelegate>().onHoverExit != null)
        {
            isHave = true;
        }
        return isHave;
    }

    static public bool IsHaveHoverStay(GameObject go)
    {
        bool isHave = false;
        if (go.GetComponent<KEventDelegate>() != null && go.GetComponent<KEventDelegate>().onHoverStay != null)
        {
            isHave = true;
        }
        return isHave;
    }

    static public KEventDelegate Get(GameObject go)
    {
        KEventDelegate listener = go.GetComponent<KEventDelegate>();
        if (listener == null) listener = go.AddComponent<KEventDelegate>();

        {
            //ui
            if (go.GetComponent<BoxCollider>() == null && go.GetComponent<RectTransform>() != null)
            {
                BoxCollider box = go.AddComponent<BoxCollider>();
                RectTransform rect = go.GetComponent<RectTransform>();
                if (rect.sizeDelta.x != 0 && rect.sizeDelta.y != 0)
                {
                    box.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1);
                }
                else
                {
                    GridLayoutGroup grid = go.transform.parent.GetComponent<GridLayoutGroup>();
                    if (grid != null)
                    {
                        box.size = new Vector3(grid.cellSize.x, grid.cellSize.y, 1);
                    }
                }

                LayoutElement layout = go.transform.GetComponent<LayoutElement>();
                if (layout != null)
                {
                    box.size = new Vector3(layout.minWidth, layout.minHeight, 1);
                }
            }

            //模型
            //if (go.GetComponent<MeshCollider>() == null && go.GetComponent<MeshRenderer>() != null)
            //{
            //    go.AddComponent<MeshCollider>();
            //}
        }

        return listener;
    }

    public void OnClick(GameObject go)
    {
        if (onClick != null)
            onClick(go);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(gameObject);
        }
    }

    public void OnHoverStay(GameObject go)
    {
        if (onHoverStay != null)
            onHoverStay(go);
    }

    public void OnHoverExit(GameObject obj)
    {
        if (onHoverExit != null)
        {
            onHoverExit(obj);
        }
    }

    public void OnHoverEnter(GameObject obj)
    {
        if (onHoverEnter != null)
        {
            onHoverEnter(obj);
        }
    }
}
