public class IdleState : INPCState {
	public void OnEnter(NPC npc) {
		npc.StopMoving();
	}

	public void OnUpdate(NPC npc) {
		ITask nextTask = npc.GetTaskQueue().GetNextTask();
		if (nextTask != null) {
			npc.SetState(new TaskState(nextTask));
		}
	}

	public void OnExit(NPC npc) { }
}
