using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private string webClientId;
    private GoogleSignInConfiguration _configuration;
    
    private FirebaseAuth _auth;
    
    private void Awake()
    {
        _configuration = new GoogleSignInConfiguration
            {WebClientId = webClientId, RequestEmail = true, RequestIdToken = true};
        
        _auth = FirebaseAuth.DefaultInstance;
    }

    private void MoveLobbyScene()
    {
        SaveManager.Instance.OnLoadPlayerCompleted -= MoveLobbyScene;
        Debug.Log("로비로 이동 합니다.");
        SceneManager.LoadScene("Lobby");
    }

    #region Google Login
    
    public void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    public void SignOutFromGoogle()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }
    
    public void OnDisconnect()
    {
        GoogleSignIn.DefaultInstance.Disconnect();
    }
    
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            if (task.Exception == null) return;
            
            using var enumerator = task.Exception.InnerExceptions.GetEnumerator();
            var error = (GoogleSignIn.SignInException) enumerator.Current;
            
            if (enumerator.MoveNext())
            {
                if (error != null)
                {
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
            }
            else
            {
                Debug.Log("Got Unexpected Exception?!?" + task.Exception);
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        var credential = GoogleAuthProvider.GetCredential(idToken, null);
        
        _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            var ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
                }
            }
            else
            {
                PlayerData.Instance.userId = _auth.CurrentUser.UserId;
            }
            
            SaveManager.Instance.OnLoadPlayerCompleted += MoveLobbyScene;
            SaveManager.Instance.Init();
        });
    }
    
    #endregion
    
}
