using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class ManageServer : MonoBehaviour
{
    int userId;

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
            StartCoroutine(UploadPurchase(DateTime.Now, 0));
        }
    }

    void ServerAddPlayer(string name, string country, DateTime dateTime)
    {
        StartCoroutine(UploadPlayer(name, country, dateTime));
    }

    void ServerAddSession(DateTime dateTime)
    {
        userId = ReadUserId();
        StartCoroutine(UpdateSession(dateTime));
    }

    void ServerEndSession(DateTime dateTime)
    {
        StartCoroutine(UpdateSession(dateTime));
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

    IEnumerator UpdateSession(DateTime dateTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("date", dateTime.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://www.my-server.com/myapi", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator UploadPurchase(DateTime dateTime, int itemId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        //form.AddField("date", dateTime.ToString());
        //form.AddField("itemId", itemId);
        form.AddField("money", 100);

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
}
