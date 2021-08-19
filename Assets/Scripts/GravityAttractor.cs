using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    [SerializeField]
    public float gravity = -3f;

    public void Attract(Rigidbody body)
    {
        Vector3 targetDirection = (body.position - this.transform.position).normalized;
        Vector3 bodyUp = body.transform.up;
        body.rotation = Quaternion.FromToRotation(bodyUp, targetDirection) * body.rotation;
        body.AddForce(targetDirection * gravity);
    }
}
