using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Order 
{
    public bool IsDone;
    public GameObject taskObject;
    public List<int> bagliDegil;
}



// unity de iç içe classlar inspectorda gorulmedigi icin boyle bir cozum buldum.
[System.Serializable]
public class ListClass
{
    public List<Order> tasks;
}
public class TaskManager : Singleton<TaskManager>
{
    public UnityEvent OnFinish;
    [SerializeField] private GameObject finishText;
    [SerializeField] private List<ListClass> tasks;
    public bool isFinish {get; private set;}
  
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
    [ContextMenu("Finish")]
    public void Finish()
    {
        Camera.main.GetComponent<CameraMovement>().CameraFinishAnimation(2f);
        SoundManager.Instance.FinishSoundEffect();
        isFinish = true;
        finishText.SetActive(true);
        OnFinish?.Invoke();
    }
    public bool CheckFinish()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            for (int j = 0; j < tasks[i].tasks.Count; j++)
            {
                if(!tasks[i].tasks[j].IsDone) 
                    return false;
            }
        }
        Finish();
        
        return true;
    }
    /// <summary>
    /// GameManagerdeki OnRestart eventinde çalışıyor.
    /// Gorevleri sıfırlıyor ve FinishText vb UIları kapatıyor.
    /// </summary>
    public void Restart()
    {
        finishText.SetActive(false);
        RestartTasks();
        isFinish = false;
    }
    private void RestartTasks()
    {
        foreach (var item in tasks)
        {
            foreach (var item1 in item.tasks)
            {
                item1.IsDone = false;
            }
        }
    }
    /// <summary>
    ///  objeyi monte ederken veya çıkarırken engelleyen objeleri döndürür.
    /// </summary>
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
