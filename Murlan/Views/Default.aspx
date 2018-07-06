<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Murlan.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cards</title>
    <link rel="stylesheet" href="../node_modules/deck-of-cards/example/example.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/zebra_dialog@latest/dist/css/flat/zebra_dialog.min.css">
</head>

<style>
    #playBtn, #pasBtn {
        bottom: 5%;
        right: 5%;
        position: fixed;
    }

    #fieldContainer {
        position: absolute;
        top: 50%;
        left: 50%;
    }

    #container {
        top: 30%;
        left: 10%;
    }
    .card{
        z-index: 0;
    }
    .face {
        cursor: pointer;
    }
</style>

<body>
    
    <script src="../node_modules/deck-of-cards/dist/deck.js"></script>
    <script src="../JS/jquery-3.2.1.js"></script>
    <script src="../Scripts/jquery.signalR-2.2.2.js"></script>
    <script src="../JS/notify.js"></script>
    <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>
    <script src="https://cdn.jsdelivr.net/npm/zebra_dialog/dist/zebra_dialog.min.js"></script>
    <%--<link href="../CSS/bootstrap.min.css" rel="stylesheet" />--%>

    <div id="container"></div>
    <div id="fieldContainer"></div>
    <input id="playBtn" class="btn btn-primary" type="button" value="Play" onclick="performPlay()" />
    <input id="pasBtn" class="btn btn-primary" type="button" value="Pass" style="bottom: 10%" onclick="passPlay()" />

    <script>
        var $container = document.getElementById('container');
        var $fieldContainer = document.getElementById('fieldContainer');

        // create Deck
        var deck = Deck(true);
        var _hand = [];
        var _play = [];
        var _playSize = 0;
        var _field = [];
        var _turn = false;
        var _isOut = false;
        var _zIndex = 0;

        // add to DOM
        deck.mount($container);
        deck.sort();
        deck.cards[1].unmount();

        //Create HUB reference
        var CardHub = $.connection.cardHub;

        //Start a connection with the HUB
        $.connection.hub.start().done(function () {
            console.log("Connected with " + $.connection.hub.transport.name);

            //Join the group 
            CardHub.server.joinRoom("8080").done(function () { console.log("You joined group 8080"); });
            CardHub.server.callTheSpread();

            //animate cards when clicked and record hand in local variable
            $(document).click(function (event) {
                var div = $(event.target).parent().prop('className');
                var cl = $(event.target).prop('className');
                if (cl == 'face') {
                    _hand.forEach(function (element) {
                        var elS = element.split("|");
                        var divS = div.split(" ");

                        if (divS[2] === elS[3] && divS[1] === elS[2]) {
                            var index = elS[0];
                            deck.cards[index].animateTo({
                                delay: 30,
                                duration: 300,
                                ease: 'quartOut',
                                y: 430
                            });
                            _play.push(divS[1] + "|" + divS[2] + "|" + elS[0]);
                            _playSize++;
                        }
                    });
                }
            });
        });

        //Recieve the broadcast plays from other players
        CardHub.client.recievePlay = function (id, play, freeHand) {
            console.log(play);
            if (freeHand)
                _field = [];
            else if (!freeHand && play.length > 0) {
                _field = play;
                var dist = 0;
                var rndX = Math.floor(Math.random() * 100);
                var rndY = Math.floor(Math.random() * 100);
                play.forEach(function (element) {
                    var index = element.split("|")[2];
                    $('.card.' + element.split("|")[0] + '.' + element.split("|")[1]).css('z-index', _zIndex++); //Trying not to have an orgy of cards
                    deck.cards[index].animateTo({
                        delay: 500 + dist,
                        duration: 500,
                        ease: 'quartOut',
                        x: rndX + 450 + dist,
                        y: rndY + 100
                    });
                    dist += 80;
                });

                setTimeout(function () {
                    play.forEach(function (element) {
                        var index = element.split("|")[2];
                        deck.cards[index].setSide('front');
                    });
                }, 1000);
            }
        }
        //Recieve the hand of the current player
        CardHub.client.recieveHand = function (id, cards, turn) {
            _turn = turn;
            if (cards !== null) {
                _hand = cards;
                var dis = 0;
                cards.forEach(function (element) {
                    //console.log(element);
                    var el = element.split("|")[0];
                    $('.card.' + element.split("|")[0] + '.' + element.split("|")[1]).css('z-index', _zIndex++);
                    deck.cards[el].animateTo({
                        delay: 1000 + dis,
                        duration: 1000,
                        ease: 'quartOut',
                        x: 30 + dis,
                        y: 450
                    });
                    dis += 80;
                });
                setTimeout(function () { flipHand(); }, 2500);
            }
            if (turn) {
                $.notify("You have the 3 of spades, procced to start the game.", "info");
            }
        }
        //Recive the hands of the opponents
        CardHub.client.p1Hand = function (id, cards) {
            console.log(cards);
            var dis = 0;
            cards.forEach(function (element) {
                var el = element.split("|")[0];
                $('.card.' + element.split("|")[2] + '.' + element.split("|")[3]).css('z-index', _zIndex++);
                deck.cards[el].animateTo({
                    delay: 1000 + dis,
                    duration: 1000,
                    ease: 'quartOut',
                    x: 200 + dis,
                    y: -80
                });
                dis += 50;
            });
        }
        CardHub.client.p2Hand = function (id, cards) {
            var dis = 0;
            cards.forEach(function (element) {
                var el = element.split("|")[0];
                $('.card.' + element.split("|")[2] + '.' + element.split("|")[3]).css('z-index', _zIndex++);
                deck.cards[el].animateTo({
                    delay: 1000 + dis,
                    duration: 1000,
                    ease: 'quartOut',
                    x: 30,
                    y: 0 + dis
                });
                dis += 20;
            });
        }
        CardHub.client.p3Hand = function (id, cards) {
            var dis = 0;
            cards.forEach(function (element) {
                var el = element.split("|")[0];
                $('.card.' + element.split("|")[2] + '.' + element.split("|")[3]).css('z-index', _zIndex++);
                deck.cards[el].animateTo({
                    delay: 1000 + dis,
                    duration: 1000,
                    ease: 'quartOut',
                    x: 1070,
                    y: 0 + dis
                });
                dis += 20;
            });
        }
        CardHub.client.spreadCards = function () {
            $.connection.hub.start().done(function () {
                CardHub.server.getMyHand();
            });
        }
        CardHub.client.enablePlay = function (turn) {
            if (!_isOut) {
                _turn = turn;
                if (turn) {
                    $.notify("It is your turn.", "success");
                }
                else
                    console.log("Not your turn mate srry");
            }
            //else {
            //    //The case that a player is out so can not have a turn
            //    if (turn) {
            //        _turn = false;
            //        $.connection.hub.start().done(function () {
            //            CardHub.server.playerIsOut();
            //        });
            //    }
            //}
        }
        CardHub.client.allOut = function (placements) {
            var plc = placements.split("|");

            plc.sort(function (x, y) {
                var xp = parseInt(x.match(/\d+/));
                var yp = parseInt(y.match(/\d+/));
                return xp == yp ? 0 : xp < yp ? -1 : 1;
            });

            $.Zebra_Dialog('First Place: ' + plc[1] + '</br>Second Place: ' + plc[2] + '</br>Third Place: ' + plc[3] + '</br>Fourth Place: ' + plc[4], {
                title: 'Game Summary',
                onClose: function () { window.location.replace("/Views/Lobby.aspx"); },
                buttons: [
                    { caption: 'Confirm', callback: function () { window.location.replace("/Views/Lobby.aspx"); } }
                ]
            });
            $.connection.hub.start().done(function () {
                CardHub.server.leaveRoom("8080").done(function () { console.log("You left the group.") });
            });
        }

        //Validation of the hand of the current player
        function resetCards() {
                _play.forEach(function (element) {
                    var index = element.split("|")[2];
                    deck.cards[index].animateTo({
                        delay: 300,
                        duration: 300,
                        ease: 'quartOut',
                        y: 450
                    });
                });
                _play = [];
                _playSize = 0;
            }
        function flipHand() {
                _hand.forEach(function (element) {
                    var index = element.split("|")[0];
                    deck.cards[index].setSide('front');
                });
            }
        function validateHand() {
            var valid = true;
            if (_play.length !== 0) {

                if (_play.length < 5) {
                    var crdRank = _play[0].split("|")[1];
                    _play.forEach(function (element) {
                        if (crdRank.toString() !== element.split("|")[1])
                            valid = false;
                    });
                } else {
                    _play.sort(function (x, y) {
                        var xp = parseInt(x.match(/\d+/));
                        var yp = parseInt(y.match(/\d+/));
                        return xp == yp ? 0 : xp < yp ? -1 : 1;
                    });

                    var weightFirst = parseInt(_play[0].match(/\d+/));
                    var weightLast = parseInt(_play[_playSize - 1].match(/\d+/));

                    if (weightFirst === 1 && weightLast === 13) {
                        _play[0] = _play[0].replace("rank1", "rank14");
                        if (parseInt(_play[1].match(/\d+/)) === 2)
                            _play[1] = _play[1].replace("rank2", "rank15");

                        _play.sort(function (x, y) {
                            var xp = parseInt(x.match(/\d+/));
                            var yp = parseInt(y.match(/\d+/));
                            return xp == yp ? 0 : xp < yp ? -1 : 1;
                        });
                        var weightFirst = parseInt(_play[0].match(/\d+/));
                        var weightLast = parseInt(_play[_playSize - 1].match(/\d+/));
                    }

                    _play.forEach(function (element) {
                        var suit = element.split("|")[0];
                        if (suit !== "joker") {
                            var elWeight = parseInt(element.match(/\d+/));
                            if (elWeight !== weightFirst) {
                                valid = false;
                            } weightFirst++;
                        } else valid = false;
                    });
                }
            } else valid = false;
            return valid;
        }
        function validatePlay() {
            var legal = true;
            if (validateHand()) {
                if (_field.length > 0) {
                    if (_play.length === _field.length) {

                        var suit = _play[0].split("|")[0];
                        var rank = parseInt(_play[0].split("|")[1].match(/\d+/));
                        var fieldRank = parseInt(_field[0].split("|")[1].match(/\d+/));
                        var fieldSuit = _field[0].split("|")[0];
                        if (_play.length < 5) {
                            //check if we have a joker/ace/two and if not check rank
                            if (suit === "joker") {
                                switch (rank) {
                                    case 1:
                                        rank = 16;
                                        break;
                                    case 3:
                                        rank = 17;
                                        break;
                                }
                            } else {
                                switch (rank) {
                                    case 1:
                                        rank = 14;
                                        break;
                                    case 2:
                                        rank = 15;
                                        break;
                                }
                            }

                            if (fieldSuit === "joker") {
                                switch (fieldRank) {
                                    case 1:
                                        fieldRank = 16;
                                        break;
                                    case 3:
                                        fieldRank = 17;
                                        break;
                                }
                            } else {
                                switch (fieldRank) {
                                    case 1:
                                        fieldRank = 14;
                                        break;
                                    case 2:
                                        fieldRank = 15;
                                        break;
                                }
                            }

                            if (fieldRank >= rank) legal = false;

                        } else {
                            //check if hand straight is bigger than field straight

                        }

                    } else legal = false;
                } else legal = true;
            } else legal = false;
            return legal;
            }
        function performPlay() {
                if (_turn) {
                    if (validatePlay()) {
                        //console.log(_hand);
                        var dist = 0;
                        var rndX = Math.floor(Math.random() * 100);
                        var rndY = Math.floor(Math.random() * 100);
                        _play.forEach(function (element) {
                            var index = element.split("|")[2];
                            deck.cards[index].animateTo({
                                delay: 500 + dist,
                                duration: 500,
                                ease: 'quartOut',
                                x: rndX + 450 + dist,
                                y: rndY + 100
                            });
                            dist += 40;
                        });

                        $.connection.hub.start().done(function () {
                            CardHub.server.broadcastPlay(_play).done(function () { console.log("Play was broadcasted!") });
                        });

                        rmPlayFromHand();
                        _field = _play;
                        _play = [];
                        _playSize = 0;
                        
                    if (!_hand.length > 0) {
                            _isOut = true;
                            $.notify("Congrats you are out :)", "success");
                            //Tell server that user is out
                            _turn = false;
                            $.connection.hub.start().done(function () {
                                CardHub.server.playerIsOut();
                            });
                        }
                    }
                    else {
                        $.notify("That is a illegal move :(", "error");
                        resetCards();
                    }
                } else {
                    resetCards();
                    $.notify("Please wait for your turn!!", "error");
                }
            }
        function passPlay() {
            if (_turn) {
                resetCards();
                $.connection.hub.start().done(function () {
                    CardHub.server.broadcastPlay(_play).done(function () { console.log("Play was broadcasted!") });
                    });
                    $.notify("You passed!", "info");
                } else
                    $.notify("Please wait for your turn!!", "error");
        }

        function rmPlayFromHand() {
            var newHand = new Array();
            _hand.forEach(function (handCard) {
                var playIsInHand = false;
                _play.forEach(function (playCard) {
                    if (handCard.split("|")[0] === playCard.split("|")[2]) //compare id's of cards
                        playIsInHand = true;
                });
                if (!playIsInHand)
                    newHand.push(handCard);
            });
            _hand = newHand;
        }
        
    </script>
</body>
</html>