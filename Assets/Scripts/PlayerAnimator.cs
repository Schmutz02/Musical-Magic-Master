using System;
using UnityEngine;

namespace MMM
{
    public class PlayerAnimator : MonoBehaviour
    {
        public Animator Animator;

        public PlayerMovement Movement;

        private Rigidbody2D _rigidbody;
        
        private static readonly int XVelocity = Animator.StringToHash("XVelocity");
        private static readonly int YVelocity = Animator.StringToHash("YVelocity");

        private void Start()
        {
            _rigidbody = Movement.Rigidbody;
        }

        private void Update()
        {
            var xVel = (int) _rigidbody.velocity.x;
            Animator.SetInteger(XVelocity, xVel);

            var yVel = _rigidbody.velocity.y;
            if (yVel > 0.01)
                Animator.SetInteger(YVelocity, 1);
            else if (yVel < -0.01)
                Animator.SetInteger(YVelocity, -1);
            
            if (Movement.Grounded)
                Animator.SetInteger(YVelocity, 0);
        }
    }
}