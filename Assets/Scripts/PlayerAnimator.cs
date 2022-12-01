using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void SetAnimState(PlayerState playerState)
    {
        System.Array array = System.Enum.GetValues(typeof(PlayerState));
        for (int i = 0; i < array.Length; i++)
            anim.SetBool(array.GetValue(i).ToString(), false);

        anim.SetBool(playerState.ToString(), true);
    }

    public void FlipSprite()
    {
        if (Input.GetAxisRaw("Horizontal") < 0 && !spriteRenderer.flipX)
            spriteRenderer.flipX = true;

        if (Input.GetAxisRaw("Horizontal") > 0 && spriteRenderer.flipX)
            spriteRenderer.flipX = false;
    }
}
