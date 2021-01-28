using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InputLoggerEntryElement : MonoBehaviour
    {
        [Serializable]
        public class InputLoggerEntryElementActionSpriteSetting
        {
            [SerializeField] public InputLogAction action;
            [SerializeField] public Sprite sprite;
        }

        [SerializeField] Sprite arrowN;
        [SerializeField] Sprite arrow6;
        [SerializeField] List<InputLoggerEntryElementActionSpriteSetting> actionSpites;

        [SerializeField] Image arrow;
        [SerializeField] Image actionPrefab;

        Dictionary<InputLogAction, Sprite> actionsDict = new Dictionary<InputLogAction, Sprite>();
        List<Image> actions = new List<Image>();

        IInputLogEntry data;

        private void Start()
        {
            foreach (var item in actionSpites)
                actionsDict[item.action] = item.sprite;
        }

        public void SetData(IInputLogEntry data)
        { 
            if (this.data != null)
            {
                this.data.OnChanged -= Data_OnChanged;
            }

            this.data = data;

            if (this.data != null)
            {
                this.data.OnChanged += Data_OnChanged;
            }

            ShowEntry();
        }

        private void Data_OnChanged()
        {
            ShowEntry();
        }

        public void ShowEntry()
        {
            if (data.Dir.x == 0 && data.Dir.y == 0)
            {
                arrow.sprite = arrowN;
                arrow.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                arrow.sprite = arrow6;
                arrow.transform.localEulerAngles = Vector3.forward * GetArrowAngle(data.Dir);
            }

            for (int i = actions.Count - 1; i >= data.States.Count; i--)
            {
                Destroy(actions[i].gameObject);
                actions.RemoveAt(i);
            }

            for (int i = actions.Count; i < data.States.Count; i++)
            {
                var newActon = GameObject.Instantiate(actionPrefab, actionPrefab.transform.parent);
                newActon.gameObject.SetActive(true);
                actions.Add(newActon);
            }

            for (int i = 0; i < data.States.Count; i++)
            {
                actions[i].sprite = actionsDict[data.States[i]];
            }
        }

        int GetArrowAngle(Vector2Int dir)
        {
            if (dir.y != 0)
            {
                return Mathf.RoundToInt(Mathf.Sign(dir.y)) * 45 
                    * (dir.x == 0 ? 2
                    : dir.x < 0 ? 3 : 1);
            }

            return dir.x < 0 ? 180 : 0;
        }

        private void OnDestroy()
        {
            if (this.data != null)
            {
                this.data.OnChanged -= Data_OnChanged;
            }
        }
    }
}