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

	protected Vector2 startPosition;

	//======================================================================| Methods

	public virtual void Spawn(Action onAttack = null) {
		
		AttackEffect instantiated = Instantiate(EffectPrefab);
		instantiated.transform.position = transform.position;
		
		Shoot(instantiated, onAttack);

	}

	private void Shoot(AttackEffect instantiated, Action onAttack) {
		instantiated.Shoot(TargetTransform.gameObject, transform.parent.gameObject, onAttack);
	}

	//======================================================================| Nested Types

	public delegate void OnCompleateEventHandler();

}
