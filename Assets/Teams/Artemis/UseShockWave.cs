using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class UseShockWave : Action
    {
        private ArtemisController _artemisController;
        private SpaceShipView _spaceShipView;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _spaceShipView = GameManager.Instance.GetSpaceShipForController(_artemisController).view;
        }

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (!(_spaceShipView.Energy >= _spaceShipView.ShockwaveEnergyCost)) return TaskStatus.Failure;

            _artemisController.UseShockWave = true;
            return TaskStatus.Success;
        }
    }
}