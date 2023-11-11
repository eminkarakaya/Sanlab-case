using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ListClass
{
    public List<Order> tasks;
}
public class TaskManager : MonoBehaviour
{
    public List<ListClass> tasks;
    public static TaskManager instance;
    private void Awake() {
        instance = this;
    }
    // public List<List<Task>> tasks;
    public Order GetTask(TaskSide taskSide,int index)
    {
        if(taskSide == TaskSide.Up)
        {
            return tasks[0].tasks[index];
        }
        else
        {
            return tasks[1].tasks[index];
        }
    }
    public bool IsFinish()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            for (int j = 0; j < tasks[i].tasks.Count; j++)
            {
                // Debug.Log(tasks[i].tasks[j],tasks[i].tasks[j].taskObject);
                if(!tasks[i].tasks[j].IsDone) 
                    return false;
            }
        }
        return true;
    }

    public List<Order> GetTasks(TaskSide taskSide,int index)
    {
        List<Order> returnTasks = new List<Order>();
        int sideIndex;
        if(taskSide == TaskSide.Up)
        {
            sideIndex = 0;
        }
        else
        {
            sideIndex = 1;
            // tasks[sideIndex].tasks[index].bagliDegil.Count > 0 && 
        }
        for (int i = index+1; i < tasks[sideIndex].tasks.Count; i++)
        {
            if(tasks[sideIndex].tasks[i].IsDone && !tasks[sideIndex].tasks[index].bagliDegil.Contains(i))
            {
                returnTasks.Add(tasks[sideIndex].tasks[i]);
            }
        }
        return returnTasks;
    }
}
