using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class FireShot : Action
    {
        private ArtemisController _artemisController;
        private SpaceShipView _spaceship;

        private bool _success;
        private bool _coroutineRunning;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _spaceship = GameManager.Instance.GetSpaceShipForController(_artemisController).view;
        }
        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_success)
            {
                _success = false;
                return TaskStatus.Success;
            }

            if (_spaceship.Energy < _spaceship.ShootEnergyCost && !_coroutineRunning) return TaskStatus.Failure;

            if (_coroutineRunning) return TaskStatus.Running;
            
            _artemisController.shootForward = true;
            StartCoroutine(WaitForSuccess());

            return TaskStatus.Running;
            
            IEnumerator WaitForSuccess()
            {
                _coroutineRunning = true;
                _artemisController.recentShot = true;
                yield return new WaitForSeconds(0.2f);
                _coroutineRunning = false;
                _artemisController.recentShot = false;
                _success = true;
            }
        }
    }
}