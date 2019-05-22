﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction action;

        public void StartAction(IAction behaviour)
        {
            if (action == behaviour) return;

            if ( null != action)
            {
                action.Cancel();
            }
            this.action = behaviour;
        }
    }
}