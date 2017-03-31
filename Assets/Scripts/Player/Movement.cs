using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameClient {
	public class Movement : MonoBehaviour {
		[Range(1, 100)]
		public int speed;

		public delegate void MoveAction (PlayerPositionMessage playerPosition);
		public static event MoveAction OnMovement;

		private PlayerPositionMessage playerPosition = new PlayerPositionMessage ();

		void Start () {
			speed = 10;
		}

		void Update () {
			if (Input.GetButton ("Horizontal")) {
				transform.Translate (new Vector3 (speed * Input.GetAxis ("Horizontal") * Time.deltaTime, 0, 0));
			}
			if (Input.GetButton ("Vertical")) {
				transform.Translate (new Vector3 (0, speed * Input.GetAxis ("Vertical") * Time.deltaTime, 0));
			}
			UpdatePlayerPosition ();
		}

		void UpdatePlayerPosition () {
			if (!(playerPosition.x == transform.position.x &&
				playerPosition.y == transform.position.y)) {

				playerPosition.x = transform.position.x;
				playerPosition.y = transform.position.y;
				if (OnMovement != null) {
					OnMovement (playerPosition);
				}
			}
		}
	}
}
