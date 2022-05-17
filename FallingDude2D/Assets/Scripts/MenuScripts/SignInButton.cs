using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInButton : MonoBehaviour
{
    private void Start()
    {
        var signInButton = GetComponent<Button>();
        var authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        signInButton.onClick.AddListener(delegate { authManager.SignInButton(); });
    }
}
