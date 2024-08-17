using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InfoStatus
{
    Error,
    Success,
    Default,
}

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private List<GameObject> notificationObj = new List<GameObject>();
    [SerializeField] private float notificationDuration = 3.0f;

    private static Notification instance;

    private void Start()
    {
        notificationPrefab = Resources.Load<GameObject>("PopUpInfo");
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SomeInformation(InfoStatus status, string info)
    {
        if (checkList())
        {
            var obj = Instantiate(notificationPrefab);
            notificationObj.Add(obj);

            switch (status)
            {
                case InfoStatus.Error:
                    setText(obj, info);
                    setColor(obj, Color.red);
                    break;
                case InfoStatus.Success:
                    setText(obj, info);
                    setColor(obj, Color.green);
                    break;
                case InfoStatus.Default:
                    setText(obj, info);
                    setColor(obj, Color.cyan);
                    break;
            }

            StartCoroutine(RemoveNotificationAfterTime(obj, notificationDuration));
        }
    }

    private void setText(GameObject notificationObj, string text)
    {
        notificationObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void setColor(GameObject notificationObj, Color color)
    {
        notificationObj.transform.GetChild(1).GetComponent<Image>().color = color;
    }

    private bool checkList()
    {
        return notificationObj.Count == 0;
    }

    private IEnumerator RemoveNotificationAfterTime(GameObject notification, float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationObj.Remove(notification);
        Destroy(notification);
    }
}
