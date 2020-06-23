using UnityEngine;
using System.Collections;

public class TerrainHole : MonoBehaviour 
{
	
	public Collider player; // assign in inspector?
	public TerrainCollider tCollider; // assign in inspector?
	public bool IsEnter = false;
	
	void Start ()
	{

	}
	void OnTriggerEnter (Collider c) 
	{
//		if (c.tag == "PlayerEmpty") 
//		{
			Physics.IgnoreCollision(player, tCollider, true);
			IsEnter = true;
//		}
	}
	
	void OnTriggerExit (Collider c) 
	{
//		if (c.tag == "PlayerEmpty") 
//		{
			Physics.IgnoreCollision(player, tCollider, false);
			IsEnter = false;
//		} 
	}

}