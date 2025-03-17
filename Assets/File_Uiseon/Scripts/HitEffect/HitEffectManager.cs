using DG.Tweening;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEditor.Experimental.GraphView.Port;

public class HitEffectManager : MonoBehaviour {

	//======================================================================| Singleton

	public static HitEffectManager Instance = null;

	//======================================================================| Fields

	[Header("Hit Effect")]
	[SerializeField]
	private VisualEffect HitEffect;

	[SerializeField]
	private GameObject PoisonObject;

	[Header("Hit Color Change")]
	[SerializeField]
	private float HitColorDuration;

	[SerializeField]
	private Gradient HitColor;

	[SerializeField]
	private Gradient PoisonColor;

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

	[Header("Miss")]
	[SerializeField]
	private GameObject MissObject;

	[SerializeField]
	private float MissMovement;

	[SerializeField]
	private float MissDuration;

	[SerializeField]
	private Ease MissEase;

	[Header("Miss Blink")]
	[SerializeField]
	private float BlinkOpacity;

	[SerializeField]
	private float BlinkDuration;

	[SerializeField]
	private float BlinkSpeed;

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

		StartCoroutine(SetSpriteColor(receiverObject, HitColor));
		StartCoroutine(DestroyParticleOnDone(instantiated));
		RotateHittedObject(receiverObject, casterObject);

	}

	public void OnPoison(GameObject receiverObject) {

		VisualEffect instantiated = Instantiate(PoisonObject).GetComponentInChildren<VisualEffect>();
		instantiated.transform.position = receiverObject.transform.position;

		StartCoroutine(SetSpriteColor(receiverObject, PoisonColor));
		StartCoroutine(DestroyParticleWithParentOnDone(instantiated, 1));

	}

	public void OnMiss(GameObject receiverObject, GameObject casterObject) {

		GameObject instantiated = Instantiate(MissObject);
		instantiated.transform.position = receiverObject.transform.position;

		instantiated.transform
			.DOLocalMove(receiverObject.transform.position + Vector3.up * MissMovement, MissDuration)
			.SetEase(MissEase);

		instantiated.GetComponentInChildren<SpriteRenderer>()
			.DOFade(0f, MissDuration)
			.SetEase(MissEase)
			.OnComplete(() => {
				Destroy(instantiated);
			});

		StartCoroutine(BlinkMissObject(receiverObject));
		RotateHittedObject(receiverObject, casterObject);

	}

	private IEnumerator SetSpriteColor(GameObject receiverObject, Gradient gradient) {

		var spriteDatas = receiverObject
			.GetComponents<SpriteRenderer>()
			.Select(spriteRenderer => (spriteRenderer, spriteRenderer.color))
			.ToList();

		var imageDatas = receiverObject
			.GetComponents<Image>()
			.Select(imageRenderer => (imageRenderer, imageRenderer.color))
			.ToList();

		float spent = 0f;
		while (spent < HitColorDuration) {

			foreach (var (spriteRenderer, color) in spriteDatas) {

				float progress = spent / HitColorDuration;
				Color currentColor = gradient.Evaluate(progress);
				Color blendedColor = Color.Lerp(color, currentColor, 0.5f);
				
				spriteRenderer.color = blendedColor;

			}

			foreach (var (imageRenderer, color) in imageDatas) {
			
				float progress = spent / HitColorDuration;
				Color currentColor = gradient.Evaluate(progress);
				Color blendedColor = Color.Lerp(color, currentColor, 0.5f);

				imageRenderer.color = blendedColor;

			}

			spent += Time.deltaTime;
			yield return null;

		}

		foreach (var (spriteRenderer, color) in spriteDatas) {
			spriteRenderer.color = Color.white;
		}

	}

	private IEnumerator DestroyParticleOnDone(VisualEffect effect) {
		yield return new WaitUntil(() => effect.aliveParticleCount == 0);
		Destroy(effect.gameObject);
	}

	private IEnumerator DestroyParticleWithParentOnDone(VisualEffect effect, int parentLevel) {

		yield return new WaitUntil(() => effect.aliveParticleCount == 0);

		Transform parent = effect.transform;
		for (int i = 0; i < parentLevel; i++) {
			parent = parent.parent;
		}

		Destroy(parent.gameObject);

	}

	private IEnumerator BlinkMissObject(GameObject receiverObject) {

		SpriteRenderer[] spriteRenderers = receiverObject.GetComponentsInChildren<SpriteRenderer>();
		Image[] imageRenderers = receiverObject.GetComponentsInChildren<Image>();

		float spent = 0f;

		while (spent < BlinkDuration) {

		float amplitude = 0.5f - (BlinkOpacity * 0.5f);
		float offset = 0.5f + (BlinkOpacity * 0.5f);
		float opacity = amplitude * Mathf.Sin(BlinkSpeed * Time.time) + offset;

			Debug.Log(opacity);

			foreach (var spriteRenderer in spriteRenderers) {
				spriteRenderer.color = spriteRenderer.color.WithAlpha(opacity);
			}

			foreach (var imageRenderer in imageRenderers) {
				imageRenderer.color = imageRenderer.color.WithAlpha(opacity);
			}

			spent += Time.deltaTime;
			yield return null;

		}

		foreach (var spriteRenderer in spriteRenderers) {
			spriteRenderer.color = spriteRenderer.color.WithAlpha(1f);
		}

		foreach (var imageRenderer in imageRenderers) {
			imageRenderer.color = imageRenderer.color.WithAlpha(1f);
		}
		
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