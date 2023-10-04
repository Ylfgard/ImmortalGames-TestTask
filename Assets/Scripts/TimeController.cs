using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

public class TimeController : MonoBehaviour
{
    public const string MoscowTimeURL = "http://worldtimeapi.org/api/timezone/Europe/Moscow";
    
    [DllImport("__Internal")]    
    private static extern void ShowTime(string time);

    public void GetMoscowTime()
    {
        StartCoroutine(FetchTimeFromServer(MoscowTimeURL));
    }

    private IEnumerator FetchTimeFromServer(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                ShowTime(ParseTimeFromJson(json));
            }
        }
    }

    private string ParseTimeFromJson(string json)
    {
        WorldTimeData timeData = JsonUtility.FromJson<WorldTimeData>(json);
        StringBuilder time = new StringBuilder(timeData.datetime, 11, 8, 8);
        return time.ToString();
    }
}
