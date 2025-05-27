public class Task
{
    public string Descriere { get; set; }
    public bool EsteFinalizat { get; set; }
    public string Priority { get; set; }
    public bool IsImportant { get; set; }

    public Task(string descriere)
    {
        Descriere = descriere;
        EsteFinalizat = false;
        Priority = "Medium";
        IsImportant = false;
    }
}