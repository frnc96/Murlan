using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Murlan.Models;

namespace Murlan
{
    public class CardHub : Hub
    {
        #region Server Methods

        public void getMyHand()
        {
            foreach (UserGroup ug in StaticClass.Gr)
            {
                if (ug.connId.Equals(Context.ConnectionId))
                {
                    Boolean turn = false;
                    Stack<String[]> otherPlayers = new Stack<String[]>();
                    foreach (Deck d in StaticClass.De)
                    {
                        if (d.GroupName.Equals(ug.group))
                        {
                            ug.Hand = d.TheHand.Pop();
                            foreach(String str in ug.Hand)
                            {
                                if (str.Contains("spades") && str.Contains("rank3"))
                                {
                                    turn = true;
                                }
                            }
                            foreach (String[] s in d.OpponentHands1)
                            {
                                if (!s.Equals(ug.Hand))
                                    otherPlayers.Push(s);
                            }
                        }
                    }

                    Clients.Client(ug.connId).recieveHand(ug.connId, ug.Hand, turn);
                    Clients.Client(ug.connId).p1Hand(ug.connId, otherPlayers.Pop());
                    Clients.Client(ug.connId).p2Hand(ug.connId, otherPlayers.Pop());
                    Clients.Client(ug.connId).p3Hand(ug.connId, otherPlayers.Pop());
                }
            }
        }
        public Task joinRoom(string roomName)
        {
            int groupMembers = 0;
            foreach(UserGroup ug in StaticClass.Gr)
            {
                if (ug.group.Equals(roomName))
                    groupMembers++;
            }

            if (groupMembers < 4)
            {
                if (groupMembers == 0)
                {
                    StaticClass.De.Add(new Deck(roomName)); //Initialise the deck for a new group
                    StaticClass.Gr.Add(new UserGroup(roomName, Context.ConnectionId)); //Add the user to user list
                    StaticClass.SomeUserConnId1 = Context.ConnectionId; //Store conn id for next user
                    StaticClass.FirstPLayerConnId = Context.ConnectionId;
                    return Groups.Add(Context.ConnectionId, roomName);
                }
                else if (groupMembers < 3)
                {
                    StaticClass.Gr.Add(new UserGroup(roomName, Context.ConnectionId, StaticClass.SomeUserConnId1));
                    StaticClass.SomeUserConnId1 = Context.ConnectionId;
                    return Groups.Add(Context.ConnectionId, roomName);
                }
                else if (groupMembers == 3)
                {
                    foreach (UserGroup ug in StaticClass.Gr)
                    {
                        if (ug.connId.Equals(StaticClass.FirstPLayerConnId))
                        {
                            ug.nextPlayerConnId = Context.ConnectionId;
                            break;
                        }
                    }
                    StaticClass.Gr.Add(new UserGroup(roomName, Context.ConnectionId, StaticClass.SomeUserConnId1));
                    return Groups.Add(Context.ConnectionId, roomName);
                }
                else
                    return null;
            }
            else
                return null;
        }
        public Task leaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
        public void broadcastPlay(String[] play)
        {
            Boolean freeHand = false;

            if (play.Length == 0)
                StaticClass.UserPassed += 1; //The user who broadcasted the play passed his turn
            else
                StaticClass.UserPassed = 0;

            freeHand = StaticClass.UserPassed == StaticClass.PlayersIngame; //Decide if to erase field play in case everyone passes

            Clients.Client(Context.ConnectionId).enablePlay(false); //Disable the turn for the player
            foreach(UserGroup ug in StaticClass.Gr)
            {
                if (ug.connId.Equals(Context.ConnectionId))
                    Clients.Client(ug.nextPlayerConnId).enablePlay(true); //Enable the turn of the player on queue
            }
            Clients.Group("8080").recievePlay(Context.User.Identity.Name, play, freeHand);
        }
        public void playerIsOut()
        {
            String placements = "";
            String tempConId = "";
            StaticClass.PlayersIngame -= 1;

            if (StaticClass.PlayersIngame == 0) //If all are out
            {
                String group = "";
                foreach (UserGroup ug in StaticClass.Gr) //Find out the group
                    if (ug.connId.Equals(Context.ConnectionId))
                    {
                        group = ug.group;
                        break;
                    }

                foreach (UserGroup ug in StaticClass.Gr) //Set the place of the last user
                    if (ug.connId.Equals(Context.ConnectionId))
                    {
                        ug.Place = 4;
                        break;
                    }

                foreach (UserGroup ug in StaticClass.Gr) //Get the placement of each player
                    if (ug.group.Equals(group))
                        placements += ug.Place + " " + ug.Username + "|";

                Clients.Group("8080").allOut(placements);
            }
            else
            {
                foreach(UserGroup ug in StaticClass.Gr) //When a player is out set his place
                    if (ug.connId.Equals(Context.ConnectionId))
                        ug.Place = ++StaticClass.Place;

                foreach (UserGroup ug in StaticClass.Gr)
                    if (ug.connId.Equals(Context.ConnectionId))
                    {
                        Clients.Client(ug.nextPlayerConnId).enablePlay(true);
                        tempConId = ug.nextPlayerConnId;
                        break;
                    }

                foreach (UserGroup ug in StaticClass.Gr) //When a user is out remove him from turn queue
                    if (ug.nextPlayerConnId.Equals(Context.ConnectionId))
                    {
                        ug.nextPlayerConnId = tempConId;
                        break;
                    }
            }
        }
        public void callTheSpread()
        {
            int count = 0;
            foreach(UserGroup ug in StaticClass.Gr)
            {
                if (ug.group.Equals("8080"))
                    count++;
            }
            if(count == 4)
                Clients.Group("8080").spreadCards();
        }

        #endregion
    }
}