using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : PlayerCapacity
{
    [SerializeField] [Range(0, 1)] float cancelThreshold = 0.3f;
    [SerializeField] [Curve(1, 50)] AnimationCurve slidingCurve;
    [SerializeField] float slidingDuration = 3f;

    public override ModuleType myType => ModuleType.Slide;

    public override void Effect()
    {
        StartCoroutine(Sliding());
    }

    IEnumerator Sliding()
    {
        Player.ResetVelocity();
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
            if (step >= cancelThreshold && Input.GetKeyDown(KeyCode.LeftShift))
            {
                print("super propulsion");
                Player.UseModule(ModuleType.Propulsion);
                break;
            }
        }

        Player.SetState(PlayerState.Idle);
    }
}
