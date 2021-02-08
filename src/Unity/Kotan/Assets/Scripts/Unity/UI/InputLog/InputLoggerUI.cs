using System;
using UnityEngine;
using UnityEngine.UI;

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
            CreateElement(data.LastEntry); //TODO - reuse instead of destroy
            CheckSize();
        }

        private void CreateElement(IInputLogEntry entry)
        {
            var newElement = Instantiate(elementPrefab, elementsRoot);
            newElement.gameObject.SetActive(true);
            newElement.name = Time.frameCount.ToString();
            newElement.SetData(entry);
        }

        private void CheckSize()
        {
            Canvas.ForceUpdateCanvases();
            if (elementsRoot.childCount > 0 && elementsRoot.GetComponent<RectTransform>().rect.width > gameObject.GetComponent<RectTransform>().rect.width)
            {
                Destroy(elementsRoot.GetChild(0).gameObject);
                Canvas.ForceUpdateCanvases();
            }
        }
    }
}