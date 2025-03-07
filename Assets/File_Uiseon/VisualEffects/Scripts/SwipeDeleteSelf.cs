using UnityEngine;
using UnityEngine.VFX;

public class SwipeDeleteSelf : MonoBehaviour {

	VisualEffect visualEffect;

	private void Awake() {
		visualEffect = GetComponent<VisualEffect>();
	}

	void Update() {
        if (visualEffect.aliveParticleCount == 0) {
			Destroy(gameObject);
		}
    }
}
