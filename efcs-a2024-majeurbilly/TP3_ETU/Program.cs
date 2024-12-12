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
        private const string NAME_QUESTION = "Enter clan name: ";
        private const string NAME_ERROR = "Please enter any name: ";
        private const string YEAR_QUESTION = "Enter year: ";
        private const string YEAR_ERROR_UNDER_2012 = "Enter a date after 2012";
        private const string INT_ERROR = "Enter a valid number in decimal format";
        private const string TYPE_QUESTION = "Enter category: ";
        private const string TYPE_ERROR = "Category Invalid: ";
        private const string SCORE_QUESTION = "Enter score: ";
        private const string ADD_PLAYER_QUESTION = "Do you want to add player(s) (Y or N)? ";
        private const string ZERO_PLAYER_QUESTION = "Enter player id: ";
        public const string MESSAGE_ERROR = "Incorrect value.";
        const string UPDATE_QUESTION = "Enter clan to update(-1 to cancel): ";

        private const string TYPE_MENU =
            "Available category: 0: Exploration, 1: Combat, 2: Trading, 3: Politics, 4: All";


        public static void Main(string[] args)
        {
            List<string> allPlayers = ReadPlayersFromFile(PLAYERS_FILE);
            List<Clan> allClans = ReadClansFromFile(CLANS_FILE);
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                DisplayMenu();
                Console.Write("Your choice : ");
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
                        Clan newClan = AddClan();
                        Library.InsertClan(allClans, newClan);
                        Console.WriteLine("Clan added successfully !");
                    }
                    else if (choice == UPDATE_CLAN)
                    {
                        DisplayAllClans(allClans);
                        UpdateClan(allClans);
                        Console.Write("Entrer l'index du clan à modifier : ");
                        int index = int.Parse(Console.ReadLine() ?? "0");
                        Clan updatedClan = AddClan();
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
                        DisplayAllClans(allClans);
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
                    MessageError("Veuillez entrer un numéro valide.");
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

        public static Clan AddClan()
        {
            string clanName = AskQuestionString(NAME_QUESTION, NAME_ERROR, isNameQuestion: true);
            int clanYear = AskQuestionInt(YEAR_QUESTION, MESSAGE_ERROR, isYearQuestion: true);
            Console.Write($"{TYPE_MENU}\nEnter category: ");
            int clanType = AskQuestionInt(TYPE_QUESTION, MESSAGE_ERROR, isType: true);
            int clanScore = AskQuestionInt(SCORE_QUESTION, MESSAGE_ERROR, isScoreQuestion: true);
            bool addPlayer = AskQuestionBool(ADD_PLAYER_QUESTION, isYesNoQuestion: true);
            List<int> clanPlayer = new List<int>();

            if (addPlayer)
            {
                string[] linesToWrite = ReadFile(PLAYERS_FILE);
                if (linesToWrite.Length > 0)
                {
                    Console.WriteLine($"Players found: {linesToWrite.Length}");
                    for (int i = 0; i < linesToWrite.Length; i++)
                    {
                        Console.WriteLine($"Player #{i.ToString()}: {linesToWrite[i]}");
                    }
                }

                clanPlayer = SelectPlayer(linesToWrite.Length);
            }

            return new Clan
            {
                Name = clanName,
                CreationYear = clanYear,
                Type = clanType,
                Score = clanScore,
                Players = clanPlayer
            };
        }

        private static void UpdateClan(List<Clan> allClans)
        {
            
                int clanSelected = AskQuestionClanId(allClans);
                if (clanSelected == -1)
                {
                    break;
                }
                string newNameUpadate = AskQuestionNameUpdate(allClans[clanSelected].Name);
                int newYearUpadte = AskQuestionYearUpadate(allClans[clanSelected].CreationYear);
                int newTypeUpadate = AskQuestionTypeUpdate(allClans[clanSelected].Type);
                int newScore = AskQuestionScoreUpadte(allClans[clanSelected].Score);
                bool addPlayer = AskQuestionBool(ADD_PLAYER_QUESTION, isYesNoQuestion: true);
                List<int> newClanPlayer = allClans[clanSelected].Players;
                string[] linesToWrite = ReadFile(PLAYERS_FILE);
                if (addPlayer)
                {
                    Console.WriteLine($"Players found: {linesToWrite.Length}");
                    for (int i = 0; i < linesToWrite.Length; i++)
                    {
                        Console.WriteLine($"Player #{i.ToString()}: {linesToWrite[i]}");
                    }
                    
                }
                
        }

        private static int AskQuestionScoreUpadte(int scoreSelected)
        {
            Console.WriteLine($"Enter score(press Enter to leave {scoreSelected}): ");
            string input = string.Empty;
            int newScore = -1;
            do
            {
                input = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(input, out newScore))
                {
                    MessageError(INT_ERROR);
                    newScore = -1;
                }
            } while (newScore > 0 && newScore < 10000);
            return newScore;
            
        }

        private static int AskQuestionTypeUpdate(int typeSelected)
        {
            Console.WriteLine(TYPE_MENU);
            Console.WriteLine($"Enter category(press Enter to leave {typeSelected}): ");
            string input = string.Empty;
            int newType = -1;
            do
            {
                input = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(input, out newType))
                {
                    MessageError(INT_ERROR);
                    newType = -1;
                }
            } while (newType < EXPLORATION && newType > ALL);
            
            return newType;
        }

        private static int AskQuestionYearUpadate(int creationYearSelected)
        {
            Console.WriteLine($"Enter year(press Enter to leave {creationYearSelected}): ");
            int yearUpadate = -1;
            string input = string.Empty;
            do
            {
                input = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(input, out yearUpadate))
                {
                    MessageError(INT_ERROR);
                    yearUpadate = -1;
                }
            } while (yearUpadate < YEAR_MINIMUM);
            return yearUpadate;
        }

        private static string AskQuestionNameUpdate(string clanSelected)
        {
            Console.WriteLine($"Enter clan name(press Enter to leave {clanSelected}): ");
            string clanName = Console.ReadLine() ?? string.Empty;
            return clanName;
        }

        private static int AskQuestionClanId(List<Clan> allClans)
        {
            int maxValueInput = allClans.Count - 1;
            int clanId;
            bool validInput = true;
            
            DisplayAllClans(allClans);
            do
            {
                Console.WriteLine(UPDATE_QUESTION);
                string input = Console.ReadLine() ?? String.Empty;
                if (!int.TryParse(input, out clanId) && clanId > maxValueInput && clanId < -2)
                {
                    MessageError(INT_ERROR);
                    validInput = false;
                }
                else
                {
                    validInput = true;
                }
            } while (!validInput);
            return clanId ;
        }

        public static void DisplayAllClans(List<Clan> clans)
        {
            for (int i = 0; i < clans.Count; i++)
            {
                Clan clan = clans[i];
                Console.WriteLine(String.Format(
                    $"{i,5}- {clan.Name,-25} {clan.CreationYear,-20} {clan.Type,10}, Score : {clan.Score,10}, Joueurs : {string.Join(", ", clan.Players),10}"));
                //todo: aligne moi ca 
            }
        }

        private static string AskQuestionString(string question, string error, bool isNameQuestion = false)
        {
            string answer = string.Empty;
            bool validAnswer = true;
            do
            {
                Console.Write(question);
                string input = Console.ReadLine() ?? string.Empty;
                if (isNameQuestion)
                {
                    if (input == string.Empty)
                    {
                        MessageError(error);
                        validAnswer = false;
                    }
                    else
                    {
                        answer = input;
                        validAnswer = true;
                    }
                }
            } while (!validAnswer);

            return answer;
        }

        private static int AskQuestionInt(string question, string error, bool isYearQuestion = false,
            bool isType = false, bool isScoreQuestion = false)
        {
            int answer = -1;
            do
            {
                Console.Write(question);
                if (!int.TryParse(Console.ReadLine(), out int input))
                {
                    MessageError(error);
                }
                else
                {
                    if (isYearQuestion)
                    {
                        if (input < YEAR_MINIMUM)
                        {
                            input = -1;
                            MessageError(YEAR_ERROR_UNDER_2012);
                        }
                    }
                    else if (isType)
                    {
                        if (input < EXPLORATION || input > ALL)
                        {
                            input = -1;
                            MessageError(error);
                        }
                    }
                    else if (isScoreQuestion)
                    {
                        if (input < EXPLORATION || input > 10000)
                        {
                            input = -1;
                            MessageError(error);
                        }
                    }
                    

                    answer = input;
                }
            } while (answer == -1);

            return answer;
        }

        public static void MessageError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static bool AskQuestionBool(string question, bool isYesNoQuestion = false)
        {
            bool answer = false;
            bool valid = false;
            while (!valid)
            {
                Console.Write(question);
                string input = Console.ReadLine() ?? string.Empty;
                if (isYesNoQuestion)
                {
                    if (input != null && input == "y" || input == "Y")
                    {
                        answer = true;
                        valid = true;
                    }
                    else if (input != null && input == "n" || input == "N")
                    {
                        answer = false;
                        valid = true;
                    }
                    else
                    {
                        MessageError("Only y or n Allowed");
                    }
                }
                else if (input == string.Empty)
                {
                    answer = false;
                }
            }

            return answer;
        }

        private static List<int> SelectPlayer(int playersCount)
        {
            int playerChoice = -1;
            List<int> selectedIds = new List<int> { };
            string userInput = string.Empty;
            do
            {
                if (selectedIds.Count == 0)
                {
                    Console.Write(ZERO_PLAYER_QUESTION);
                }
                else
                {
                    Console.Write(
                        $"Enter player id (press Enter to leave {string.Join(", ", selectedIds)}):: ");
                }

                userInput = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(userInput, out playerChoice) || playerChoice > playersCount - 1 ||
                    playerChoice < 0)
                {
                    if (selectedIds.Count != 0 && userInput == string.Empty)
                    {
                        return selectedIds;
                    }
                    else
                    {
                        MessageError("Error id");
                        userInput = string.Empty;
                        playerChoice = -1;
                    }
                }
                else
                {
                    selectedIds.Add(playerChoice);
                    userInput = string.Empty;
                    playerChoice = -1;
                }
            } while (userInput == string.Empty);

            return selectedIds;
        }

        public static string CreatePlayer()
        {
            Console.Write("Nom du joueur : ");
            string name = Console.ReadLine() ?? "Sans Nom";

            return $"{name}";
        }

        #region FILE ACCESS

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