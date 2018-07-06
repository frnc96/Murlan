using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Murlan.Models
{
    public static class StaticClass
    {
        static List<UserGroup> gr = new List<UserGroup>();
        static List<Deck> de = new List<Deck>();
        static String _UserEmail;
        static int _UserId;

        static String SomeUserConnId;
        static int userPassed = 0;
        static int playersIngame = 3;
        static int place = 1;

        public static string UserEmail { get => _UserEmail; set => _UserEmail = value; }
        public static int UserId { get => _UserId; set => _UserId = value; }
        public static List<UserGroup> Gr { get => gr; set => gr = value; }
        public static List<Deck> De { get => de; set => de = value; }

        public static string SomeUserConnId1 { get => SomeUserConnId; set => SomeUserConnId = value; }
        public static object FirstPLayerConnId { get; internal set; }
        public static int UserPassed { get => userPassed; set => userPassed = value; }
        public static int PlayersIngame { get => playersIngame; set => playersIngame = value; }
        public static int Place { get => place; set => place = value; }
    }
}