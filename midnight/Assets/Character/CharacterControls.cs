using UnityEngine;
using System.Collections.Generic;

public abstract class CharacterControls : MonoBehaviour 
{
	public float CharacterSpeed = 1.0f;
	
	protected MainCharacter Character { get; set; }
	protected SceneGui Gui { get; set; }
	public float jumpParallaxHeight = 100.0f;
	
	protected virtual void Start()
	{
		Character = Find.ObjectInScene<MainCharacter>();
		Gui = Find.ObjectInScene<SceneGui>();
	}
	
	
	// Move the character (negative left, positive right)
	protected void Translate(float amount)
	{
		CharacterController controller = GetComponent<CharacterController>();
		Vector3 translation = Vector3.right * amount;
		controller.SimpleMove( translation );
	
		PostTranslate(translation);
	}
	
	protected void ForceTranslate(float amount)
	{
		CharacterController controller = GetComponent<CharacterController>();
		Vector3 translation = Vector3.right * amount;
		controller.Move( translation );
		
		PostTranslate(translation);
	}
	
	protected virtual void PostTranslate(Vector3 translation)
	{
		OTSprite sprite = GetComponent<OTSprite>();
		sprite.flipHorizontal = translation.x > 0.0f;
	}
	
	protected void JumpBackParallax()
	{
		
	}
	
	protected void JumpForwardParallax()
	{
		
	}
	
	private IEnumerator<IYieldInstruction> JumpCoroutine(Vector3 target)
	{
		float targetDir = target.x < transform.position.x ? -1.0f : 1.0f;
		float frameSpeed = CharacterSpeed * Time.deltaTime;
		float frameVelocity = targetDir * frameSpeed;
		
		while( (target.x < transform.position.x ? -1.0f : 1.0f) == targetDir ) 
		{
			Translate(frameVelocity);
			yield return Yield.UntilNextFrame;
		}
	}
}
