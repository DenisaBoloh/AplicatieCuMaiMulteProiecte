using System;
using System.Collections.Generic;

public class Persoana
{
    public string Nume { get; set; }
    private List<Task> taskuri;

    public Persoana(string nume)
    {
        Nume = nume;
        taskuri = new List<Task>();
    }

    public void AdaugaTask(string descriereTask)
    {
        Task task = new Task(descriereTask);
        taskuri.Add(task);
    }

    public void MarcheazaTaskFinalizat(string descriereTask)
    {
        Task task = taskuri.Find(t => t.Descriere.Equals(descriereTask, StringComparison.OrdinalIgnoreCase));
        if (task != null)
        {
            task.Stare = TaskStare.Finalizat;
        }
    }

    public void AfiseazaTaskuri()
    {
        Console.WriteLine($"Taskuri pentru {Nume}:");
        foreach (var task in taskuri)
        {
            Console.WriteLine($"{task.Descriere} - {task.Stare}");
        }
    }

    public override string ToString()
    {
        string taskuriString = string.Join(";", taskuri);
        return $"{Nume} | {taskuriString}";
    }
}
