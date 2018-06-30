using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Game.Multi {
    public class MatchingManager : MonoBehaviour {
        private static readonly char NameIdSeparator = '_';

        [SerializeField]
        private GameObject matchingWindow;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;
        [SerializeField]
        private Button cancelButton;
        
        private string parseNickName(string rawName) {
            var at_pos = rawName.LastIndexOf(NameIdSeparator);
            if (at_pos == -1) {
                return rawName;
            }
            return rawName.Substring(0, at_pos);
        }

        private void Awake() {
            PhotonNetwork.automaticallySyncScene = true;
            connectToPhoton();
            // PhotonNetwork.logLevel = PhotonLogLevel.Full;
        }

        private void Start() {
            matchingWindow.SetActive(true);
            matchingWindow.GetComponent<Animator>().Play(@"OpenWindow");
        }

        private void connectToPhoton() {
            if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
                Debug.Log("Connecting to Server...");
                PhotonNetwork.ConnectUsingSettings("1.0");
            }
        }

        private IEnumerator requestJoinRandomRoom() {
            yield return new WaitUntil(() => PhotonNetwork.connectedAndReady);
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartMatching() {
            StartCoroutine(requestJoinRandomRoom());
        }

        public void CancelMatching() {
            matchingWindow.GetComponent<Animator>().Play(@"CloseWindow");
            if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
            SceneController.Instance.LoadScene(SceneName.Title, 1.0f);
        }

        private void updateOtherPlayerStatus() {
            if (PhotonNetwork.otherPlayers.Length > 0) {
                StartCoroutine(successMatching());
            }
        }

        private IEnumerator successMatching() {
            Debug.Log(@"Found the other player. Will begin transit to network battle scene, Wait a moment...");
            messageLabel.text = @"対戦相手が見つかりました！";
            statusLabel.text = @"あいて: " + parseNickName(PhotonNetwork.otherPlayers[0].NickName);
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
            updateOtherPlayerStatus();
        }

        public void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");

            if (string.IsNullOrEmpty(PhotonNetwork.playerName)) {
                PhotonNetwork.playerName = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName) + NameIdSeparator + PhotonNetwork.AuthValues.UserId;
            }

            statusLabel.text = @"あなた: " + parseNickName(PhotonNetwork.playerName);
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
        }

        public void OnFailedToConnectToPhoton(object parameters) {
            Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
            StartCoroutine(retryConnectToPhoton());
        }

        private IEnumerator retryConnectToPhoton() {
            yield return new WaitForSeconds(3.0f);
            connectToPhoton();
        }


        public void OnConnectedToMaster() {
            Debug.Log("OnConnectedToMaster()");
            PhotonNetwork.JoinLobby();
        }
    }
}