using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	

	private Animator anim;
	private Rigidbody rigidbody;
	private Transform cameraTransform;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;

	private ThinkGear thinkGear;

	[SerializeField]
	private AudioClip pointClip;
	[SerializeField]
	private AudioClip stopMovingClip;

	private AudioSource audioSource;

	private void Start () {
		rigidbody = GetComponent<Rigidbody>();
		cameraTransform = GetComponentInChildren<Camera>().transform;
		anim = gameObject.GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		

	}


}
