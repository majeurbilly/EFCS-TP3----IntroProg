namespace TP3_ETU
{
    public class Program
    {
        // Sources des données
        public const string PLAYERS_FILE = "Files/players.csv";
        public const string CLANS_FILE = "Files/clans.csv";
        public const char PLAYERS_SEPARATION_TOKEN = ';';

        // Types de clan : vous pourriez également essayer d'utiliser une enum.
        public const int EXPLORATION = 0;
        public const int COMBAT = 1;
        public const int TRADING = 2;
        public const int POLITICS = 3;
        public const int ALL = 4;

        // Menu
        public const int QUIT = 0;
        public const int ADD_CLAN = 1;
        public const int UPDATE_CLAN = 2;
        public const int REMOVE_CLAN = 3;
        public const int LIST_ALL_CLAN = 4;
        public const int ADD_PLAYER = 5;
        public static readonly int[] CHOICING = { QUIT, ADD_CLAN, UPDATE_CLAN, REMOVE_CLAN, LIST_ALL_CLAN, ADD_PLAYER };


    public static void Main(string[] args)
    {
            // Initialisation
            List<Clan> allClans = new List<Clan>();
            bool exit = false;

            while (!exit)
            {
                // Affiche le menu
                DisplayMenu();
                Console.Write("Votre choix : ");
                string? input = Console.ReadLine();
                int choice;

                if (int.TryParse(input, out choice))
                {
                    if (choice == QUIT)
                    {
                        Console.WriteLine("Au revoir !");
                        exit = true;
                    }
                    else if (choice == ADD_CLAN)
                    {
                        Clan newClan = CreateClan();
                        Library.InsertClan(allClans, newClan);
                        Console.WriteLine("Clan ajouté avec succès !");
                    }
                    else if (choice == UPDATE_CLAN)
                    {
                        Console.Write("Entrez l'index du clan à modifier : ");
                        int index = int.Parse(Console.ReadLine() ?? "0");
                        Clan updatedClan = CreateClan();
                        Library.UpdateClan(allClans, index, updatedClan);
                        Console.WriteLine("Clan mis à jour !");
                    }
                    else if (choice == REMOVE_CLAN)
                    {
                        Console.Write("Entrez l'index du clan à supprimer : ");
                        int index = int.Parse(Console.ReadLine() ?? "0");
                        Library.RemoveClan(allClans, index);
                        Console.WriteLine("Clan supprimé !");
                    }
                    else if (choice == LIST_ALL_CLAN)
                    {
                        ListAllClans(allClans);
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un numéro valide.");
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }

        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Menu principal");
            Console.WriteLine("0) Quitter");
            Console.WriteLine("1) Ajouter un clan");
            Console.WriteLine("2) Modifier un clan");
            Console.WriteLine("3) Supprimer un clan");
            Console.WriteLine("4) Lister tous les clans");
        }

        public static Clan CreateClan()
        {
            Clan clan = new Clan();
            Console.Write("Nom du clan : ");
            clan.ClanName = Console.ReadLine() ?? "Sans Nom";

            Console.Write("Année de création : ");
            clan.ClanYear = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Type de clan (0: Exploration, 1: Combat, 2: Commerce, 3: Politique) : ");
            clan.ClanCategory = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Score du clan : ");
            clan.ClanScore = int.Parse(Console.ReadLine() ?? "0");

            clan.ClanPlayers = new List<int>();
            Console.WriteLine("Entrez les IDs des joueurs (séparés par des espaces) : ");
            string? playersInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(playersInput))
            {
                string[] playerIds = playersInput.Split(' ');
                foreach (string id in playerIds)
                {
                    if (int.TryParse(id, out int playerId))
                    {
                        clan.ClanPlayers.Add(playerId);
                    }
                }
            }

            return clan;
        }

        public static void ListAllClans(List<Clan> clans)
        {
            Console.Clear();
            Console.WriteLine("Liste des clans :");
            for (int i = 0; i < clans.Count; i++)
            {
                Clan clan = clans[i];
                Console.WriteLine($"{i}) {clan.ClanName} - Année : {clan.ClanYear}, Type : {clan.ClanCategory}, Score : {clan.ClanScore}, Joueurs : {string.Join(", ", clan.ClanPlayers)}");
            }
        }

    private static List<string> ReadPlayersFromFile(string filesPlayersCsv)
    {
        try
        {
            List<string> players = new List<string>();
            using (StreamReader reader = new StreamReader(filesPlayersCsv))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    players.Add(line.Trim()); // Suppression des espaces en trop
                }
            }
            return players;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier des joueurs : {ex.Message}");
            return new List<string>(); // Retourne une liste vide en cas d'erreur
        }
    }

    private static List<Clan> ReadClansFromFile(string filesClansCsv)
    {
        try
        {
            List<Clan> clans = new List<Clan>();
            using (StreamReader reader = new StreamReader(filesClansCsv))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(','); // Séparer les parties du clan
                    if (parts.Length >= 5)
                    {
                        Clan clan = new Clan
                        {
                            ClanName = parts[0],
                            ClanYear = int.Parse(parts[1]),
                            ClanCategory = int.Parse(parts[2]),
                            ClanScore = int.Parse(parts[3]),
                            ClanPlayers = parts[4].Split(';').Select(p => int.Parse(p)).ToList()
                        };
                        clans.Add(clan);
                    }
                }
            }
            return clans;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier des clans : {ex.Message}");
            return new List<Clan>(); // Retourne une liste vide en cas d'erreur
        }
    }

    private static void WriteClansToFile(string filesClansCsv, List<Clan> allClans)
    {
        string[] allLines = new string[allClans.Count];
        for (int i = 0; i < allClans.Count; i++)
        {
            Clan clan = allClans[i];
            allLines[i] = $"{clan.ClanName},{clan.ClanYear},{clan.ClanCategory},{clan.ClanScore}," +
                          $"{string.Join(";", clan.ClanPlayers)}";
        }
        WriteFile(filesClansCsv, allLines);
    }

    private static void WriteFile(string filesPlayersCsv, string[] toArray)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filesPlayersCsv, false, System.Text.Encoding.UTF8))
            {
                foreach (string line in toArray)
                {
                    writer.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'écriture dans le fichier : {ex.Message}");
        }
    }

    #region FILE ACCESS

 
    // PROF : vous aurez peut-être à modifier les noms des propriétés suivantes.
    private static void WriteClansToFile(string fileName, List<Clan> allClans)
    {
        string[] allLines = new string[allClans.Count];
        for (int i = 0; i < allClans.Count; i++)
        {
            allLines[i] = allClans[i].Name;
            allLines[i] += "," + Convert.ToString(allClans[i].CreationYear);
            allLines[i] += "," + Convert.ToString(allClans[i].Type);
            allLines[i] += "," + Convert.ToString(allClans[i].Score);
            allLines[i] += ",";

            for (int j = 0; j < allClans[i].Players.Count; j++)
            {
                if (j > 0)
                    allLines[i] += ";";
                allLines[i] += Convert.ToString(allClans[i].Players[j]);
            }



        }
        WriteFile(fileName, allLines);
    }

    private static List<string> ReadPlayersFromFile(string filename)
    {
        List<string> players = new List<string>();
        string[] allLines = ReadFile(filename);
        for (int i = 0; i < allLines.Length; i++)
        {
            if (!string.IsNullOrEmpty(allLines[i]))
            {
                players.Add(allLines[i]);
            }
        }
        return players;
    }

    private static List<Clan> ReadClansFromFile(string fileName)
    {
        List<Clan> allClans = new List<Clan>();
        string[] allLines = ReadFile(fileName);
        for (int i = 0; i < allLines.Length && !string.IsNullOrEmpty(allLines[i]); i++)
        {
            string[] currentLine = allLines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
            Clan newClan = new Clan();
            newClan.Name = currentLine[0];
            newClan.CreationYear = int.Parse(currentLine[1]);
            newClan.Type = int.Parse(currentLine[2]);
            newClan.Score = int.Parse(currentLine[3]);
            newClan.Players = new List<int>();
            if (currentLine.Length > 4)
            {
                string[] playersId = currentLine[4].Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in playersId)
                {
                    newClan.Players.Add(int.Parse(id));
                }
            }
            allClans.Add(newClan);
        }
        return allClans;
    }

    /// <summary>
    /// Lit un fichier texte et stocke une ligne par cellule de tableau.
    /// </summary>
    /// <param name="fileName">Nom du fichier à lire. Il doit être situé
    /// dans le dossier bin/Debug/net6.0/Files</param>
    /// <param name="nbLinesMax">Nombres de lignes maximum qui pourront être lues dans le fichier</param>
    /// <returns>Un tableau des lignes lues</returns>
    public static string[] ReadFile(string fileName)
    {
        StreamReader reader = new StreamReader(fileName, System.Text.Encoding.UTF8);
        List<string> allLines = new List<string>();

        while (!reader.EndOfStream)
        {
#pragma warning disable CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.
            string line = reader.ReadLine();
#pragma warning restore CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.
            allLines.Add(line);

        }

        reader.Close();

        return allLines.ToArray();
    }

    /// <summary>
    /// Écrit un fichier texte à partir d'un tableau de lignes.
    /// </summary>
    /// <param name="fileName">Nom du fichier à écrire. Il sera situé
    /// dans le dossier bin/Debug/net6.0/Files</param>
    /// <param name="linesToWrite">Tableau contenant les lignes à écrire</param>
    public static void WriteFile(string fileName, string[] linesToWrite)
    {
        StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);

        for (int i = 0; i < linesToWrite.Length; i++)
        {
            writer.WriteLine(linesToWrite[i]);
        }

        writer.Close();
    }
    */

    #endregion
}

}