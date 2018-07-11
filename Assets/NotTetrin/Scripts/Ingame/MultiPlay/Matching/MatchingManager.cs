using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Ingame.MultiPlay.Matching {
    public class MatchingManager : MonoBehaviour {
        [SerializeField]
        private GameObject matchingWindow;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;
        [SerializeField]
        private Button cancelButton;

        private bool quit = false;
        
        private string playerName => IdentificationNameUtility.ParseName(PhotonNetwork.player.NickName);

        private void Awake() {
            PhotonNetwork.automaticallySyncScene = true;
            Debug.Log(PhotonNetwork.connectionStateDetailed);

            if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
                connectToPhoton();
            }
            // PhotonNetwork.logLevel = PhotonLogLevel.Full;
        }

        private void Start() {
            matchingWindow.SetActive(true);
            matchingWindow.GetComponent<Animator>().Play(@"OpenWindow");
        }

        private void Update() {
            if (Input.GetButtonDown("Escape")) {
                CancelMatching();
            }
        }

        private void connectToPhoton() {
            Debug.Log("Connecting to Server...");
            statusLabel.text = @"接続中";
            PhotonNetwork.ConnectUsingSettings("1.0");
        }

        private IEnumerator requestJoinRandomRoom() {
            yield return new WaitUntil(() => PhotonNetwork.connectedAndReady);
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartMatching() {
            StartCoroutine(requestJoinRandomRoom());
        }

        public void CancelMatching() {
            quit = true;
            matchingWindow.GetComponent<Animator>().Play(@"CloseWindow");

            PhotonNetwork.Disconnect();
            SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
        }

        private void updateOtherPlayerStatus() {
            if (PhotonNetwork.otherPlayers.Length > 0) {
                StartCoroutine(successMatching());
            }
        }

        private IEnumerator successMatching() {
            Debug.Log(@"Found the other player. Will begin transit to network battle scene, Wait a moment...");
            messageLabel.text = @"対戦相手が見つかりました！";
            statusLabel.text = @"あいて: " + IdentificationNameUtility.ParseName(PhotonNetwork.otherPlayers[0].NickName);
            cancelButton.interactable = false;
            PhotonNetwork.room.IsOpen = false;
            yield return new WaitForSeconds(2.0f);

            SceneController.Instance.LoadScene(SceneName.NetworkBattle, 0.4f);
        }

        public void OnPhotonPlayerConnected(PhotonPlayer player) {
            Debug.Log($"Joined player(id: { player.ID }) in this room.");
            updateOtherPlayerStatus();
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            Debug.Log($"A player(id: { player.ID }) has left room.");
        }

        public void OnJoinedRoom() {
            Debug.Log("OnJoinedRoom");
            statusLabel.text = $"あなた: {playerName} (ホスト)";
            updateOtherPlayerStatus();
        }

        public void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");

            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var id = PhotonNetwork.AuthValues.UserId;
            PhotonNetwork.playerName = IdentificationNameUtility.Create(name, id);

            statusLabel.text = $"あなた: {playerName}";
            StartMatching();
        }

        public void OnPhotonCreateRoomFailed() {
            Debug.Log("OnPhotonCreateRoomFailed");
        }

        public void OnPhotonJoinRoomFailed(object[] cause) {
            Debug.Log("OnPhotonJoinRoomFailed");
        }

        public void OnPhotonRandomJoinFailed() {
            Debug.Log("OnPhotonRandomJoinFailed");
            PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 }, null);
        }

        public void OnCreatedRoom() {
            Debug.Log("OnCreatedRoom");
        }

        public void OnDisconnectedFromPhoton() {
            Debug.Log("Disconnected from Photon.");
            if (quit) { return; }
            StartCoroutine(retryConnectToPhoton());
        }

        public void OnFailedToConnectToPhoton(object parameters) {
            Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
        }

        private IEnumerator retryConnectToPhoton() {
            statusLabel.text = $"サーバーから切断されました\n3秒後にリトライします";
            yield return new WaitForSeconds(3.0f);
            connectToPhoton();
        }


        public void OnConnectedToMaster() {
            Debug.Log("OnConnectedToMaster()");
            PhotonNetwork.JoinLobby();
        }
    }
}