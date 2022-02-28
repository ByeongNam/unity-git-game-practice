using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    [SyncVar (hook = nameof(HandleDisplayNameUpdate))] //synchroniaze variable, SnycVar는 특정 변수를 동기화 변수로 만들어줘서 값이 변경되면 자동으로 동기화
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdate))] 
    // hook 은 해당 변수가 값이 변경되었을 때 등록된 함수가 호출
    // 함수가 호출될 때 변경된 변수값이 매개변수로 전달
    [SerializeField]
    private Color displayColor = Color.black;

    #region Server

    [Server] // prevent client access
    public void SetDisplayName(string newDisplayName)// needs validation for client and server
    {
        if(newDisplayName.Length < 2){
            Debug.Log("Your name must be over 2 letters");
            return;
        }
        if(newDisplayName.Length > 12){
            Debug.Log("Your name must be under 12 letters");
            return;
        }
        if(newDisplayName.Contains(' ')){
            Debug.Log("There is no space for Whitespace in your name");
            return;
        }

        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }
    
    [Command] 
    // Commands are sent from player objects on the client to player objects on the server.
    private void CmdSetDisplayName(string newDisplayName)// 이름을 정하는데 validation for client
    {
        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName); 
    }

    #endregion

    #region Client

    private void HandleDisplayNameUpdate(string oldName, string newName) // old new 는 형식
    {   
        displayNameText.SetText(newName);
    }
    private void HandleDisplayColorUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color",newColor); // _BaseColor?
    }

    [ContextMenu("SetMyName")] 
    // ContextMenu는 사용자가 인스펙터에 첨부된 스크립트를 context 메뉴에 기능을 추가할 수 있도록 해줌 
    // 인스펙터에 있는 스크립트에 우클릭
    private void SetMyName()
    {
        CmdSetDisplayName("NewName");
    }

    [ClientRpc]
    //ClientRpc 호출은 서버의 오브젝트에서 모든 클라이언트 오브젝트로 전송
    //TargetRpc 서버의 오브젝트에서 특정 클라이언트 오브젝트로 전송
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log("RPC: " + newDisplayName);
    }
    #endregion


}
