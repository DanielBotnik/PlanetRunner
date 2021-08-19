using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautManager : ActivityManager
{

    private const float TURN_SPEED = 1f;
    private const int MINIMUM_MEDITATION_LEVEL = 25;
    private const int MINIMUM_ATTETION_LEVEL = 25;
    private const float INTERVAL_BETWEEN_SOUNDS = 2f;

	[SerializeField]
	private float moveSpeed = 3;

	private float turnTime = 0.5f;
	private float timeTillNextTurn = 3f;
	private float playSoundTimer = 0f;
	private int turnSize = 1;

	[SerializeField]
	private AudioClip pointClip;
	[SerializeField]
	private AudioClip stopMovingClip;

	private AudioSource audioSource;

	private Animator anim;
	private Rigidbody rigidbody;
	private Transform cameraTransform;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		cameraTransform = GetComponentInChildren<Camera>().transform;
		anim = gameObject.GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	protected override void UpdateBehaviour(bool state)
    {
		if (state)
		{
            if (playSoundTimer <= Mathf.Epsilon)
            {
                audioSource.PlayOneShot(pointClip);
                playSoundTimer = INTERVAL_BETWEEN_SOUNDS;
            }
            anim.SetInteger("AnimationPar", 1);
            Vector3 moveDirection = new Vector3(0, 0, 1).normalized;
            Vector3 targetMoveAmount = moveDirection * moveSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);


            if (timeTillNextTurn < 0)
            {
                if (turnTime > 0)
                {
                    Quaternion newRotation = rigidbody.rotation * new Quaternion(0, 1f, 0, turnSize * 1f);
                    var moveTo = Quaternion.Lerp(rigidbody.rotation, newRotation, Time.deltaTime * TURN_SPEED);
                    rigidbody.MoveRotation(moveTo.normalized);
                    turnTime -= Time.deltaTime;
                }
                else
                {
                    timeTillNextTurn = Random.Range(3f, 10f);
                    turnTime = Random.Range(0.5f, 2f);
                    turnSize = Random.Range(0f, 1f) > 0.5 ? 1 : -1;

                }
            }

            cameraTransform.localEulerAngles = Vector3.left;
            timeTillNextTurn -= Time.deltaTime;
            playSoundTimer -= Time.deltaTime;

        }

		else
		{
			if (playSoundTimer != 0)
			{
				audioSource.PlayOneShot(stopMovingClip);
				anim.SetInteger("AnimationPar", 0);
				playSoundTimer = 0;
			}
		}
	}

    private void FixedUpdate()
    {
        if (HasReachedProtocol)
            rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }
}
