public class TaskState : INPCState {
	private ITask currentTask;

	public TaskState(ITask task) {
		currentTask = task;
	}

	public void OnEnter(NPC npc) {
		currentTask?.OnStart(npc);
	}

	public void OnUpdate(NPC npc) {
		if (currentTask == null || currentTask.IsComplete(npc)) {
			npc.SetState(new IdleState());
		} else {
			currentTask.ExecuteTask(npc);
		}
	}

	public void OnExit(NPC npc) {
		currentTask?.OnFinish(npc);
	}
}
