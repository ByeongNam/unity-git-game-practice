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

    [Server] // prevent client access
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    private void HandleDisplayNameUpdate(string oldName, string newName) // old new 는 형식
    {   
        displayNameText.SetText(newName);
    }
    private void HandleDisplayColorUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color",newColor); // _Color?
    }
}
