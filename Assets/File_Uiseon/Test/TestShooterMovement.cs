using UnityEngine;

public class TestShooterMovement : MonoBehaviour {

	public float Speed = 0.25f;

	public void FixedUpdate() {
		
		Vector2 input = new();
		if (Input.GetKey(KeyCode.UpArrow)) input.y++;
		if (Input.GetKey(KeyCode.DownArrow)) input.y--;
		if (Input.GetKey(KeyCode.RightArrow)) input.x++;
		if (Input.GetKey(KeyCode.LeftArrow)) input.x--;

		input.Normalize();

		transform.position += (Vector3)input * Speed;

	}

}
