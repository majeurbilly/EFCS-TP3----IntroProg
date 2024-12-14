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

        public const int TYPE_MINIMUM = 10000;
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
        public const string UPDATE_QUESTION = "Enter clan to update(-1 to cancel): ";
        public const string DELETE_QUESTION = "Enter clan to delete(-1 to cancel): ";
        public const string INVALID_CHOICE = "Invalid choice";
        public const string CREATE_PLAYER_QUESTION = "Enter new player name: ";
        public const string ERROR_ID = "Error id";
        public const string EXPLORATION_STRING = "EXPLORATION";
        public const string COMBAT_STRING = "COMBAT";
        public const string TRADING_STRING = "TRADING";
        public const string POLITICS_STRING = "POLITICS";
        public const string ALL_STRING = "ALL";
        
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
                        MessageGreen("Au revoir !");
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
                        int clanSelected = AskQuestionClanId(allClans, UPDATE_QUESTION);
                        if (clanSelected != -1)
                        {
                            Clan updatedClan = UpdateClan(allClans, clanSelected);
                            Library.UpdateClan(allClans, clanSelected, updatedClan);
                            Console.WriteLine("Clan mis à jour !");
                        }
                    }
                    else if (choice == REMOVE_CLAN)
                    {
                        int clanSelected = AskQuestionClanId(allClans, DELETE_QUESTION);
                        if (clanSelected != -1)
                        {
                            Library.RemoveClan(allClans, clanSelected);
                            Console.WriteLine("Clan removed successfully !");
                        }
                    }
                    else if (choice == LIST_ALL_CLAN)
                    {
                        DisplayAllClans(allClans);
                    }
                    else if (choice == ADD_PLAYER)
                    {
                        string newPlayer = "string.Empty";
                        DisplayPlayers();
                        while (newPlayer != string.Empty)
                        {
                            newPlayer = CreatePlayer();
                            if (newPlayer != string.Empty)
                            {
                                allPlayers.Add(newPlayer);
                                WriteFile(PLAYERS_FILE, allPlayers.ToArray());
                                MessageGreen("Joueur ajouté avec succès !");
                            }
                        }
                    }
                    else
                    {
                        MessageError(INVALID_CHOICE);
                    }
                }
                else
                {
                    MessageError(INVALID_CHOICE);
                }


                MessageGreen("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();

                WriteClansToFile(CLANS_FILE, allClans);
                WriteFile(PLAYERS_FILE, allPlayers.ToArray());
            }
        }

        private static void MessageGreen(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void MessageError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
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
        public static void DisplayPlayers()
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
        }
        public static void DisplayAllClans(List<Clan> clans)
        {
            Console.WriteLine(String.Format($"{"#"} {"Name",-25} {"Year",-10} {"Type",-10} {"Score",-7} {"Players",-30}"));
            Console.WriteLine("=============================================================================================================================");
            for (int i = 0; i < clans.Count; i++)
            {
                Clan clan = clans[i];
                Console.WriteLine(String.Format(
                    $"{i}- {clan.Name,-25} {clan.CreationYear,-7} {ConvertirTypeInString(clan.Type),-10} {clan.Score,-7} {string.Join(", ", ConvertirPlayerInString(clan.Players)),-30}"));
                //todo: aligne moi ca 
            }
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
                clanPlayer = AddClanPlayer(clanPlayer);
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
        private static List<int> AddClanPlayer(List<int> clanPlayer)
        {
            string[] linesToWrite = ReadFile(PLAYERS_FILE);
            DisplayPlayers();
            clanPlayer = SelectPlayer(linesToWrite.Length);
            return clanPlayer;
        }

        private static Clan UpdateClan(List<Clan> allClans, int clanSelected)
        {
            string newNameUpadate = AskQuestionNameUpdate(allClans[clanSelected].Name);
            int newYearUpadte = AskQuestionYearUpadate(allClans[clanSelected].CreationYear);
            int newTypeUpadate = AskQuestionTypeUpdate(allClans[clanSelected].Type);
            int newScore = AskQuestionScoreUpadte(allClans[clanSelected].Score);


            bool addPlayer = AskQuestionBool(ADD_PLAYER_QUESTION, isYesNoQuestion: true);
            List<int> newClanPlayer = new List<int>();
            if (addPlayer)
            {
                newClanPlayer = allClans[clanSelected].Players;
                newClanPlayer = AddClanPlayer(newClanPlayer);
                allClans[clanSelected].Players = newClanPlayer;
            }


            if (newNameUpadate != string.Empty)
            {
                allClans[clanSelected].Name = newNameUpadate;
            }

            if (newYearUpadte != int.MinValue)
            {
                allClans[clanSelected].CreationYear = newYearUpadte;
            }

            if (newTypeUpadate != int.MinValue)
            {
                allClans[clanSelected].Type = newTypeUpadate;
            }

            if (newScore != int.MinValue)
            {
                allClans[clanSelected].Score = newScore;
            }
            return new Clan
            {
                Name = allClans[clanSelected].Name,
                CreationYear = allClans[clanSelected].CreationYear,
                Type = allClans[clanSelected].Type,
                Score = allClans[clanSelected].Score,
                Players =  newClanPlayer
            };
        }
        private static int AskQuestionScoreUpadte(int scoreSelected)
        {
            string input = string.Empty;
            int newScore = int.MinValue;
            do
            {
                Console.Write($"Enter score(press Enter to leave {scoreSelected}): ");
                input = Console.ReadLine() ?? string.Empty;
                if (input == string.Empty)
                {
                    return int.MinValue;
                }

                if (!int.TryParse(input, out newScore) || newScore < Decimal.Zero || newScore > TYPE_MINIMUM)
                {
                    MessageError(INT_ERROR);
                    newScore = int.MinValue;
                }
            } while (newScore < Decimal.Zero || newScore >TYPE_MINIMUM);

            return newScore;
        }
        private static int AskQuestionTypeUpdate(int typeSelected)
        {
            Console.WriteLine(TYPE_MENU);
            
            string input = string.Empty;
            int newType = int.MinValue;
            do
            {
                Console.Write($"Enter category(press Enter to leave {typeSelected}): ");
                input = Console.ReadLine() ?? string.Empty;
                if (input == string.Empty)
                {
                    return int.MinValue;
                }

                if (!int.TryParse(input, out newType) || (newType < EXPLORATION) || (newType > ALL))
                {
                    MessageError(INT_ERROR);
                    newType = int.MinValue;
                }
            } while (newType < EXPLORATION || newType > ALL);

            return newType;
        }
        private static int AskQuestionYearUpadate(int creationYearSelected)
        {
           
            int yearUpadate = int.MinValue;
            string input = string.Empty;
            do
            {
                Console.Write($"Enter year(press Enter to leave {creationYearSelected}): ");
                input = Console.ReadLine() ?? string.Empty;
                if (input == string.Empty)
                {
                    return int.MinValue;
                }

                if (!int.TryParse(input, out yearUpadate) || yearUpadate < YEAR_MINIMUM)
                {
                    MessageError(INT_ERROR);
                    yearUpadate = int.MinValue;
                }
            } while (yearUpadate < YEAR_MINIMUM || input == string.Empty);

            return yearUpadate;
        }

        private static string AskQuestionNameUpdate(string clanSelected)
        {
            Console.Write($"Enter clan name(press Enter to leave {clanSelected}): ");
            string clanName = Console.ReadLine() ?? string.Empty;
            return clanName;
        }

        private static int AskQuestionClanId(List<Clan> allClans, string question)
        {
            int maxValueInput = allClans.Count - 1;
            int clanId;
            bool validInput = true;

            DisplayAllClans(allClans);
            do
            {
                Console.Write(question);
                string input = Console.ReadLine() ?? String.Empty;

                if (!int.TryParse(input, out clanId) || clanId > maxValueInput || clanId <= -2) //todo: -0 = 0 
                {
                    MessageError(INT_ERROR);
                    validInput = false;
                }
                else
                {
                    validInput = true;
                }
            } while (!validInput);

            return clanId;
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
                        if (input < EXPLORATION || input > TYPE_MINIMUM)
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

        private static List<string> ConvertirPlayerInString(List<int> clanPlayers)
        {
            List<string> convertedPlayers = ReadPlayersFromFile(PLAYERS_FILE);
            List<string> newClanPlayersConverted = new List<string>();

            foreach (int playerIndex in clanPlayers)
            {
                if (playerIndex >= Decimal.Zero && playerIndex < convertedPlayers.Count)
                {
                    newClanPlayersConverted.Add(convertedPlayers[playerIndex]);
                }
            }

            return newClanPlayersConverted;
        }

        private static string ConvertirTypeInString(int clanType)
        {
            switch (clanType)
            {
                case EXPLORATION:
                    return EXPLORATION_STRING;
                
                case COMBAT:
                    return COMBAT_STRING;
                
                case TRADING:
                    return TRADING_STRING;
                
                case POLITICS:
                    return POLITICS_STRING;
                case ALL:
                    return ALL_STRING;
                default:
                    return "UNKNOWN";
            }
        }
        private static List<int> SelectPlayer(int playersCount)
        {
            int playerChoice = -1;
            List<int> selectedIds = new List<int> { };
            string userInput = string.Empty;
            do
            {
                if (selectedIds.Count == Decimal.Zero)
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
                        MessageError(ERROR_ID);
                        
                        
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
            string name = string.Empty;
            Console.Write(CREATE_PLAYER_QUESTION);
            name = Console.ReadLine() ?? string.Empty;
            return name;
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