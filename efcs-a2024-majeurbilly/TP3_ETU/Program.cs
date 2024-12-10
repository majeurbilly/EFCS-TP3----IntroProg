using System.Diagnostics;

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

        public const int YEAR_MINIMUM = 2012;
        public readonly List<char> VALIDATION_YES_NO_TOASTER = new List<char> { 'y', 'Y', 'n', 'N' };


        public static void Main(string[] args)
        {
            List<string> allPlayers = ReadPlayersFromFile(PLAYERS_FILE);
            List<Clan> allClans = ReadClansFromFile(CLANS_FILE);
            bool exit = false;

            while (!exit)
            {
                DisplayMenu();
                Console.Write("Votre choix : ");
                string input = Console.ReadLine() ?? String.Empty;
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
                        Console.Write("Entrer l'index du clan à modifier : ");
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
                    else if (choice == ADD_PLAYER)
                    {
                        string newPlayer = CreatePlayer();
                        allPlayers.Add(newPlayer);
                        WriteFile(PLAYERS_FILE, allPlayers.ToArray());
                        Console.WriteLine("Joueur ajouté avec succès !");
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

                WriteClansToFile(CLANS_FILE, allClans);
                WriteFile(PLAYERS_FILE, allPlayers.ToArray());
            }
        }

        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("********************* Clan App *********************");
            Console.WriteLine("0) Quit");
            Console.WriteLine("1) Add a clan");
            Console.WriteLine("2) Update a clan");
            Console.WriteLine("3) Remove a clan");
            Console.WriteLine("4) List all clans");
            Console.WriteLine("5) Add a player");
        }

        public static Clan CreateClan()
        {
            // nom du clan
            Clan clan = new Clan();
            Console.Write("Enter clan name: ");
            clan.Name = Console.ReadLine() ?? "Sans Nom";

            // year 
            do
            {
                Console.Write("Enter year:");
                clan.CreationYear = int.Parse(Console.ReadLine() ?? "0");
                if (clan.CreationYear < YEAR_MINIMUM)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter a date after 2012");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!(clan.CreationYear >= YEAR_MINIMUM));
            
            // type
            do
            {
                Console.Write(
                    "Available category: 0: Exploration, 1: Combat, 2: Trading, 3: Politics, 4: All\nEnter category: ");
                // todo: ici le Parse fait sortir un 0 au lieu ou un null
                string category = Console.ReadLine();
                if (category == "")
                {
                    
                }

                int.TryParse(Console.ReadLine(), out int inputUser);
                    clan.Type = inputUser;

                    if (clan.Type < 0 && clan.Type > 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid category");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        switch (clan.Type)
                        {
                            case EXPLORATION:
                                break;
                            case COMBAT:
                                break;
                            case TRADING:
                                break;
                            case POLITICS:
                                break;
                            case ALL:
                                break;
                        }
                    
                }
            } while (clan.Type < 0 && clan.Type > 4);


            do
            {
                Console.Write("Clan score: ");
                clan.Score = int.Parse(Console.ReadLine() ?? "0");
                if (clan.Score < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Score Invalid");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (clan.Score < 0);

            string? userInputYesNoToaster = "";
            do
            {
                Console.WriteLine("Do you want to add player(s)? (y/n) : ");
                userInputYesNoToaster = Console.ReadLine();
            } while (userInputYesNoToaster == null && userInputYesNoToaster != "y" && userInputYesNoToaster != "n" &&
                     userInputYesNoToaster != "Y" && userInputYesNoToaster != "N");

            clan.Players = new List<int>();
            string? playersInput = Console.ReadLine();
            if ((playersInput != null) && (userInputYesNoToaster == "y" || userInputYesNoToaster == "y"))
            {
                string[] playerIds = playersInput.Split(',');
                foreach (string player in playerIds)
                {
                    if (int.TryParse(player, out int playerId))
                    {
                        clan.Players.Add(playerId);
                    }
                }
            }

            return clan;
        }

        public static void ListAllClans(List<Clan> clans)
        {
            Console.Clear();
            Console.WriteLine("Liste des clans : ");
            for (int i = 0; i < clans.Count; i++)
            {
                Clan clan = clans[i];
                Console.WriteLine(String.Format(
                    $"{i}) {clan.Name,-25} {clan.CreationYear,-20} {clan.Type,10}, Score : {clan.Score,10}, Joueurs : {string.Join(", ", clan.Players),10}"));
            }
        }

        public static string CreatePlayer()
        {
            Console.Write("Nom du joueur : ");
            string name = Console.ReadLine() ?? "Sans Nom";

            return $"{name}";
        }

        private static void WriteClansToFile(string filesClansCsv, List<Clan> allClans)
        {
            string[] allLines = new string[allClans.Count];
            for (int i = 0; i < allClans.Count; i++)
            {
                Clan clan = allClans[i];
                allLines[i] = $"{clan.Name},{clan.CreationYear},{clan.Type},{clan.Score}," +
                              $"{string.Join(";", clan.Players)}";
            }

            WriteFile(filesClansCsv, allLines);
        }

        #region FILE ACCESS

        // PROF : vous aurez peut-être à modifier les noms des propriétés suivantes.

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

        #endregion
    }
}