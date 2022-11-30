using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class LeaderboardGameServices : MonoBehaviour
{

    public int score;
    //public bool loginsuccess;
    public GameObject LeaderboardButton;

    // Start is called before the first frame update
    void Start()
    {
        GleyNotifications.Initialize();
        //this method will be called after login process is done private void LoginComplete (bool success 
        //Check if the device cannot reach the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Internet: non raggiungibile");
        }
        else { 
            GameServices.Instance.LogIn(LoginResult);
            Debug.Log("Internet: raggiungibile");
        }  
    }

    private void LoginResult(bool success)
    {
        if (success == true)
        {
            // Login was successful
            Debug.Log("Login success: " + success);
            LeaderboardButton.SetActive(true);
            //loginsuccess = true;
        }
        else
        {
            //Login failed
            Debug.Log("Login failed: " + success);
            LeaderboardButton.SetActive(false);
            //loginsuccess = false;
        }
    }

    // Update is called once per frame

    /* void Update()
    {
        if (loginsuccess == true)
        {
            LeaderboardButton.SetActive(true);
        }
        else
        {
            LeaderboardButton.SetActive(false);
        }
        //score = (int)Variables.Saved.Get("Points");
        //Debug.Log("score is " + score);
        //GameServices.Instance.SubmitScore(score, LeaderboardNames.Leaderboard);
    }*/

    public void ButtonShowLead()
    {
        score = (int)Variables.Saved.Get("Points");
        Debug.Log("score is " + score);
        Debug.Log("Button cliked");
        GameServices.Instance.SubmitScore(score, LeaderboardNames.Leaderboard);
        GameServices.Instance.ShowSpecificLeaderboard(LeaderboardNames.Leaderboard);
    }

    /// The best way to schedule notifications is from OnApplicationFocus method
    /// when this is called user left your app
    /// when you trigger notifications when user is still in app, maybe your notification will be delivered when user is still inside the app and that is not good practice  
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            //if user left your app schedule all your notifications
            //GleyNotifications.SendNotification("Soft Skills Training App", "It's been a while since you opened the app. Keep training to improve your skills.", new System.TimeSpan(0, 1, 0), "icon_0", "icon_1", "Keep training to improve your skills!");
            GleyNotifications.SendNotification("Soft Skills Training App", "It's been a while since you opened the app. Keep training to improve your skills.", new System.TimeSpan(2, 0, 0), "icon_0", "icon_1");
            GleyNotifications.SendNotification("Soft Skills Training App", "We haven't seen you for a long time! Why don't you keep practicing to improve your skills?!", new System.TimeSpan(8, 0, 0), "icon_0", "icon_1");
            GleyNotifications.SendNotification("Soft Skills Training App", "Hey what happened to you? Don't feel like practicing your soft skills a little?", new System.TimeSpan(14, 0, 0), "icon_0", "icon_1");
        }
        else
        {
            //call initialize when user returns to your app to cancel all pending notifications
            GleyNotifications.Initialize();
        }
    }

}
