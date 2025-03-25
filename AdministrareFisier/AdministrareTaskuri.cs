using System;

public class AdministrareTaskuri
{
    private AdministrarePersoane administrarePersoane;

    public AdministrareTaskuri(AdministrarePersoane administrarePersoane)
    {
        this.administrarePersoane = administrarePersoane;
    }

    public void AdaugaTask(string numePersoana, string descriereTask)
    {
        Persoana persoana = administrarePersoane.CautaPersoana(numePersoana);
        if (persoana != null)
        {
            persoana.AdaugaTask(descriereTask);
        }
    }

    public void MarcheazaTaskCaFinalizat(string numePersoana, string descriereTask)
    {
        Persoana persoana = administrarePersoane.CautaPersoana(numePersoana);
        if (persoana != null)
        {
            persoana.MarcheazaTaskFinalizat(descriereTask);
        }
    }

    public void AfiseazaTaskuri(string numePersoana)
    {
        Persoana persoana = administrarePersoane.CautaPersoana(numePersoana);
        if (persoana != null)
        {
            persoana.AfiseazaTaskuri();
        }
        else
        {
            Console.WriteLine("Persoana nu a fost gasita.");
        }
    }
}
