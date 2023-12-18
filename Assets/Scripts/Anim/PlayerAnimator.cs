using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void SetAnimState(PlayerState playerState)
    {
        System.Array array = System.Enum.GetValues(typeof(PlayerState));
        for (int i = 0; i < array.Length; i++)
            anim.SetBool(array.GetValue(i).ToString(), false);

        anim.SetBool(playerState.ToString(), true);
    }

    public void FlipSprite()
    {
        bool flip = CameraManager.Instance.MousePosition().x < transform.position.x;
        if (flip)
            transform.rotation = Quaternion.Euler(Vector3.up * 180);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
