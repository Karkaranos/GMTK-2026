using NaughtyAttributes;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RocketMover : MonoBehaviour
{
    [SerializeField, Tooltip("Multiplies calculataed distance from the progress manager by this.")] 
    private float unitScaler = 1;
    [SerializeField] private float launchDelay;
    [SerializeField] private float rocketSpeed;
    [SerializeField] private float rocketAcceleration;
    [SerializeField, Range(0, 1), Tooltip("Speed of the rocket as it drifts after reaching it's peak height.")] 
    private float driftSpeed;
    [SerializeField, Tooltip("How far the rocket needs to travel to escape the gravitational field.  " +
        "Should be approximately how far the player needs to get to \"win.\"")] 
    private float maxGravityFieldDistance;
    [SerializeField] private AnimationCurve gravityFalloffCurve;
    [SerializeField] private float debugHeight;

    [SerializeField, BoxGroup("References")] private CinemachineCamera rocketCam;
    [SerializeField, BoxGroup("Components")] private Rigidbody2D rb;

    private struct FlyTime
    {
        internal float accelerateTime;
        internal float constantSpeedTime;
    }

    private void Awake()
    {
        //StartCoroutine(LaunchRoutine(ProgressManager.DistanceFlown * unitScaler));
    }

    [Button]
    private void TestLaunch()
    {
        StartCoroutine(LaunchRoutine(debugHeight));
    }

    private IEnumerator LaunchRoutine(float flyDistance)
    {
        yield return new WaitForSeconds(launchDelay);
        // Calculate the time the rocket needs to accelerate, decellerate, and fly at a constant speed.
        
        FlyTime times = GetFlyTimes(flyDistance);

        float baseGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        float velocity = 0;
        // Accelerate to max speed.
        float timer = times.accelerateTime;
        while(timer > 0)
        {
            velocity = Mathf.MoveTowards(velocity, rocketSpeed, rocketAcceleration * Time.deltaTime);

            rb.linearVelocityY = velocity;
            timer -= Time.deltaTime;
            yield return null;
        }

        // Move at a constant speed.
        yield return new WaitForSeconds(times.constantSpeedTime);

        timer = times.accelerateTime;
        while (timer > 0)
        {
            velocity = Mathf.MoveTowards(velocity, driftSpeed, rocketAcceleration * Time.deltaTime);

            rb.linearVelocityY = velocity;
            timer -= Time.deltaTime;
            yield return null;
        }

        float flownHeight = rb.position.y;
        //Reapply gravity based on how far the rocket traveled.
        float normalizedGravDistance = Mathf.Clamp01(flownHeight / maxGravityFieldDistance);
        rb.gravityScale = baseGravityScale * (1 - gravityFalloffCurve.Evaluate(normalizedGravDistance));
        rocketCam.Follow = null;

        // Capture the max reached height.
        Debug.Log(rb.position.y);
    }

    private FlyTime GetFlyTimes(float flyDistance)
    {
        FlyTime resultTime = new FlyTime();

        float timeToAccelerate = rocketSpeed / rocketAcceleration;
        float acceleratingDistanceTraveled = rocketAcceleration * Mathf.Pow(timeToAccelerate, 2) / 2;

        if (flyDistance < 2 * acceleratingDistanceTraveled)
        {
            // Calculate using partial accelerations
            resultTime.constantSpeedTime = 0;
            resultTime.accelerateTime = Mathf.Sqrt(flyDistance / rocketAcceleration);
        }
        else
        {
            resultTime.accelerateTime = timeToAccelerate;

            float remainingDistance = flyDistance - 2 * acceleratingDistanceTraveled;
            float remainingTime = remainingDistance / rocketSpeed;
            resultTime.constantSpeedTime = remainingTime;
        }

        return resultTime;
    }
}
