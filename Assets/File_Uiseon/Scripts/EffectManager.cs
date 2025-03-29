using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EffectManager : MonoBehaviour {

	//======================================================================| Singleton

	public static EffectManager Instance = null;

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

	[Header("Shield")]
	[SerializeField]
	private GameObject ShieldObject;
	private readonly Dictionary<GameObject, ShieldEffect> shieldEffects = new();

	[Header("Buff")]
	[SerializeField]
	private VisualEffect ArrowDownEffect;

	[SerializeField]
	private VisualEffect ArrowUpEffect;

	[SerializeField]
	private float ArrowDuration;

	[Header("Dragon Breath")]
	[SerializeField]
	private VisualEffect DragonBreathObject;

	[SerializeField]
	private float DragonBreashDuration;

	//======================================================================| Colors

	public static readonly Color ColorOfThron = new(15, 145, 17);
	public static readonly Color ColorOfPower = new(191, 10, 14);
	public static readonly Color ColorOfHealDecreasment = new(191, 10, 14);
	public static readonly Color ColorOfWeakness = new(56, 0, 191);
	public static readonly Color ColorOfSilence = new(255, 255, 255);

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

	public void SpawnShield(GameObject target) {
		if (shieldEffects.TryGetValue(target, out var instantiatedShield)) {
			if (instantiatedShield == null) {
				shieldEffects.Remove(target);
			}
			else if (!instantiatedShield.isActiveAndEnabled) {
				Destroy(instantiatedShield.transform.parent.gameObject);
				shieldEffects.Remove(target);
			}
			else {
				return;
			}
		}

		GameObject instantiated = Instantiate(ShieldObject);
		instantiated.transform.position = target.transform.position;
		instantiated.transform.parent = target.transform;

		ShieldEffect shieldEffect = instantiated.GetComponentInChildren<ShieldEffect>();
		shieldEffects[target] = shieldEffect;


	}

	public void RemoveShield(GameObject target) {

		if (shieldEffects.TryGetValue(target, out var instantiated)) {
			if (instantiated != null && !instantiated.isActiveAndEnabled) {
				Destroy(instantiated.transform.parent.gameObject);
			}
			else {
				instantiated.Stop();
			}
			shieldEffects.Remove(target);

		}

	}

	public void OnBuff(GameObject target, Color color) {
		
		VisualEffect instantiated = Instantiate(ArrowUpEffect);
		instantiated.transform.position = target.transform.position;

		instantiated.SetVector4("Color", color);
		StartCoroutine(RemoveWhen(instantiated, ArrowDuration));

	}

	public void OnDebuff(GameObject target, Color color) {
		
		VisualEffect instantiated = Instantiate(ArrowDownEffect);
		instantiated.transform.position = target.transform.position;

		instantiated.SetVector4("Color", new(color.r, color.g, color.b, 0f));
		StartCoroutine(RemoveWhen(instantiated, ArrowDuration));

	}

	public void OnDragonBreath(GameObject target, GameObject caster) {
		StartCoroutine(OnDragonBreathInner(target, caster));
	}

	private IEnumerator OnDragonBreathInner(GameObject target, GameObject caster) {

		VisualEffect instantiated = Instantiate(DragonBreathObject);
		instantiated.transform.position = caster.transform.position;

		instantiated.SetVector2("TargetPosition", target.transform.position - caster.transform.position);

		yield return new WaitForSeconds(DragonBreashDuration);

		instantiated.Stop();

		yield return new WaitUntil(() => instantiated.aliveParticleCount == 0);

		Destroy(instantiated.gameObject);

	}

	private IEnumerator RemoveWhen(VisualEffect visualEffect, float time) {
		yield return new WaitForSeconds(time);
		visualEffect.Stop();

		yield return new WaitForSeconds(1f);
		Destroy(visualEffect.gameObject);
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
				spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
				//spriteRenderer.color.WithAlpha(opacity);
			}

			foreach (var imageRenderer in imageRenderers) {
				imageRenderer.color = new(imageRenderer.color.r, imageRenderer.color.g, imageRenderer.color.b, opacity);
			}

			spent += Time.deltaTime;
			yield return null;

		}

		foreach (var spriteRenderer in spriteRenderers) {
			spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
		}
		foreach (var imageRenderer in imageRenderers) {
			imageRenderer.color = new(imageRenderer.color.r, imageRenderer.color.g, imageRenderer.color.b, 1f);
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