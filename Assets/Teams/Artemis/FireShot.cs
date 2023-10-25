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
        public SpaceShip _spaceship;

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_spaceship.Energy < _spaceship.ShootEnergyCost) return TaskStatus.Failure;
                
            _spaceship.Shoot();
            return TaskStatus.Success;
        }
    }
}