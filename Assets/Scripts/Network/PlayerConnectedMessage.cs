using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameClient {
	public class PlayerConnectedMessage : Message {
		public Player player;

		public PlayerConnectedMessage(Player player, string type = "connected")
			: base(type) {
			this.player = player;
		}
	}
}
