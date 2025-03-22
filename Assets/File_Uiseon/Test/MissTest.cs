using UnityEngine;

public class MissTest : MonoBehaviour
{
    public KeyCode key;
	public GameObject me;
	public GameObject you;

	private void Update() {
		
		if (Input.GetKeyDown(key)) {
			HitEffectManager.Instance.OnMiss(you, me);
		}

	}

}
