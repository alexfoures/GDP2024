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
        private bool _isDrill;
        private Vector3 _baseLocalScale;

        public bool IsDoubleJump => _isDoubleJump;
        public bool IsWallJump => _isWallJump;
        public bool IsDrill => _isDrill;
        // Start is called before the first frame update

        private SoundManager _soundManager;

        void Start()
        {
            _baseLocalScale = GetComponent<Transform>().localScale;
            _soundManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
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
                    EnableDrill(enable);
                    break;
                default:
                    break;
            }
            if(UnityEngine.Random.Range(0, 1)==0)
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>().PlayVoice(SoundManager.Voices.Upscale);
            else
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>().PlayVoice(SoundManager.Voices.Downscale);

        }


        private void Resize(bool resize)
        {          
            var vector = resize ? new Vector3(_baseLocalScale.x/2, _baseLocalScale.y/2, _baseLocalScale.z/2) :
                new Vector3(_baseLocalScale.x, _baseLocalScale.y, _baseLocalScale.z);
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

        private void EnableDrill(bool enable)
        {
            _isDrill = enable;            
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
