using System;
using UnityEngine;

namespace UI.Test
{
    public class TestLoggerCreator : MonoBehaviour
    {
        void Start()
        {
            var controller = FindObjectOfIType<IBufferedInputController>();
            var logger = new InputLoggerModel(controller);
            GetComponent<InputLogger>().SetData(logger);
        }

        public static T FindObjectOfIType<T>() where T: class
        {
            MonoBehaviour[] sceneObjects = FindObjectsOfType<MonoBehaviour>();

            for (int i = 0; i < sceneObjects.Length; i++)
            {
                MonoBehaviour currentObj = sceneObjects[i];
                var tComponent = currentObj.GetComponent<T>();

                if (tComponent != null)
                {
                    return tComponent;
                }
            }

            return null;
        }
    }
}