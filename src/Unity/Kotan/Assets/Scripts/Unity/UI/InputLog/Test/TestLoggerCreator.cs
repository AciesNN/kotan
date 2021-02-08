using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace UI.Test
{
    public class TestLoggerCreator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var controller = GetComponent<IPlayerInputController>();
            var logger = new InputLogOutput(controller);
            GetComponent<InputLoggerUI>().SetData(logger);
        }
    }
}