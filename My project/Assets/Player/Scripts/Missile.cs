using UnityEngine;
using System.Collections;
using UnityEngine.LowLevelPhysics;

namespace Assets.Player.Scripts
{

	[RequireComponent(typeof(Rigidbody))]
	public class Missile: MonoBehaviour
	{
		public GameObject _prefab;
		private Rigidbody _rb;

        // Use this for initialization
        void Start()
        {
			_rb = this.GetComponent<Rigidbody>();
			_rb.useGravity = false;
			_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }

		// Update is called once per frame
		void Update()
		{

            _rb.linearVelocity = this.transform.rotation * new Vector3(0, 0, 15);
        }
	}
}