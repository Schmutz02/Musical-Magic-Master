using System;
using MMM.DialogSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMM
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        public Transform GroundCheck;
        public LayerMask GroundLayer;

        public float MoveSpeed;

        public GameObject JumpEffect;
        public float JumpForce;
        public float MaxJumpDuration;
        public float CoyoteTimer;

        private bool _jumping;
        private float _jumpEnd;
        private float _coyoteTimerEnd;
        private float _currentJumpForce;
        private int _airJumpCount;

        [HideInInspector]
        public bool Grounded;
        private bool _groundedLastFrame;

        private float _horizontalVel;
        private bool _horizontalFlip;

        [SerializeField]
        private bool _canDoubleJump;

        [SerializeField]
        private bool _canGravityFlip;
        private int _flipCount;

        public AudioSource AudioSource;
        public AudioClip[] JumpSounds;
        public AudioClip FlipUpSound;
        public AudioClip FlipDownSound;

        public void AllowDoubleJump()
        {
            _canDoubleJump = true;
        }

        public void AllowGravityFlip()
        {
            _canGravityFlip = true;
        }

        private void Update()
        {
            if (DialogManager.Instance.DialogActive)
                return;
            
            HandleMovementLogic();
            HandleJumpLogic();
            HandleFlipLogic();
        }

        // private void LateUpdate()
        // {
        //     var pos = transform.position;
        //     pos.x = Mathf.Clamp(pos.x, -65, 34);
        //     transform.position = pos;
        // }

        private void HandleMovementLogic()
        {
            _horizontalVel = Input.GetAxisRaw("Horizontal") * MoveSpeed;

            var localScale = transform.localScale;
            if (_horizontalVel < 0 && !_horizontalFlip)
            {
                _horizontalFlip = true;
                localScale.x *= -1;
            } 
            else if (_horizontalVel > 0 && _horizontalFlip)
            {
                _horizontalFlip = false;
                localScale.x *= -1;
            }
            transform.localScale = localScale;
        }

        private void HandleJumpLogic()
        {
            if (!Grounded && _groundedLastFrame && !_jumping)
            {
                _coyoteTimerEnd = Time.time + CoyoteTimer;
            }

            if (Grounded && !_groundedLastFrame)
                _airJumpCount = 0;

            var groundOrCoyote = Grounded || Time.time <= _coyoteTimerEnd;
            var isDoubleJump = !Grounded && _canDoubleJump && _airJumpCount < 1;
            if ((groundOrCoyote || isDoubleJump) && Input.GetKeyDown(KeyCode.Space))
            {
                _jumping = true;
                _currentJumpForce = JumpForce;
                _jumpEnd = Time.time + MaxJumpDuration;

                var currentVelocity = Rigidbody.velocity;
                currentVelocity.y = 0;
                Rigidbody.velocity = currentVelocity;

                if (!groundOrCoyote)
                {
                    AudioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Length)]);
                    
                    _airJumpCount++;
                    var effect = Instantiate(JumpEffect, transform.position - transform.up, Quaternion.identity);
                    Destroy(effect, 0.5f);
                }
            }
            
            if (!Grounded && _canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                _jumping = true;
            }
        
            if (_jumping && Input.GetKeyUp(KeyCode.Space) || Time.time >= _jumpEnd)
                _jumping = false;

            _groundedLastFrame = Grounded;
        }

        private void HandleFlipLogic()
        {
            if (!_canGravityFlip)
                return;

            if (Input.GetKeyDown(KeyCode.LeftShift) && _flipCount < 2)
            {
                Rigidbody.gravityScale *= -1;
                _flipCount++;
                transform.Rotate(0, 0, 180);
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
                
                if (Rigidbody.gravityScale < 0)
                    AudioSource.PlayOneShot(FlipUpSound);
                else if (Rigidbody.gravityScale > 0)
                    AudioSource.PlayOneShot(FlipDownSound);
            }
        }

        private void FixedUpdate()
        {
            if (DialogManager.Instance.DialogActive)
            {
                Rigidbody.velocity = Vector2.zero;
                return;
            }
                
            
            var vel = Rigidbody.velocity;
            vel.x = _horizontalVel * Time.fixedDeltaTime;
            Rigidbody.velocity = vel;

            if (_jumping)
            {
                Rigidbody.AddForce(transform.up * _currentJumpForce, ForceMode2D.Force);
                _currentJumpForce -= (JumpForce / MaxJumpDuration) * Time.fixedDeltaTime;
            }

            Grounded = false;

            var groundCollider = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
            if (groundCollider)
            {
                Grounded = true;
                _flipCount = 0;
            }
            
            var pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -65, 34);
            transform.position = pos;
        }
    }
}
