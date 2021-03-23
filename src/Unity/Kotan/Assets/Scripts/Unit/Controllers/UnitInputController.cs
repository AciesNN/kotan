using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitInputController : MonoBehaviour
    {
        [SerializeField] protected Unit unit;

        protected UnitChangeStateLogicFactory stateLogicFactory = new UnitChangeStateLogicFactory();
    }
}