using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ArcadeGame.Camera
{
    [ExecuteInEditMode] // Allows the script to run in the editor
    public class DollyDriver : MonoBehaviour
    {
        private CinemachineDollyCart dollyCart;
        private Coroutine dollyCoroutine;

        private void Awake()
        {
            dollyCart = GetComponent<CinemachineDollyCart>();
        }

        public void StartDolly()
        {
            dollyCoroutine = StartCoroutine(StartDollyCoroutine());
        }
        
        private IEnumerator StartDollyCoroutine()
        {
            while (true)
            {
                dollyCart.m_Position += 1f;
                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }
    }
}
