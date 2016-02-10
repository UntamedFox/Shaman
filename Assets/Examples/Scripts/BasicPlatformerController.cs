

/*****************************************************************************
 * Basic Platformer Controller created by Mitch Thompson
 * Full irrevocable rights and permissions granted to Esoteric Software
*****************************************************************************/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BasicPlatformerController : MonoBehaviour {

#if UNITY_4_5
    [Header("Controls")]
#endif
	public string XAxis = "Horizontal";
	public string YAxis = "Vertical";
	public string JumpButton = "Jump";

#if UNITY_4_5
	[Header("Moving")]
#endif
	public float walkSpeed = 4;
	public float runSpeed = 10;
	public float gravity = 65;

#if UNITY_4_5
	[Header("Jumping")]
#endif
	public float jumpSpeed = 25;
	public float jumpDuration = 0.5f;
	public float jumpInterruptFactor = 100;
	public float forceCrouchVelocity = 25;
	public float forceCrouchDuration = 0.5f;

#if UNITY_4_5
	[Header("Graphics")]
#endif
	public Transform graphicsRoot;
	public SkeletonAnimation skeletonAnimation;

#if UNITY_4_5
	[Header("Animation")]
#endif
    [SpineAnimation(dataField: "skeletonAnimation")]
    public string deathName = "death";
    //[SpineAnimation(dataField: "skeletonAnimation")]
	//public string runName = "run";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string idleName = "idle";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string jumpName = "jump";
	//[SpineAnimation(dataField: "skeletonAnimation")]
	//public string fallName = "Fall";
	//[SpineAnimation(dataField: "skeletonAnimation")]
	//public string crouchName = "Crouch";

#if UNITY_4_5
	[Header("Audio")]
#endif
	public AudioSource jumpAudioSource;
	//public AudioSource hardfallAudioSource;
	//public AudioSource footstepAudioSource;
	[SpineEvent]
	public string footstepEventName = "Footstep";
	CharacterController charController;
	Vector2 velocity = Vector2.zero;
	Vector2 lastVelocity = Vector2.zero;
	bool lastGrounded = false;
	float jumpEndTime = 0;
	bool jumpInterrupt = false;
	float forceCrouchEndTime;
	Quaternion flippedRotation = Quaternion.Euler(0, 180, 0);

	void Awake () {
		charController = GetComponent<CharacterController>();
	}

	void Start () {
		//register a callback for Spine Events (in this case, Footstep)
		skeletonAnimation.state.Event += HandleEvent;
	}

	void HandleEvent (Spine.AnimationState state, int trackIndex, Spine.Event e) {
		//play some sound if footstep event fired
		if (e.Data.Name == footstepEventName) {
			//footstepAudioSource.Stop();
			//footstepAudioSource.Play();
		}
	}

	void Update () {
		//control inputs
		float x = Input.GetAxis(XAxis);
		float y = Input.GetAxis(YAxis);
		//check for force crouch
		bool crouching = (charController.isGrounded && y < -0.5f) || (forceCrouchEndTime > Time.time);
		velocity.x = 0;

		//Calculate control velocity
		if (!crouching) { 
			if (Input.GetButtonDown(JumpButton) && charController.isGrounded) {
				//jump
				jumpAudioSource.Stop();
				jumpAudioSource.Play();
				velocity.y = jumpSpeed;
				jumpEndTime = Time.time + jumpDuration;
			} else if (Time.time < jumpEndTime && Input.GetButtonUp(JumpButton)) {
					jumpInterrupt = true;
				}

            
			if (x != 0) {
				//walk or run
				velocity.x = Mathf.Abs(x) > 0.6f ? runSpeed : walkSpeed;
				velocity.x *= Mathf.Sign(x);
			}

			if (jumpInterrupt) {
				//interrupt jump and smoothly cut Y velocity
				if (velocity.y > 0) {
					velocity.y = Mathf.MoveTowards(velocity.y, 0, Time.deltaTime * 100);
				} else { 
					jumpInterrupt = false;
				}
			}
		}

		//apply gravity F = mA (Learn it, love it, live it)
		velocity.y -= gravity * Time.deltaTime;

		//move
		charController.Move(new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime);
        
		if (charController.isGrounded) {
			//cancel out Y velocity if on ground
			velocity.y = -gravity * Time.deltaTime;
			jumpInterrupt = false;
		}

        
		Vector2 deltaVelocity = lastVelocity - velocity;

		if (!lastGrounded && charController.isGrounded) {
			//detect hard fall
			if ((gravity * Time.deltaTime) - deltaVelocity.y > forceCrouchVelocity) {
				forceCrouchEndTime = Time.time + forceCrouchDuration;
				//hardfallAudioSource.Play();
			} else {
				//play footstep audio if light fall because why not
				//footstepAudioSource.Play();
			}
            
		}

		//graphics updates
		if (charController.isGrounded) {
			if (crouching) { //crouch
				//skeletonAnimation.AnimationName = crouchName;
			} else {
				if (x == 0) //idle
					skeletonAnimation.AnimationName = idleName;
				else {
					//move
					//skeletonAnimation.AnimationName = Mathf.Abs(x) > 0.6f ? runName : walkName;
					//skeletonAnimation.AnimationName = runName;
				}
			}
		} else {
			if (velocity.y > 0) //jump
				skeletonAnimation.AnimationName = jumpName;
			//else //fall
				//skeletonAnimation.AnimationName = fallName;
		}

		//flip left or right
		if (x > 0)
			graphicsRoot.localRotation = Quaternion.identity;
		else if (x < 0)
				graphicsRoot.localRotation = flippedRotation;


		//store previous state
		lastVelocity = velocity;
		lastGrounded = charController.isGrounded;
	}
}