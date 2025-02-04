using UnityEngine;

public class TestTargetMovement : MonoBehaviour {

	public float Speed = 0.2f;

	public void FixedUpdate() {
		
		Vector2 input = new();
		if (Input.GetKey(KeyCode.W)) input.y++;
		if (Input.GetKey(KeyCode.S)) input.y--;
		if (Input.GetKey(KeyCode.D)) input.x++;
		if (Input.GetKey(KeyCode.A)) input.x--;

		input.Normalize();

		transform.position += (Vector3)input * Speed;

	}

}
