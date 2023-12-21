using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : PlayerCapacity
{
    public override ModuleType myType => ModuleType.Slide;

    [Header(nameof(Slide))]
    [SerializeField] [Range(0, 1)] float cancelThreshold = 0.3f;
    [SerializeField] float slidingDuration = 3f;
    [SerializeField] [Curve(1, 50)] AnimationCurve slidingCurve;

    public override void Effect()
    {
        StartCoroutine(Sliding());
    }

    IEnumerator Sliding()
    {
        Player.ResetYVelocity();
        Player.SetState(PlayerState.IsSliding);
        float timer = 0;

        while (timer < slidingDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            float step = timer / slidingDuration;
            float force = slidingCurve.Evaluate(step);

            Vector2 newVelocity = Player.Rigidbody.velocity;
            newVelocity.x = Player.InputDirection * force;
            Player.Rigidbody.velocity = newVelocity;

            //print(step);
            if (step >= cancelThreshold && Input.GetButtonDown("Propulsion"))
            {
                print("super propulsion");
                Player.Propulsion();
                break;
            }
        }

        Player.SetState(PlayerState.Idle);
    }
}
