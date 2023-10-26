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
        public ArtemisController _artemisController;
        public SpaceShip _spaceship;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _artemisController.shootForward = true;
        }
        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_spaceship.Energy < _spaceship.ShootEnergyCost) return TaskStatus.Failure;
            
            return TaskStatus.Success;
        }
    }
}