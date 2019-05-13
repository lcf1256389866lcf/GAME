﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

#if DEBUG_WINDOW
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class InputGyroscopeInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("Input Gyroscope Information");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable", GUILayout.Height(DefaultWindowRect.height / 18)))
                        {
                            Input.gyro.enabled = true;
                        }
                        if (GUILayout.Button("Disable", GUILayout.Height(DefaultWindowRect.height / 18)))
                        {
                            Input.gyro.enabled = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Enabled:", Input.gyro.enabled.ToString());
                    DrawItem("Update Interval:", Input.gyro.updateInterval.ToString());
                    DrawItem("Attitude:", Input.gyro.attitude.eulerAngles.ToString());
                    DrawItem("Gravity:", Input.gyro.gravity.ToString());
                    DrawItem("Rotation Rate:", Input.gyro.rotationRate.ToString());
                    DrawItem("Rotation Rate Unbiased:", Input.gyro.rotationRateUnbiased.ToString());
                    DrawItem("User Acceleration:", Input.gyro.userAcceleration.ToString());
                }
                GUILayout.EndVertical();
            }
        }
    }
}
#endif