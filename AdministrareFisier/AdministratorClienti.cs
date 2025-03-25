using System;
using System.Collections.Generic;
using System.IO;

public class AdministratorClienti
{
    private string numeFisier;
    private List<Persoana> persoane;
    private AdministrarePersoane administrarePersoane;
    private AdministrareTaskuri administrareTaskuri;

    public AdministratorClienti(string numeFisier)
    {
        this.numeFisier = numeFisier;
        this.persoane = new List<Persoana>();
        this.administrarePersoane = new AdministrarePersoane();
        this.administrareTaskuri = new AdministrareTaskuri(administrarePersoane);
        CitesteDinFisier();
    }

    public void AdaugaPersoana(string nume)
    {
        administrarePersoane.AdaugaPersoana(nume);
        ScrieInFisier();
    }

    public Persoana CautaPersoana(string nume)
    {
        return administrarePersoane.CautaPersoana(nume);
    }

    public void AdaugaTask(string numePersoana, string descriereTask)
    {
        administrareTaskuri.AdaugaTask(numePersoana, descriereTask);
        ScrieInFisier();
    }

    public void MarcheazaTaskCaFinalizat(string numePersoana, string descriereTask)
    {
        administrareTaskuri.MarcheazaTaskCaFinalizat(numePersoana, descriereTask);
        ScrieInFisier();
    }

    public void AfiseazaTaskuri(string numePersoana)
    {
        administrareTaskuri.AfiseazaTaskuri(numePersoana);
    }

    private void ScrieInFisier()
    {
        using (StreamWriter sw = new StreamWriter(numeFisier))
        {
            foreach (var persoana in administrarePersoane.GetPersoane())
            {
                sw.WriteLine(persoana.ToString());
            }
        }
    }

    private void CitesteDinFisier()
    {
        if (File.Exists(numeFisier))
        {
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    var date = linie.Split('|');
                    string numePersoana = date[0].Trim();
                    Persoana persoana = new Persoana(numePersoana);

                    string[] taskuri = date[1].Split(';');
                    foreach (var task in taskuri)
                    {
                        persoana.AdaugaTask(task.Trim());
                    }
                    administrarePersoane.AdaugaPersoana(persoana.Nume);
                }
            }
        }
    }
}
