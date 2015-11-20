using UnityEngine;
using System.Collections;

 public class GameStateManager : MonoBehaviour
{

	static public void ToStart()
	{ 
		Messenger.Broadcast("changegamestate", STATES.START);
		Application.LoadLevel(Application.loadedLevel);
	}
	static public void ToPlay()
	{
		Messenger.Broadcast("changegamestate", STATES.PLAY);
	}
	static public void ToPause()
	{
		Messenger.Broadcast("changegamestate",STATES.PAUSE);
	}
	static public void ToGameover()
	{
		Messenger.Broadcast("changegamestate",STATES.GAMEOVER);
	}
	static public void ToEnd()
	{
		Messenger.Broadcast("changegamestate",STATES.END);
	}
	static public void ToTerm()
	{
		Messenger.Broadcast("changegamestate",STATES.TERM);
	}
}
