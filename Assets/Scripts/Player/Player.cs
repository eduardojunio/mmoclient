using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameClient {
	public class Player {
		public struct Coordinates {
			public float x, y;
			public Coordinates (float x, float y) {
				this.x = x;
				this.y = y;
			}
		}

		public Coordinates coordinates;
		public string ipAddress;
		public int portNumber;

		public Player(Coordinates coordinates, string ipAddress, int portNumber) {
			this.coordinates = coordinates;
			this.ipAddress = ipAddress;
			this.portNumber = portNumber;
		}
	}
}
