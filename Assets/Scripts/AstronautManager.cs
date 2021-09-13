using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautManager : ActivityManager
{
	[SerializeField]
    private const float TURN_SPEED = 20f;
	[SerializeField]
	private const float INTERVAL_BETWEEN_POINTS = 2f;
	[SerializeField]
	private float MOVE_SPEED = 20f;

	[SerializeField]
	private const int POINT_WORTH = 10;

	private float turnTime = 0.5f;

	private float timeTillNextTurn = 3f;

	private float playSoundTimer = 0f;

	private int turnDirection = 1;

	private int score = 0;

	private int timeLeft = 120;

	private float elapsedSecond = 1f;

	[SerializeField]
	private AudioClip pointClip;
	[SerializeField]
	private AudioClip stopMovingClip;

	private AudioSource audioSource;

	private Animator anim;
	private Transform cameraTransform;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;

    private Vector3 moveDirection = new Vector3(0, 0, 1);

	public static bool playing = false;

	public delegate void OneValueDelegate(int val);

	public static event OneValueDelegate UpdateScoreEvent;
	public static event OneValueDelegate UpdateTimeLeftEvent;
	public static event EmptyDelegate ReturnToMenu;

	private void Start()
	{
		cameraTransform = GetComponentInChildren<Camera>().transform;
		anim = gameObject.GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	protected override void UpdateBehaviour(bool state)
	{
		if (playing)
		{
			if (state)
			{

				if (playSoundTimer <= Mathf.Epsilon)
				{
					audioSource.PlayOneShot(pointClip);
					score += POINT_WORTH;
					UpdateScoreEvent?.Invoke(score);
					playSoundTimer = INTERVAL_BETWEEN_POINTS;
				}

				transform.parent.Rotate(Vector3.right * MOVE_SPEED * Time.deltaTime);

				if (timeTillNextTurn < 0)
				{
					if (turnTime > 0)
					{
						transform.parent.Rotate(Vector3.forward * turnDirection * TURN_SPEED * Time.deltaTime);
						turnTime -= Time.deltaTime;
					}
					else
					{
						timeTillNextTurn = Random.Range(3f, 10f);
						turnTime = Random.Range(1.5f, 3f);
						turnDirection = Random.Range(0f, 1f) > 0.5 ? 1 : -1;
					}
				}


				timeTillNextTurn -= Time.deltaTime;
				playSoundTimer -= Time.deltaTime;
			}

			else
			{
				if (playSoundTimer != 0)
				{
					audioSource.PlayOneShot(stopMovingClip);
					playSoundTimer = 0;
				}
			}

			if (elapsedSecond < 0)
			{
				elapsedSecond = 1f;
				timeLeft--;
				if (timeLeft >= 0)
					UpdateTimeLeftEvent?.Invoke(timeLeft);
				if(!(playing = timeLeft > 0))
					StartCoroutine(GameOver(5f));
			}

			elapsedSecond -= Time.deltaTime;

			anim.SetBool("AnimationPar", state);
		}

	}

	private IEnumerator GameOver(float time)
    {
		yield return new WaitForSeconds(time);
		score = 0;
		timeLeft = 120;
		elapsedSecond = 1f;
		DifficultyManager.instance.ActivityEnded();
		ReturnToMenu?.Invoke();
	}

}
