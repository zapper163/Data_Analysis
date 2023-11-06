using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class ManageServer : MonoBehaviour
{
    int userId = 0;
    int sessionId = 0;

    private void OnEnable()
    {
        Simulator.OnNewPlayer += ServerAddPlayer;
        Simulator.OnNewSession += ServerAddSession;
        Simulator.OnEndSession += ServerEndSession;
        Simulator.OnBuyItem += ServerBuyItem;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= ServerAddPlayer;
        Simulator.OnNewSession -= ServerAddSession;
        Simulator.OnEndSession -= ServerEndSession;
        Simulator.OnBuyItem -= ServerBuyItem;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(UploadPurchase(DateTime.Now, 4));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(UpdateSession(DateTime.Now, true));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(UpdateSession(DateTime.Now, false));
        }
    }

    void ServerAddPlayer(string name, string country, DateTime dateTime)
    {
        StartCoroutine(UploadPlayer(name, country, dateTime));
    }

    void ServerAddSession(DateTime dateTime)
    {
        StartCoroutine(UpdateSession(dateTime, true));
    }

    void ServerEndSession(DateTime dateTime)
    {
        StartCoroutine(UpdateSession(dateTime, false));
    }

    void ServerBuyItem(int itemId, DateTime dateTime)
    {
        StartCoroutine(UploadPurchase(dateTime, itemId));
    }

    IEnumerator UploadPlayer(string name, string country, DateTime dateTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", ++userId);
        form.AddField("name", name);
        form.AddField("country", country);
        form.AddField("date", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~rubenaa3/NewUserSim.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Form upload complete!");
            CallbackEvents.OnAddPlayerCallback.Invoke(0);
        }
    }

    IEnumerator UpdateSession(DateTime dateTime, bool startSession)
    {
        int sId;
        string start;
        if (startSession)
        {
            sId = ++sessionId;
            start = "true";
        }
        else
        {
            sId = sessionId;
            start = "false";
        }

        WWWForm form = new WWWForm();
        form.AddField("start", start);
        form.AddField("sessionId", sId);
        form.AddField("userId", userId);
        form.AddField("date", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~rubenaa3/SessionSim.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            if (startSession)
            {
                //SaveSessionId(int.Parse(www.downloadHandler.text));
                CallbackEvents.OnNewSessionCallback.Invoke(0);
            }
            else
            {
                CallbackEvents.OnEndSessionCallback.Invoke(0);
            }
        }
    }

    IEnumerator UploadPurchase(DateTime dateTime, int itemId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("itemId", itemId);

        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~rubenaa3/PurchaseSim.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            CallbackEvents.OnItemBuyCallback.Invoke();
        }
    }


    void SaveSessionId(int sessionId)
    {
        this.sessionId = sessionId;
    }
}
