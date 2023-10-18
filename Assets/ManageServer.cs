using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class ManageServer : MonoBehaviour
{
    int userId;
    int sessionId;

    private void OnEnable()
    {
        Simulator.OnNewPlayer += ServerAddPlayer;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= ServerAddPlayer;
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
        userId = ReadUserId();
        StartCoroutine(UpdateSession(dateTime, true));
    }

    void ServerEndSession(DateTime dateTime)
    {
        StartCoroutine(UpdateSession(dateTime, false));
    }

    void ServerBuyItem(DateTime dateTime, int itemId)
    {
        StartCoroutine(UploadPurchase(dateTime, itemId));
    }

    IEnumerator UploadPlayer(string name, string country, DateTime dateTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("country", country);
        form.AddField("date", dateTime.ToString());

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
        }

        // server return user Id
        if (www.downloadHandler.text == "nullData")
        {

        }
        else
        {
            SaveUserId(int.Parse(www.downloadHandler.text));
        }
    }

    IEnumerator UpdateSession(DateTime dateTime, bool startSession)
    {
        string sessionId;
        if (startSession)
        {
            userId = ReadUserId();
            sessionId = "startSession";
        }
        else
        {
            sessionId = this.sessionId.ToString();
        }

        WWWForm form = new WWWForm();
        form.AddField("sessionId", sessionId);
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
                SaveSessionId(int.Parse(www.downloadHandler.text));
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
        }
    }

    void SaveUserId(int userId)
    {
        this.userId = userId;
    }

    int ReadUserId()
    {
        // read form txt
        return 0;
    }


    void SaveSessionId(int sessionId)
    {
        this.sessionId = sessionId;
    }
}
