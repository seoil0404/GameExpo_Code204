using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class HitEffectManager : MonoBehaviour {

	//======================================================================| Singleton

	public static HitEffectManager Instance = null;

	//======================================================================| Fields

	[Header("Hit Effect")]
	[SerializeField]
	private VisualEffect HitEffect;

	[Header("Hit Color Change")]
	[SerializeField]
	private float HitColorDuration;

	[SerializeField]
	private Gradient HitColor;

	//======================================================================| Unity Behaviours

	private void Awake() {
		if (
			Instance.IsUnityNull() ||
			Instance.IsDestroyed() ||
			Instance.isActiveAndEnabled == false
		) {
			Instance = this;
		}
	}

	//======================================================================| Methods

	public void OnHit(GameObject gameObject, Vector2 position) {
		
		VisualEffect instantiated = Instantiate(HitEffect);
		instantiated.transform.position = position;

		StartCoroutine(SetSpriteColor(gameObject));
		StartCoroutine(DestroyParticleOnDone(instantiated));

	}

	private IEnumerator SetSpriteColor(GameObject gameObject) {

		var spriteDatas = gameObject
			.GetComponentsInChildren<SpriteRenderer>()
			.Select(spriteRenderer => (spriteRenderer, spriteRenderer.color));

		float spent = 0f;
		while (spent < HitColorDuration) {

			foreach (var (spriteRenderer, color) in spriteDatas) {

				float progress = spent / HitColorDuration;
				Color currentColor = HitColor.Evaluate(progress);
				Color blendedColor = Color.Lerp(color, currentColor, 0.5f);
				
				spriteRenderer.color = blendedColor;

			}

			spent += Time.deltaTime;
			yield return null;

		}

		foreach (var (spriteRenderer, color) in spriteDatas) {
			spriteRenderer.color = color;
		}

	}

	private IEnumerator DestroyParticleOnDone(VisualEffect effect) {
		yield return new WaitUntil(() => effect.aliveParticleCount == 0);
		Destroy(effect.gameObject);
	}

}