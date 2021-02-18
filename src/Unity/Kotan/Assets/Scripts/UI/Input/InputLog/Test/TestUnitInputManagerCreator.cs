using System;
using UnityEngine;

namespace UI.Test
{
    public class TestUnitInputManagerCreator : MonoBehaviour
    {
        [SerializeField] GameObject unit;

        PlayerInputManager inputManager;

        void Start()
        {
            var controller = TestLoggerCreator.FindObjectOfIType<IPlayerInputController>();
            inputManager = new PlayerInputManager(controller, unit.GetComponent<IUnit>());
        }

        private void OnDestroy()
        {
            inputManager.Dispose();
        }
    }
}