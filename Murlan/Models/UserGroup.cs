using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Murlan.Models
{
    public class UserGroup
    {
        private String username = "unknown";
        public String group;
        public String connId;
        public String nextPlayerConnId;
        private String[] hand;
        private int place;

        public UserGroup(String gr, String id)
        {
            group = gr;
            connId = id;
        }

        public UserGroup(String gr, String id, String nxtPlayer)
        {
            nextPlayerConnId = nxtPlayer;
            group = gr;
            connId = id;
        }


        public string[] Hand { get => hand; set => hand = value; }
        public int Place { get => place; set => place = value; }
        public string Username { get => username; set => username = value; }
    }
}