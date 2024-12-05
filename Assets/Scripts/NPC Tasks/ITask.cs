public interface ITask {
	bool IsAvailable();
	bool IsComplete(NPC npc);
	void ExecuteTask(NPC npc);
	void OnStart(NPC npc);
	void OnFinish(NPC npc);
}
