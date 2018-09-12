﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : AutoStaticInstance<UIManager> {

    private List<PopupBase> m_AllWindowLists = new List<PopupBase>();
    private Dictionary<string, GameObject> m_AllWindowPrefabs = new Dictionary<string, GameObject>();

    private Transform mtransform;
    private Transform parent {
        get {
            if (mtransform == null)
            {
                mtransform = GameObject.FindGameObjectWithTag("Main_Canvas").transform;
            }
            return mtransform;
        }
    }

    public T OpenWindow<T>(Action<T> callBack = null) where T: PopupBase
    {
        Type type = typeof(T);
        string windowName = type.Name;
        PopupBase window = GetWindow(windowName);
        if (window == null)
        {
            GameObject prefab = FetchWindowPrefab(windowName);
            GameObject result = Instantiate(prefab, parent);
            result.transform.localPosition = Vector3.zero;
            result.transform.localScale = Vector3.one;
            window = result.GetComponent(type) as PopupBase;
            window.EnableGameObject();
            window.ObjectName = windowName;
            m_AllWindowLists.Add(window);
        }
        else
        {
            window.EnableGameObject();
        }
        return window as T;
    }

    /// <summary>
    /// 加载窗口预制
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private GameObject FetchWindowPrefab(string name)
    {
        GameObject obj = null;
        if (!m_AllWindowPrefabs.ContainsKey(name))
        {
            LoggerM.LogError(name);
            obj = Resources.Load<GameObject>(StaticData.POPUP_PATH + name);
            if (obj != null)
            {
                m_AllWindowPrefabs.Add(name, obj);
            }
        }  
        return obj;
    }

    private PopupBase GetWindow(string name)
    {
        return m_AllWindowLists.Find(window=>window.ObjectName == name);
    }
}
