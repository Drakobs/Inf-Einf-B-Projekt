using UnityEngine;

public class Broom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _broom_anim;

    [Header("Values")]
    [SerializeField] public bool rb_simulated;
    [SerializeField] public bool isFlying;


    void Awake()
    {
        //deactivate gravity at start
        rb_simulated = false;
        _rb.simulated = rb_simulated;
    }

    // Update is called once per frame
    void Update()
    {
        //give animator bool
         _broom_anim.SetBool("isFlying", isFlying);

         //enable gravity if player is dead -> broom can fall off of player
         if(rb_simulated)
         {
            _rb.simulated = true;
         }
    }
}
