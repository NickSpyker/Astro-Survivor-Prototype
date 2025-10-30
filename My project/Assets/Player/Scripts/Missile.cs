using UnityEngine;
using System.Collections;
using UnityEngine.LowLevelPhysics;

namespace Assets.Player.Scripts
{
	public class Missile: MonoBehaviour
	{
		public Vector3 position;
        public Vector3 direction;
		public float speed;

		private GameObject _gm;


        // Use this for initialization
        void Start()
        {
			_gm = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			_gm.transform.position = position;
			_gm.transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);

			_gm.AddComponent<Rigidbody>();
			_gm.GetComponent<Rigidbody>().useGravity = false;
			_gm.GetComponent<Rigidbody>().linearVelocity = direction.normalized * speed;
        }

		// Update is called once per frame
		void Update()
		{

		}
	}
}