using UnityEngine;

public class AttackEffectSpawner : MonoBehaviour {

	//======================================================================| Properties

	[Tooltip("공격 이펙트 목표 (도착) 위치")]
	[field: SerializeField]
	public Transform TargetPosition { get; set; }

	[Tooltip("날아갈 오브젝트")]
	[field: SerializeField]
	public AttackEffect EffectPrefab { get; set; }

	//======================================================================| Fields

	private Canvas ThisCanvas;

	//======================================================================| Unity Behaviours

	private void Awake() {
		ThisCanvas = transform.GetComponentInParent<Canvas>();
	}

	//======================================================================| Methods

	public void Spawn() {
		
		AttackEffect instantiated = Instantiate(EffectPrefab, ThisCanvas.transform);
		instantiated.transform.position = transform.position;
		
		Shoot(instantiated);

	}

	private void Shoot(AttackEffect instantiated) {
		instantiated.Shoot(TargetPosition.gameObject);
	}

	//======================================================================| Nested Types

	public delegate void OnCompleateEventHandler();

}
