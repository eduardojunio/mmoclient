using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameClient {
	public class PlayerPositionMessage : Message {
		public float x, y;

		public PlayerPositionMessage (float x = 0, float y = 0, string type = "update")
			: base(type) {
			this.x = x;
			this.y = y;
		}
	}
}
