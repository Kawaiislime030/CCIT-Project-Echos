using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Remark 1: Dialogue Lines, autoProgressLines and end progress lines always same amount.
Remark 2: Choices Points, Choices and next dialogues Index always same amount.
*/

[CreateAssetMenu(fileName= "NewNPCDialogue" , menuName = "NPC Dialogue")]

public class NPCDialogue : ScriptableObject
{
	//public string npcName;
	//public Sprite npcPortrait; 
	public string[] dialogueLines;
	public float typingSpeed = 0.05f;
	public AudioClip voiceSound;
	public float voicePitch = 1f;
	public bool[] endDialogueLines; //Mark where dialogue ends
	public float autoProgressDelay = 1.5f;
	public DialogueChoice[] choices;
}

[System.Serializable]

public class DialogueChoice
{
	public int dialogueIndex; //Dialogue line where choices appear
	public string[] choices; //player response options
	public int[] nextDialogueIndexes; //Where choice leads
	public int[] choicePoints; // points for each choice

}




    