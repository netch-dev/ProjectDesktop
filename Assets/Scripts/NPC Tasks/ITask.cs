public interface ITask {
	bool IsAvailable(NPC npc);
	bool IsComplete(NPC npc);
	void ExecuteTask(NPC npc);
}