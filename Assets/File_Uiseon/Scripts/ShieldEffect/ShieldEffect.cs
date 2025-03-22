using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShieldEffect : MonoBehaviour {

	//======================================================================| Members

	[Header("Size")]
	[SerializeField]
	private float _sizeAmplitude;
	
	[SerializeField]
	private float _sizeSpeed;

	[Header("Opacity")]
	[SerializeField]
	private float _defaultOpacity;

	[SerializeField]
	private float _opacityAmplitude;

	[SerializeField]
	private float _opacitySpeed;

	[Header("Spawn")]
	[SerializeField]
	private float _spawnDuration;

	[SerializeField]
	private Ease _spawnEase;

	[Header("Destroy")]

	[SerializeField]
	private float _destroyScale;

	[SerializeField]
	private float _destroyDuration;

	[SerializeField]
	private Ease _destroyEase;

	private bool _break;
	private SpriteRenderer _spriteRenderer;

	private Color _defaultColor;
	private float _defaultSize;

	private float _sizeWaveMultiply;
	private float _opacityWaveMultiply;

	private float _sizeMultiply = 0;
	private float _opacityMultiply = 0;

	//======================================================================| Unity Behaviours

	private void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		
		_defaultSize = transform.localScale.x;
		_defaultColor = _spriteRenderer.color;

		transform.localScale = Vector2.zero;
		_spriteRenderer.color = _defaultColor * new Color(1f, 1f, 1f, 0f);

		SpawnSize();
		SpawnOpacity();

		StartCoroutine(UpdateSize());
		StartCoroutine(UpdateOpacity());

	}

	private void Update() {
		
		float scale = _sizeMultiply * (_defaultSize + _sizeWaveMultiply);

		transform.localScale = Vector2.one * scale;
	
		_spriteRenderer.color = new(
			_defaultColor.r,
			_defaultColor.g,
			_defaultColor.b,
			_opacityMultiply * _opacityWaveMultiply
		);

	}

	//======================================================================| Methods

	public void Stop() => _break = true;

	private void SpawnSize() {

		DOTween
			.To(() => _sizeMultiply, x => _sizeMultiply = x, 1f, _spawnDuration)
			.SetEase(_spawnEase);

	}

	private void SpawnOpacity() {

		DOTween
			.To(() => _opacityMultiply, x => _opacityMultiply = x, _defaultOpacity, _spawnDuration)
			.SetEase(_spawnEase);

	}

	private IEnumerator UpdateSize() {

		while (!_break) {
			
			_sizeWaveMultiply = _sizeAmplitude * Mathf.Sin(Time.time * _sizeSpeed);
			yield return null;

		}

		DOTween
			.To(() => _sizeMultiply, x => _sizeMultiply = x, _destroyScale, _destroyDuration)
			.SetEase(_destroyEase);

		yield return new WaitForSeconds(_destroyDuration);
		yield return null;

		Destroy(transform.parent.gameObject);

	}

	private IEnumerator UpdateOpacity() {
	
		while (!_break) {

			float opacityDelta = _opacityAmplitude * Mathf.Sin(Time.time * _opacitySpeed);
			_opacityWaveMultiply = _defaultOpacity + opacityDelta;

			yield return null;

		}

		DOTween
			.To(() => _opacityMultiply, x => _opacityMultiply = x, 0f, _destroyDuration)
			.SetEase(_destroyEase);

	}


}