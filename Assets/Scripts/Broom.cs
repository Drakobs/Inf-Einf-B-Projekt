using UnityEngine;

public class Broom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _broom_anim;
    [SerializeField] Player player;
    [SerializeField] Collider2D broom_collider;

    [Header("Values")]
    [SerializeField] public bool rb_simulated;
    [SerializeField] public bool isFlying;



    void Awake()
    {
        //deactivate gravity at start
        _broom_anim.SetBool("isAlive", true);
        rb_simulated = false;
        _rb.simulated = rb_simulated;
        broom_collider.enabled = false;
    }

    void Start()
    {
        player.Died += OnDeath;
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
    public void PauseAnim(bool isPaused)
    {
        float anim_speed;
        if(isPaused == true)
        {
            anim_speed = 0f;
        }
        else
        {
            anim_speed = 1f;
        }
    _broom_anim.speed = anim_speed;
    }
    public void OnDeath()
    {
        broom_collider.enabled = true;
        _broom_anim.SetBool("isAlive", false);
    }
private void OnDestroy()
{
    player.Died -= OnDeath;
}

}


