using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Player.Scripts
{
    internal class ObjectFollower : MonoBehaviour
    {
        public UnityEngine.Object obj;
        public Vector3 offset;
        
        private Transform objTransform;

        public void Start()
        {
            if (obj)
            {
                objTransform = obj.GetComponent<Transform>();
            }
        }

        public void Update()
        {
            transform.position = objTransform.position + offset;
        }
    }
}
