namespace LibrarieModele
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Persoana
    {
        public string Nume { get; }
        private List<Task> taskuri;

        public Persoana(string nume)
        {
            Nume = nume;
            taskuri = new List<Task>();
        }

        public void AdaugaTask(Task task)
        {
            taskuri.Add(task);
        }

        public void MarcheazaTaskFinalizat(string descriereTask)
        {
            var task = taskuri.FirstOrDefault(t => t.Descriere.Equals(descriereTask, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.EsteFinalizat = true;
            }
        }

        public List<Task> GetTaskuri()
        {
            return taskuri;
        }
    }
}
