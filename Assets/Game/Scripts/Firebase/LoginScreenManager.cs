using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginScreenManager : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text confirmLoginText;
    public TMP_Text warningLoginText;

    [Header("Register")]
    public TMP_InputField userRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordConfirmRegisterField;
    public TMP_Text warningRegisterText;
    public GameObject warning;
    public GameObject checkRegister;

    [Header("Forget Password")]
    public TMP_InputField forgetPasswordEmail;
    public TMP_Text forgetPasswordWarning;
    public void LoginButtom()
    {
        StartCoroutine(FirebaseManager.Instance.Login(
            emailLoginField.text, 
            passwordLoginField.text, 
            false, 
            confirmLoginText, 
            warningLoginText));
    }
    public void RegisterButtom()
    {
        StartCoroutine(FirebaseManager.Instance.Register(
            emailRegisterField.text, 
            passwordRegisterField.text, 
            userRegisterField.text, 
            warningRegisterText, 
            passwordRegisterField, 
            passwordConfirmRegisterField,
            confirmLoginText,
            warningLoginText,
            checkRegister,
            warning));
    }
    public void ForgetPasswordButton()
    {
        FirebaseManager.Instance.ForgetPasswordSubmit(forgetPasswordEmail.text, forgetPasswordWarning);
    }

}
