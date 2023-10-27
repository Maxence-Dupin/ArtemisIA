using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class DropMine : Action
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

            if (_spaceship.Energy < _spaceship.MineEnergyCost) return TaskStatus.Failure;

            if (_artemisController.DistanceWithEnemySpaceship() > 5 &&
                _artemisController.DistanceWithClosestWaypoint() < 1 && _spaceship.Energy > 0.6)
            {
                _artemisController.dropMine = true;
            }
            return TaskStatus.Success;
        }
    }
}