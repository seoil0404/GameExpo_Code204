using DG.Tweening;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

	[field: Header("Hit Rotation")]
	[field: SerializeField]
	public float HitRotationAngle { get; private set; }

	[field: SerializeField]
	public float HitRotationDuration { get; private set; }

	[field: Header("Attack Rotation")]
	[field: SerializeField]
	public float AttackRotationAngle { get; private set; }

	[field: SerializeField]
	public float AttackRotationDuration { get; private set; }

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

	public void OnHit(GameObject receiverObject, Vector2 position, GameObject casterObject) {
		
		VisualEffect instantiated = Instantiate(HitEffect);
		instantiated.transform.position = position;

		StartCoroutine(SetSpriteColor(receiverObject));
		StartCoroutine(DestroyParticleOnDone(instantiated));
		RotateHittedObject(receiverObject, casterObject);

	}

	private IEnumerator SetSpriteColor(GameObject receiverObject) {

		var spriteDatas = receiverObject
			.GetComponentsInChildren<SpriteRenderer>()
			.Select(spriteRenderer => (spriteRenderer, spriteRenderer.color));

		var imageDatas = receiverObject
			.GetComponentsInChildren<Image>()
			.Select(imageRenderer => (imageRenderer, imageRenderer.color));

		float spent = 0f;
		while (spent < HitColorDuration) {

			foreach (var (spriteRenderer, color) in spriteDatas) {

				float progress = spent / HitColorDuration;
				Color currentColor = HitColor.Evaluate(progress);
				Color blendedColor = Color.Lerp(color, currentColor, 0.5f);
				
				spriteRenderer.color = blendedColor;

			}

			foreach (var (imageRenderer, color) in imageDatas) {
			
				float progress = spent / HitColorDuration;
				Color currentColor = HitColor.Evaluate(progress);
				Color blendedColor = Color.Lerp(color,currentColor, 0.5f);

				imageRenderer.color = blendedColor;

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

	private void RotateHittedObject(GameObject receiverObject, GameObject casterObject) {
		
		bool isFlipped = 
			receiverObject.transform.position.x <
			casterObject.transform.position.x;

		float angle = HitRotationAngle * (isFlipped ? 1f : -1f);

		receiverObject.transform.DOKill();
		receiverObject.transform.rotation = Quaternion.identity;

		Vector3 endRotation = Vector3.forward * angle;

		receiverObject.transform
			.DORotate(endRotation, HitRotationDuration / 2f)
			.SetEase(Ease.OutSine)
			.OnComplete(() => receiverObject.transform
				.DORotate(Vector3.zero, HitRotationDuration / 2f)
				.SetEase(Ease.InOutSine)
			);

	}

}