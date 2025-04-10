using System;
using _Assets.Scripts.InputLogic;
using _Assets.Scripts.Utils;
using UnityEngine;

namespace _Assets.Scripts.HeroLogic.Movement
{
    public class HeroMovement : MonoBehaviour, IDisposable
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        
        private float _moveSpeed;
        private IInputService _inputService;
        private Vector2 _mapSize;

        public void Initialize(float moveSpeed, IInputService inputService, Vector2 mapSize)
        {
            _moveSpeed = moveSpeed;
            _inputService = inputService;
            _mapSize = mapSize;
            rigidbody2d.linearVelocity = Vector2.zero;

            _inputService.MoveInput += Move;
        }

        public void Dispose() =>
            OnDestroy();

        private void OnDestroy() =>
            _inputService.MoveInput -= Move;

        private void Move(Vector2 input)
        {
            if (input.magnitude > 0.01f)
            {
                var normalizedInput = input.normalized;
                Rotate(normalizedInput);
                
                Vector2 currentPosition = transform.position;
                var intendedPosition = currentPosition + normalizedInput * _moveSpeed * Time.fixedDeltaTime;

                if (MovementUtil.IsInsideBoundary(intendedPosition, _mapSize))
                    rigidbody2d.MovePosition(intendedPosition);
                else
                {
                    var clampedPosition = MovementUtil.ClampPositionToBoundary(intendedPosition, _mapSize);
                    rigidbody2d.MovePosition(clampedPosition);

                    if (clampedPosition.magnitude > 0.01f)
                        rigidbody2d.MovePosition(clampedPosition);
                    else
                        rigidbody2d.linearVelocity = Vector2.zero;
                }
            }
            else
                rigidbody2d.linearVelocity = Vector2.zero;
        }

        private void Rotate(Vector2 input)
        {
            var targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            
            var targetRotation = Quaternion.Euler(0f, 0f, -targetAngle);
            transform.rotation = targetRotation;
        }
    }
}