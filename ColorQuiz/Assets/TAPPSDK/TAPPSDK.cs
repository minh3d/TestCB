using UnityEngine;
using System.Collections;
using DG.Tweening;
using UniRx;
using System.Security.Cryptography;
using System;
using System.Text;
using UnityEngine.UI;

public class TAPPSDK : MonoBehaviour {

    private string URLLOGIN = "http://tapp.vn/charge/code/login_mini.html";
    private string APPID = "4";
    private string APPSERECT = "5dafbie4qLX9dd5+AeZ8aB3fMXlR8SJabbeiMAM";


    public InputField usernameInput;
    public InputField passwordInput;

    public InputField username2Input;
    public InputField password2Input;
    public InputField password3Input;

    public GameObject MainPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject WelcomePanel;

    public GameObject GameController;
    public GameObject Loading;

    public Text UName;

    public GameObject FloatingButton;

    public GameObject webView;

   

    // Use this for initialization
    void Start () {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        //punch
        Debug.Log(PlayerPrefs.GetString("sid").Length);
        if (PlayerPrefs.GetString("sid") != null && PlayerPrefs.GetString("sid") != "")
        {
            //load userinfo
            loadInfo();
            //hide pannel
        } else
        {
            transform.DOPunchScale(Vector3.one/4, 1, 7, 0.2f);
        }
    }
    void loadInfo()
    {
        WelcomePanel.SetActive(true);
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        MainPanel.SetActive(false);

        Loading.SetActive(true);
        var form = new WWWForm(); //here you create a new form connection
        form.AddField("token", PlayerPrefs.GetString("sid"));
        form.AddField("appid", APPID);
        form.AddField("sign", md5(PlayerPrefs.GetString("sid") + APPSERECT));

        //send
        ObservableWWW.Post("http://tapp.vn/charge/code/authen_mini.html", form).TakeUntilDestroy(this).Subscribe(
            x => {
                Debug.Log(x);
                JSONObject obj = new JSONObject(x);
                UName.text = obj["data"]["username"].str;
                FloatingButton.SetActive(true);
                Loading.SetActive(false);

                //GameController.GetComponent<Done_GameController>().enabled = true;
                gameObject.SetActive(false);
            }, // onSuccess
            ex =>
            {
                MobileNativeMessage msg = new MobileNativeMessage("TAPP.vn", "Có lỗi xảy ra");
            }// onError
        );
    }

    public void LoginAsGuest()
    {
        var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        string time = (System.DateTime.UtcNow - epochStart).TotalSeconds.ToString();

        var form = new WWWForm(); //here you create a new form connection
        form.AddField("type", "guess");
        form.AddField("platform", "android");
        form.AddField("username", "ad" + time);
        form.AddField("password", "");
        form.AddField("appid", APPID);
        form.AddField("time", time);
        form.AddField("sign", md5(time + APPID + APPSERECT));

        Loading.SetActive(true);

        //send
        ObservableWWW.Post(URLLOGIN, form).Subscribe(
            x => {
                Debug.Log(x);
                JSONObject obj = new JSONObject(x);
                PlayerPrefs.SetString("sid", (obj["token"].str));
                loadInfo();
                Loading.SetActive(false);

            }, // onSuccess
            ex =>
            {
                MobileNativeMessage msg = new MobileNativeMessage("TAPP.vn", ex.ToString());
            }// onError
        );
    }
    public void LoginButton()
    {
        MainPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void RegisterButton()
    {
        MainPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void Register()
    {
        if (password2Input.text != password3Input.text)
        {
            MobileNativeMessage msg = new MobileNativeMessage("TAPP.vn", "Hai mật khẩu không giống nhau");
            return;
        }
        double CurrentTimestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        int timestamp = Mathf.RoundToInt((float)CurrentTimestamp);
        var form = new WWWForm(); //here you create a new form connection
        form.AddField("type", "register");
        form.AddField("platform", "android");
        form.AddField("username", username2Input.text);
        form.AddField("password", password2Input.text);
        form.AddField("appid", APPID);
        form.AddField("time", timestamp.ToString());
        form.AddField("sign", md5(timestamp.ToString() + APPID + APPSERECT));

        //send
        Loading.SetActive(true);
        ObservableWWW.Post(URLLOGIN, form).Subscribe(
            x =>
            {
                Debug.Log(x);
                JSONObject obj = new JSONObject(x);
                if (obj["status"].i <= 0)
                {
                    MobileNativeMessage msg = new MobileNativeMessage("TAPP.vn", "Thông tin đăng nhập không đúng");
                }
                else
                {
                    PlayerPrefs.SetString("sid", (obj["token"].str));
                    loadInfo();
                }
                Loading.SetActive(false);

            }, // onSuccess
            ex => Debug.LogException(ex) // onError
        );
    }

    public void Login()
    {
        double CurrentTimestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        int timestamp = Mathf.RoundToInt((float)CurrentTimestamp);
        var form = new WWWForm(); //here you create a new form connection
        form.AddField("type", "login");
        form.AddField("platform", "android");
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("appid", APPID);
        form.AddField("time", timestamp.ToString());
        form.AddField("sign", md5(timestamp.ToString() + APPID + APPSERECT));

        //send
        Loading.SetActive(true);
        ObservableWWW.Post(URLLOGIN, form).Subscribe(
            x =>
            {
                Debug.Log(x);
                JSONObject obj = new JSONObject(x);
                if (obj["status"].i <= 0)
                {
                    MobileNativeMessage msg = new MobileNativeMessage("TAPP.vn", "Thông tin đăng nhập không đúng");
                }
                else
                {
                    PlayerPrefs.SetString("sid", (obj["token"].str));
                    PlayerPrefs.SetString("uid", (obj["userid"].str));
                    loadInfo();
                }
                Loading.SetActive(false);

            }, // onSuccess
            ex => Debug.LogException(ex) // onError
        );
    }


    public string md5(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("sid");
        Application.LoadLevel(Application.loadedLevel);
    }

    public void close()
    {
        Application.Quit();
    }

    public void back()
    {
        MainPanel.SetActive(true);
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
    }

    public void UpdateInfo()
    {
        Destroy(webView.GetComponent<UniWebView>());
        UniWebView wv = webView.AddComponent<UniWebView>();
        wv.Show();
        wv.url = "http://tapp.vn/updateinfo?userid="+PlayerPrefs.GetString("uid")+ "&token=" + PlayerPrefs.GetString("sid") + "&sign=" + md5(APPSERECT + PlayerPrefs.GetString("sid"));
        wv.Load();
    }

    public void Events()
    {
        Destroy(webView.GetComponent<UniWebView>());
        UniWebView wv = webView.AddComponent<UniWebView>();
        wv.Show();
        wv.url = "http://tapp.vn/event";
        wv.Load();
    }

    public void Games()
    {
        Destroy(webView.GetComponent<UniWebView>());
        UniWebView wv = webView.AddComponent<UniWebView>();
        wv.Show();
        wv.url = "http://tapp.vn/game";
        wv.Load();
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("sid");
        Application.LoadLevel(0);
        gameObject.SetActive(true);
        WelcomePanel.SetActive(false);
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        MainPanel.SetActive(true);
        FloatingButton.SetActive(false);
    }
}
