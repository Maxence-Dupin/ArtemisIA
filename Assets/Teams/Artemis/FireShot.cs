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

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _spaceship = GameManager.Instance.GetSpaceShipForController(_artemisController).view;
            _artemisController.shootForward = true;
        }
        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_spaceship.Energy < _spaceship.ShootEnergyCost) return TaskStatus.Failure;
            
            _artemisController.shootForward = false;
            
            return TaskStatus.Success;
        }
    }
}