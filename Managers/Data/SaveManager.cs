using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private FirebaseFirestore _db;
    private Dictionary<string, object> _documentDic;

    public delegate void LoadPlayerHandler();
    public event LoadPlayerHandler OnLoadPlayerCompleted;
    
    private bool _isExistPlayer;
    public void Init()
    {
        _db = FirebaseFirestore.DefaultInstance;
        _documentDic = new Dictionary<string, object>();
        _isExistPlayer = false;

        CheckUserId();
    }

    private void CheckUserId()
    {
        var playersRef = _db.Collection("Players");
        playersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            foreach (var document in task.Result.Documents)
            {
                if (PlayerData.Instance.userId != document.Id)
                {
                    continue;
                }

                Debug.Log("존재하는 아이디가 있어서 정보를 불러 옵니다.");
                _documentDic = document.ToDictionary();


                _isExistPlayer = true;
            }
            LoadPlayer();
        });
    }

    private void LoadPlayer()
    {
        Debug.Log("LoadPlayer 실행");
        if (_isExistPlayer)
        {
            ReadPlayerData();
        }
        else
        {
            SetFirstPlayerInfo();
        }
        
    }

    private void SetFirstPlayerInfo()
    {
        Debug.Log("캐릭터를 생성 합니다.");
        PlayerData.Instance.PlayerDataInit();
        WritePlayerData();
    }

    private void ReadPlayerData()
    {
        Debug.Log("정보 읽는 중!");
        PlayerData.Instance.SetPlayerData(_documentDic);
        OnLoadPlayerCompleted?.Invoke();
    }

    public void WritePlayerData()
    {
        var playerRef = _db.Collection("Players").Document(PlayerData.Instance.userId);
        var player = PlayerData.Instance.GetPlayerData();
        
        playerRef.SetAsync(player).ContinueWithOnMainThread(task =>
        {
            OnLoadPlayerCompleted?.Invoke();
        });
    }
}