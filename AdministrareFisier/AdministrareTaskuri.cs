﻿namespace AdministrareFisier
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using LibrarieModele;
    

    public class AdministrareTaskuri
    {
        private const string numeFisierTaskuri = @"..\..\..\AplicatieCuMaiMulteProiecte\bin\Debug\taskuri.txt";
        private AdministrareClienti adminClienti;

        public AdministrareTaskuri(AdministrareClienti adminClienti)
        {
            this.adminClienti = adminClienti;
            IncarcaTaskuri();
        }

        public void AdaugaTask(string numePersoana, string descriereTask)
        {
            var persoana = adminClienti.CautaPersoana(numePersoana);
            if (persoana != null)
            {
                persoana.AdaugaTask(new Task(descriereTask));
                SalveazaTaskuri();
            }
        }

        public void MarcheazaTaskCaFinalizat(string numePersoana, string descriereTask)
        {
            var persoana = adminClienti.CautaPersoana(numePersoana);
            if (persoana != null)
            {
                persoana.MarcheazaTaskFinalizat(descriereTask);
                SalveazaTaskuri();
            }
        }

        public void AfiseazaTaskuri(string numePersoana)
        {
            var persoana = adminClienti.CautaPersoana(numePersoana);
            if (persoana != null)
            {
                foreach (var task in persoana.GetTaskuri())
                {
                    Console.WriteLine($"- {task.Descriere} [{(task.EsteFinalizat ? "X" : " ")}]");
                }
            }
        }

        private void IncarcaTaskuri()
        {
            if (File.Exists(numeFisierTaskuri))
            {
                try
                {
                    foreach (var line in File.ReadAllLines(numeFisierTaskuri))
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            var persoana = adminClienti.CautaPersoana(parts[0]);
                            if (persoana != null)
                            {
                                var task = new Task(parts[1])
                                {
                                    EsteFinalizat = bool.Parse(parts[2])
                                };
                                persoana.AdaugaTask(task);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid line format in {numeFisierTaskuri}: {line}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading {numeFisierTaskuri}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File {numeFisierTaskuri} not found.");
            }
        }


        private void SalveazaTaskuri()
        {
            var taskLines = new List<string>();
            foreach (var persoana in adminClienti.GetPersoane())
            {
                foreach (var task in persoana.GetTaskuri())
                {
                    taskLines.Add($"{persoana.Nume}|{task.Descriere}|{task.EsteFinalizat}");
                }
            }
            File.WriteAllLines(numeFisierTaskuri, taskLines);
        }
    }
}
