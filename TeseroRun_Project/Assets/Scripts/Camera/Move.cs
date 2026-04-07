using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AEA
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f; // Player movement speed
        [SerializeField] private Transform _player;  // Reference to the player object
        [SerializeField] private Vector3 _cameraOffset = new Vector3(0, 0, -10); // Camera offset from the player
        [SerializeField] private float _smoothSpeed = 0.125f; // Smoothing factor for camera movement
        [SerializeField] private Vector2 _cameraMinBounds; // Minimum bounds for the camera
        [SerializeField] private Vector2 _cameraMaxBounds; // Maximum bounds for the camera

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            if (_player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    _player = playerObject.transform;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            if (_player != null)
            {
                FollowPlayer();
            }
        }

        // Handle player movement
        private void MovePlayer()
        {
            float moveX = 0;

            if (Input.GetKey(KeyCode.A) && transform.position.x > 0f)
            {
                moveX = -_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D) && transform.position.x < 36f)
            {
                moveX = _speed * Time.deltaTime;
            }

            transform.Translate(new Vector2(moveX, 0));
        }

        // Camera follows the player with smooth movement, constrained within bounds
        private void FollowPlayer()
        {
            // Calculate desired camera position (player's position with offset)
            Vector3 desiredPosition = _player.position + _cameraOffset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            // Clamp the camera's position within the level bounds
            float clampedX = Mathf.Clamp(smoothedPosition.x, _cameraMinBounds.x, _cameraMaxBounds.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, _cameraMinBounds.y, _cameraMaxBounds.y);

            // Update the camera's position with clamped values
            transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
        }
    }
}
