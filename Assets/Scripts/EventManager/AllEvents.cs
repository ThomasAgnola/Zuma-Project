using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}
public class GameHighScoreEvent : SDD.Events.Event
{
}
public class GameCreditEvent : SDD.Events.Event
{
}
public class NewlevelEvent : SDD.Events.Event
{
}
public class AsteroidDestroyedEvent : SDD.Events.Event
{
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
public class NextLevelButtonClickedEvent : SDD.Events.Event
{
}
public class HighScoreButtonClickedEvent : SDD.Events.Event
{
}
public class CreditButtonClickedEvent : SDD.Events.Event
{
}
public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eScore { get; set; }
	public float eCountDownValue { get; set; }
}
#endregion

#region LevelManager Events
public class LevelHasBeenInitializedEvent : SDD.Events.Event
{
	public Vector3 ePlayerSpawnPos;
}
#endregion

#region Asteroid event
public class AsteroidExplosionEvent : SDD.Events.Event
{
}
#endregion
