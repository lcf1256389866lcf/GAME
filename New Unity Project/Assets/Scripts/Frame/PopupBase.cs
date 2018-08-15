﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : ObjBase {

    public virtual void Close()
    {
        Debug.Log("---Closed!---");
        DisableGameObject();
    }
}