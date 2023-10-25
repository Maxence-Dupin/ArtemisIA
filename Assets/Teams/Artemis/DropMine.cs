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
        public SpaceShip _spaceship;

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_spaceship.Energy < _spaceship.MineEnergyCost) return TaskStatus.Failure;
                
            _spaceship.DropMine();
            return TaskStatus.Success;
        }
    }
}