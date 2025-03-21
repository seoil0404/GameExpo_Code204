using UnityEngine;

public class ShieldTest : MonoBehaviour {
	
	public KeyCode Active;
	public KeyCode Inactive;

	private void Update() {
		
		if (Input.GetKeyDown(Active)) {
			EffectManager.Instance.SpawnShield(gameObject);
		}
		if (Input.GetKeyDown(Inactive)) {
			EffectManager.Instance.RemoveShield(gameObject);
		}

	}

}
