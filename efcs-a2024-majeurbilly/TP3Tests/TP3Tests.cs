using TP3_ETU;

namespace TP3_ETU.Tests
{
    public class TP3Tests
    {
       
        private List<Clan> CreateClanList(int nbClans)
        {
            List<Clan> clanList = new List<Clan>();
            for (int i = 0; i < nbClans; i++)
            {
                Clan clan = new Clan();
                clan.ClanName = $"Titre {i}";
                clan.ClanYear = 2012 + i;
                clan.ClanCategory = (i % Program.ALL+1);
                clan.ClanScore = i % 10;
                clanList.Add(clan);
            }
            return clanList;
        }
 
        [Fact]
        public void InsertClan_EmptyList_ListWithOneElement()
        {
            const string ANY_TITLE = "Any ClanName";
            const int ANY_YEAR = 1999;
            const int ANY_ClanCategory = Program.COMBAT;
            const int ANY_ClanScore = 5000;
            const int DEFAULT_NB_CLANS = 0;

            List<Clan> allClans = new List<Clan>();


            Clan clan = new Clan();
            clan.ClanName = ANY_TITLE;
            clan.ClanYear = ANY_YEAR;
            clan.ClanCategory = ANY_ClanCategory;
            clan.ClanScore = ANY_ClanScore;

            Library.InsertClan(allClans, clan);

            Assert.Single(allClans);
            Assert.Equal(ANY_TITLE, allClans[DEFAULT_NB_CLANS].ClanName);
            Assert.Equal(ANY_YEAR, allClans[DEFAULT_NB_CLANS].ClanYear);
            Assert.Equal(ANY_ClanCategory, allClans[DEFAULT_NB_CLANS].ClanCategory);
            Assert.Equal(ANY_ClanScore, allClans[DEFAULT_NB_CLANS].ClanScore);
        }
        [Fact]
        public void InsertClan_NullClan_Exception()
        {
            List<Clan> allClans = new List<Clan>();
            Assert.Throws<ArgumentException>(() => Library.InsertClan(allClans, null));
        }

        [Fact]
        public void InsertClan_NotEmptyList_ListWithOneMoreElement()
        {
            const string ANY_TITLE = "Any ClanName";
            const int ANY_YEAR = 1999;
            const int ANY_ClanCategory = Program.COMBAT;
            const int ANY_ClanScore = 7;
            const int DEFAULT_NB_CLANS = 12;

            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);

            Clan clan = new Clan();
            clan.ClanName = ANY_TITLE;
            clan.ClanYear = ANY_YEAR;
            clan.ClanCategory = ANY_ClanCategory;
            clan.ClanScore = ANY_ClanScore;

            Library.InsertClan(allClans, clan);

            Assert.Equal(DEFAULT_NB_CLANS + 1, allClans.Count);
            Assert.Equal(ANY_TITLE, allClans[DEFAULT_NB_CLANS].ClanName);
            Assert.Equal(ANY_YEAR, allClans[DEFAULT_NB_CLANS].ClanYear);
            Assert.Equal(ANY_ClanCategory, allClans[DEFAULT_NB_CLANS].ClanCategory);
            Assert.Equal(ANY_ClanScore, allClans[DEFAULT_NB_CLANS].ClanScore);
        }
        [Fact]
        public void RemoveClan_ClanNumBiggerThanListSize_Exception()
        {
            const int DEFAULT_NB_CLANS = 12;
            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);
            Assert.Throws<ArgumentException>(() => { Library.RemoveClan(allClans, DEFAULT_NB_CLANS + 1); });
        }
        
        [Fact]
        public void RemoveClan_NegativeClanNum_Exception()
        {
            const int DEFAULT_NB_CLANS = 12;
            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS); ;
            Assert.Throws<ArgumentException>(() => { Library.RemoveClan(allClans, -1); });
        }
        [Fact]
        public void RemoveClan_NotEmptyListAndValidClanNum_ListWithOneMoreLessElement()
        {

            const int DEFAULT_NB_CLANS = 12;
            const int ANY_VALID_CLAN_NUM = DEFAULT_NB_CLANS / 2;

            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);

            Library.RemoveClan(allClans, ANY_VALID_CLAN_NUM);

            Assert.Equal(DEFAULT_NB_CLANS - 1, allClans.Count);
        }
        [Fact]
        public void UpdateClan_ClanNumBiggerThanListSize_Exception()
        {
            const int DEFAULT_NB_CLANS = 12;

            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);
            Assert.Throws<ArgumentException>(() => { Library.UpdateClan(allClans, DEFAULT_NB_CLANS + 1, new Clan()); });
        }
        [Fact]
        public void UpdateClan_NegativeClanNum_Exception()
        {
            const int DEFAULT_NB_CLANS = 12;
            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS); ;
            Assert.Throws<ArgumentException>(() => { Library.UpdateClan(allClans, -1, new Clan()); });
        }
        [Fact]
        public void UpdateClan_NotEmptyListAndValidClanNum_ListWithClanUpdated()
        {

            const int DEFAULT_NB_CLANS = 12;
            const int ANY_VALID_CLAN_NUM = DEFAULT_NB_CLANS / 2;

            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);
            const string ANY_TITLE = "Any ClanName";
            const int ANY_YEAR = 1999;
            const int ANY_ClanCategory = Program.COMBAT;
            const int ANY_ClanScore = 7;
            Clan clan = new Clan();
            clan.ClanName = ANY_TITLE;
            clan.ClanYear = ANY_YEAR;
            clan.ClanCategory = ANY_ClanCategory;
            clan.ClanScore = ANY_ClanScore;

            Library.UpdateClan(allClans, ANY_VALID_CLAN_NUM, clan);

            Assert.Equal(DEFAULT_NB_CLANS, allClans.Count);
            Assert.Equal(clan, allClans[ANY_VALID_CLAN_NUM]);
        }
        [Fact]
        public void UpdateClan_NullClan_Exception()
        {
            const int DEFAULT_NB_CLANS = 12;

            List<Clan> allClans = CreateClanList(DEFAULT_NB_CLANS);
            Assert.Throws<ArgumentException>(() => { Library.UpdateClan(allClans, DEFAULT_NB_CLANS + 1, null); });
        }
    }
}