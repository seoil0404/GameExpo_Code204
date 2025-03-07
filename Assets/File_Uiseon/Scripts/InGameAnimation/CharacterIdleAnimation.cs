using UnityEngine;

public class CharacterIdleAnimation : MonoBehaviour
{

	//======================================================================| Fields

	[SerializeField]
	private float waveSpeed;

	[SerializeField]
	private float waveSize;

	[SerializeField]
	private float waveDecelerationOnDead;

	float timer = 0f;
	private Vector2 startPosition;

	//======================================================================| Unity Behaviours

	private void Start() {
		startPosition = transform.localPosition;
	}

	private void Update() {
		
		timer += Time.deltaTime;
		
		float positionDelta = waveSize * Mathf.Sin(waveSpeed * timer);
		float yPosition = startPosition.y + positionDelta;

		transform.localPosition = new(startPosition.x, yPosition);

	}
}
