namespace LibrarieModele
{
    public class Task
    {
        public string Descriere { get; }
        public bool EsteFinalizat { get; set; }

        public Task(string descriere)
        {
            Descriere = descriere;
            EsteFinalizat = false;
        }
    }
}