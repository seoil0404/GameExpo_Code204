using UnityEngine;

public class PoisonTest : MonoBehaviour {

	public KeyCode Key;
	public GameObject Target;

	private void Update() {
		if (Input.GetKeyDown(Key)) {
			HitEffectManager.Instance.OnPoison(Target);	
		}
	}

}
