using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageData", menuName = "ScriptableObjects/MessageData", order = 1)]
public class MessagesDatabase : ScriptableObject
{
    public List<string> messagePT;
    public List<string> messageEN;
}
