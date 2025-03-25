public class Task
{
    public string Descriere { get; set; }
    public TaskStare Stare { get; set; }

    public Task(string descriere)
    {
        Descriere = descriere;
        Stare = TaskStare.InAsteptare;
    }

    public override string ToString()
    {
        return $"{Descriere}, {Stare}";
    }
}
