using UnityEngine;

namespace SongsTest {

	public class TestShooter : MonoBehaviour {
	
		public AttackEffectSpawner Spawner;
		public KeyCode ShootKey = KeyCode.Space;

		private void Update() {
			if (Input.GetKeyDown(ShootKey)) {
				Spawner.Spawn();
			}
		}
	}
}
