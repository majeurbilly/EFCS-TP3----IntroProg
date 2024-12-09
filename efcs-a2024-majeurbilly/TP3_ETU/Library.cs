namespace TP3_ETU
{
    public  class Library
    {
        public static void InsertClan(List<Clan> allClans, Clan clan)
        {
            if (allClans == null)
            {
                throw new ArgumentException("La liste des clans ne peut pas être nulle");
            }

            if (clan == null)
            {
                throw new ArgumentException("Le clan ne peut pas être nul.");
            }

            allClans.Add(clan);
        }
        public static void RemoveClan(List<Clan> allClans, int clanIndex)
        {
            if (allClans == null)
            {
                throw new ArgumentException("La liste des clans ne peut pas être nulle.");
            }

            if (clanIndex < 0 || clanIndex >= allClans.Count)
            {
                throw new ArgumentException("Index du clan non valide.");
            }

            allClans.RemoveAt(clanIndex);
        }
        public static void UpdateClan(List<Clan> allClans, int clanIndex, Clan updatedClan)
        {
            if (allClans == null)
            {
                throw new ArgumentException("La liste des clans ne peut pas être nulle.");
            }

            if (updatedClan == null)
            {
                throw new ArgumentException("Le clan mis à jour ne peut pas être nul.");
            }

            if (clanIndex < 0 || clanIndex >= allClans.Count)
            {
                throw new ArgumentException("Index du clan non valide..");
            }

            allClans[clanIndex] = updatedClan;
        }
        
    }
}
