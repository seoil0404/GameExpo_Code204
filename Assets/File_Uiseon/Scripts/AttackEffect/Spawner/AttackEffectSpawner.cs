using System;
using UnityEngine;

public class AttackEffectSpawner : MonoBehaviour {

	//======================================================================| Properties

	[Tooltip("공격 이펙트 목표 (도착) 위치")]
	[field: SerializeField]
	public Transform TargetTransform { get; set; }

	[Tooltip("날아갈 오브젝트")]
	[field: SerializeField]
	public AttackEffect EffectPrefab { get; set; }

	public Action OnHit { get; set; } = () => {};

	//======================================================================| Fields

	protected Canvas curentCanvas;
	protected Vector2 startPosition;

	//======================================================================| Unity Behaviours

	private void Awake() {
		curentCanvas = transform.GetComponentInParent<Canvas>();
		startPosition = transform.parent.position;
	}

	//======================================================================| Methods

	public virtual void Spawn() {
		
		AttackEffect instantiated = Instantiate(EffectPrefab, curentCanvas.transform);
		instantiated.transform.position = transform.position;
		instantiated.OnHit = OnHit;
		
		Shoot(instantiated);

	}

	private void Shoot(AttackEffect instantiated) {
		instantiated.Shoot(TargetTransform.gameObject, transform.parent.gameObject);
	}

	//======================================================================| Nested Types

	public delegate void OnCompleateEventHandler();

}
