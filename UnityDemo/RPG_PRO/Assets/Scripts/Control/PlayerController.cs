﻿using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        private Mover m_Mover;
        private Fighter m_Fighter;

        void Start () {
            m_Mover = this.GetComponent<Mover> ();
            m_Fighter = this.GetComponent<Fighter> ();
        }

        void Update () {

            if (InteractWithCombat ()) return; //战斗要在移动前面，不然就会一直移动到指定位置，而不会去战斗
            if (InteractWithMovement ()) return;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <returns></returns>
        private bool InteractWithMovement () {
            if (Input.GetMouseButton (0)) {
                MoveToCorsur ();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 战斗
        /// </summary>
        /// <returns></returns>
        private bool InteractWithCombat () {
            RaycastHit[] hits = Physics.RaycastAll (GetMouseRay ());
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget> ();
                if (null == target) continue; //身上有CombatTarget的对象才能开启战斗

                if (Input.GetMouseButtonDown (0)) {
                    m_Fighter.Attack (target.gameObject);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据鼠标点击的位置移动
        /// </summary>
        private void MoveToCorsur () {
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast (GetMouseRay (), out hitInfo);

            if (hasHit) {
                m_Mover.StartMoveAction (hitInfo.point);
            }
        }

        private Ray GetMouseRay () {
            return Camera.main.ScreenPointToRay (Input.mousePosition);
        }

    }

}