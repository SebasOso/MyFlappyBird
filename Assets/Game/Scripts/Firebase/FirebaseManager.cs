using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance {  get; private set; }

    [Header("User Save")]
    [SerializeField] public bool logged = false;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference database;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        //Check that all necesary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are available Initilize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    private void Start()
    {
        if(user == null)
        {
            Debug.Log("User is null");
        }
        logged = PlayerPrefs.GetInt("logged") == 1;
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void LogOutButton()
    {
        LogOut();
        StartCoroutine(StartLoginScreen());
    }

    //LOG COURUTINES
    public IEnumerator LoginAgain(TMP_Text confirmLoginText, TMP_Text warningLoginText)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(Login(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"), false, confirmLoginText, warningLoginText));
    }
    public IEnumerator Login(string _email, string _password, bool isFisrstTime, TMP_Text confirmLoginText, TMP_Text warningLoginText)
    {
        if (auth != null)
        {
            Debug.Log("Auth no es nulo");
        }
        else
        {
            Debug.Log("Auth es nulo");
        }
        var loginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            //If there are errors, handle them
            Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            user = loginTask.Result.User;
            Debug.LogFormat("User signed in succesfully: {0} ({1})", user.DisplayName, user.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged in!";

            if(isFisrstTime == true)
            {
                StartCoroutine(StartPoints());            }
            else
            {
                StartCoroutine(UpdatePoints(PlayerPrefs.GetInt("points")));
            }

            //Save player data
            PlayerPrefs.SetString("email", _email);
            PlayerPrefs.SetString("password", _password);
            PlayerPrefs.SetInt("logged", 1);

            StartCoroutine(StartMainMenu());
        }
    }
    private void LogOut()
    {
        if(auth != null && user != null)
        {
            auth.SignOut();
        }
    }
    public IEnumerator StartLoginScreen()
    {
        yield return new WaitForSeconds(1f);
        if(user == null)
        {
            Debug.Log("User is null");
        }
        SceneManager.LoadScene(0);
    }
    public IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    //REGISTER COURUTINES
    public IEnumerator Register(string _email, string _password, string _userName, TMP_Text warningRegisterText, TMP_InputField passwordRegisterField, TMP_InputField passwordConfirmRegisterField, TMP_Text confirmLoginText, TMP_Text warningLoginText, GameObject checkRegister, GameObject warningRegister)
    {
        if(database == null)
        {
            Debug.Log("db is null");
        }
        if (_userName == "")
        {
            checkRegister.SetActive(false);
            warningRegister.SetActive(true);
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordConfirmRegisterField.text)
        {
            checkRegister.SetActive(false);
            warningRegister.SetActive(true);
            warningRegisterText.text = "Passwords does not match!";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);
            if (registerTask.Exception != null)
            {
                //If there are errors, handle them
                warningRegister.SetActive(true);
                checkRegister.SetActive(false);
                Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Paswword";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                checkRegister.SetActive(false);
                warningRegister.SetActive(true);
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get result
                checkRegister.SetActive(true);
                user = registerTask.Result.User;
                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _userName };
                    var profileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);
                    if (profileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                        FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        
                        warningRegisterText.text = "";

                        PlayerPrefs.SetInt("points", 0);
                        PlayerPrefs.SetString("username", _userName);

                        StartCoroutine(UpdateUsernameDatabase(_userName));
                        StartCoroutine(LoginAfterRegister(_email, _password, confirmLoginText, warningLoginText));
                    }
                }
            }
        }
    }

    public IEnumerator LoginAfterRegister(string email, string password, TMP_Text confirmLoginText, TMP_Text warningLoginText)
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(Login(email, password, true, confirmLoginText, warningLoginText));
    }

    //UPDATE USERNAME, START POINTS, START USERNAME
    private IEnumerator UpdateUsernameAuth(string username)
    {
        if (user == null)
        {
            Debug.Log("user is null");
        }
        UserProfile userProfile = new UserProfile { DisplayName= username };
        var ProfileTask = user.UpdateUserProfileAsync(userProfile);

        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if(ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth Username is updated
        }
    }
    private IEnumerator UpdateUsernameDatabase(string username)
    {
        if(user == null)
        {
            Debug.Log("user is null");
        }
        var DBTask = database.Child("users").Child(user.UserId).Child("username").SetValueAsync(username);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is updated
        }
    }
    private IEnumerator StartPoints()
    {
        if (user == null)
        {
            Debug.Log("user is null");
        }
        var DBTask = database.Child("users").Child(user.UserId).Child("points").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database points are updated
        }
    }

    //UPDATE POINTS WHEN PLAYING
    private IEnumerator UpdatePoints(int points)
    {
        //Taking data from the actual user
        Task<DataSnapshot> DBTask = database.Child("users").Child(user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            int currentPoints = int.Parse(snapshot.Child("points").Value.ToString());
            if(currentPoints < points)
            {
                PlayerPrefs.SetInt("points", points);
                database.Child("users").Child(user.UserId).Child("points").SetValueAsync(points);
            }
        }

    }
    public void UpdateNewPoints(int points)
    {
        StartCoroutine(UpdatePoints(points));
    }

    //LOAD SCORE BOARD
    public void LoadScoreBoard(Transform scoreboardContent, GameObject scoreElement)
    {
        StartCoroutine(LoadScoreboardData(scoreboardContent, scoreElement));
    }
    private IEnumerator LoadScoreboardData(Transform scoreboardContent , GameObject scoreElement)
    {
        //Get all the users data ordered by points amount
        Task<DataSnapshot> DBTask = database.Child("users").OrderByChild("points").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int points = int.Parse(childSnapshot.Child("points").Value.ToString());
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, points);
            }
        }
    }
}