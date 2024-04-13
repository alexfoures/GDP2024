using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDP2024
{
    public class PowerUpManagement : MonoBehaviour
    {
        public enum PowerUp {
            None,
            Resize,
            DoubleJump,
            WallJump,
            Drill
        }

        [SerializeField] public PowerUp powerUp;

        private PowerUp _currentPower;
        private bool _isDoubleJump;
        private bool _isWallJump;

        public bool IsDoubleJump => _isDoubleJump;
        public bool IsWallJump => _isWallJump;
        // Start is called before the first frame update
        void Start()
        {
            SetPowerUp(powerUp);
        }

        void SetPowerUp(PowerUp up)
        {
            SetUpSwitch(_currentPower, false);
            if (up != PowerUp.None)
            {
                SetUpSwitch(up, true);
            }
        }

        private void SetUpSwitch(PowerUp up, bool enable)
        {
            switch (up)
            {
                case PowerUp.Resize:
                    Resize(enable);
                    break;
                case PowerUp.DoubleJump:
                    EnableDoubleJump(enable);
                    break;
                case PowerUp.WallJump:
                    EnableWallJump(enable);
                    break;
                case PowerUp.Drill:
                    break;
                default:
                    break;
            }
        }

        private void Resize(bool resize)
        {
            var vector = resize ? new Vector3(0.5f, 0.5f, 0.5f) : new Vector3(1f, 1f, 1f);
            GetComponent<Transform>().localScale = vector;
        }

        private void EnableDoubleJump(bool enable)
        {          
            _isDoubleJump = enable;
        }

        private void EnableWallJump(bool enable)
        {
            _isWallJump = enable;
        }

        // Update is called once per frame
        void Update()
        {
            if (powerUp != _currentPower) {
                SetPowerUp(powerUp);
                _currentPower = powerUp;
            }
        }
    }
}
