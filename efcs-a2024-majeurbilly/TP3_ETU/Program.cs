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



        public static void Main(string[] args)
        {
            //List<string> allPlayers = ReadPlayersFromFile(PLAYERS_FILE);
            //List<Clan> allClans = ReadClansFromFile(CLANS_FILE);

            //WriteClansToFile(CLANS_FILE, allClans);
            //WriteFile(PLAYERS_FILE, allPlayers.ToArray());
        }

        #region FILE ACCESS
        /*
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