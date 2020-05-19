using UnityEngine;

 public class ParentScript : MonoBehaviour
{
	public void CollisionDetected(movable childScript)
	{
		Debug.Log("child collided");
	}
}
