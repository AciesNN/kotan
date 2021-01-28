using System;
using UnityEngine;

namespace UI
{
    public interface IInputLogOutput
    {
        IInputLogEntry LastEntry { get; }

        event Action OnEntryAdded;
    }

    public class InputLoggerUI : MonoBehaviour
    {
        [SerializeField] InputLoggerEntryElement elementPrefab;
        [SerializeField] Transform elementsRoot;

        IInputLogOutput data;

        public void SetData(IInputLogOutput data)
        {
            if (this.data != null)
            {
                this.data.OnEntryAdded -= Data_OnEntryAdded;
            }

            this.data = data;

            if (this.data != null)
            {
                this.data.OnEntryAdded += Data_OnEntryAdded;
            }

            DestroyElements();
        }

        private void DestroyElements()
        {
            for (int i = elementsRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(elementsRoot.GetChild(i).gameObject);
            }
        }

        void Data_OnEntryAdded()
        {
            CreateElement(data.LastEntry);
        }

        private void CreateElement(IInputLogEntry entry)
        {
            elementPrefab.SetData(entry);
        }
    }
}