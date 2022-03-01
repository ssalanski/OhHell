extends Node2D

export(PackedScene) var Card
export(PackedScene) var Player
export(PackedScene) var Trick

var players = []
var me

var num_cpu_players = 0

var trumpCard
var currentTrick

func _init():
	randomize() # TODO: this may move elsewhere

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# everyone runs this function, but only the server deals the cards each hand
remotesync func run_game():
	print("gametime!")
	var card_counts = [7,6,5,4,3,2,1,2,3,4,5,6,7]
	for hand_num in range(13):
		var card_count = card_counts[hand_num]
		if get_tree().is_network_server():
			print("server is dealing %d cards" % card_count)
			deal_cards_and_trump(card_count)
		var lead_player = players[hand_num%players.size()]
		for _trick_num in range(card_count):
			print("everyone is playing round %d" % _trick_num)
			yield(play_trick(lead_player), "completed")
			lead_player = currentTrick.get_winner()
			currentTrick.queue_free()

	# runs only on the master node
	if get_tree().is_network_server():
		pass
	# this function, run only by the master node, will progress the game hand by hand, trick by trick, turn by turn
	pass

func deal_cards_and_trump(num_cards):
	var deck = range(0,52)
	deck.shuffle()
	for _cardnum in range(num_cards):
		for player in players:
			var c = deck.pop_back()
			rpc("deal_card",player.name,c)
			#rpc("deal_card",player.name,player.seat) # debug trick, set card values as player seat instead, to see who sits where
	rpc("deal_trump", deck.pop_back())

# runs everywhere, including caller
remotesync func deal_card(id, card):
	# only accept calls from server
	if get_tree().get_rpc_sender_id() == 1:
		get_node(id).receive_card(card)

# runs everywhere, including caller
remotesync func deal_trump(card):
	# only the server can set trump
	if get_tree().get_rpc_sender_id() == 1:
		trumpCard = Card.instance()
		add_child(trumpCard)
		trumpCard.set_value(card)
		trumpCard.set_faceup(true)
		trumpCard.position = $TrumpCardAnchor.position

func play_trick(lead_player):
	currentTrick = Trick.instance()
	add_child(currentTrick)
	currentTrick.position = Vector2(512,300)
	var player = lead_player
	while currentTrick.cards.size() < players.size():
		player.take_turn()
		yield(player, "card_played")
		player = player.next_player
	yield(get_tree().create_timer(3), "timeout")

func seat_players(_player_list):
	print("my id is: " + str(get_tree().get_network_unique_id()))
	for player_info in _player_list:
		print("seating a player like: " + str(player_info))
		var player = Player.instance()
		player.set_name(str(player_info["id"]))
		player.is_a_real_boy = !player_info["name"].begins_with("CPU")
		player.playername = player_info["name"]
		player.seat = player_info["seat"]
		if player.is_a_real_boy:
			print(player.playername + " is a real boy, network master: " + str(player_info["id"]))
			player.set_network_master(player_info["id"])
		else:
			print(player.playername + " is CPU, network master: " + str(player.get_network_master()))
		players.append(player)
		add_child(player)
		print("added player %s named %s at seat %d" % [player.name,player.playername,player.seat])
		player.connect("card_played", self, "on_play_card")
		if player.name == str(get_tree().get_network_unique_id()):
			me = player
		elif player.playername.begins_with("CPU"):
			num_cpu_players = num_cpu_players + 1
	# set up link references from each player to the next, in circular seating order, clunky here, convenient later
	var seatmap = {}
	for seat in range(players.size()):
		for player in players:
			if player.seat == seat:
				seatmap[seat] = player
	for seat in range(players.size()):
		seatmap[seat].next_player = seatmap[(seat+1)%players.size()]
	# spatially arrange players around table, with network master seated at the front, but maintaining order around the table
	for player in players:
		if player == me:
			player.position = $MainPlayerAnchor.position
		else:
			var pos_clockwise_from_me = ((players.size() + player.seat - me.seat) % players.size()) - 1
			if players.size() == 2:
				$SeatingPath/PathFollow2D.set_unit_offset(0.5)
			elif players.size() == 3:
				$SeatingPath/PathFollow2D.set_unit_offset(0.333 * (pos_clockwise_from_me+1))
			else:
				var offset_increment = 1.0 / (players.size() - 2)
				$SeatingPath/PathFollow2D.set_unit_offset(pos_clockwise_from_me*offset_increment)
			player.position = $SeatingPath/PathFollow2D.position
			player.look_at(Vector2(512,300))
			player.rotate(PI/2)
	# TODO: why cant I rpc call myself, and just have it run locally?
	if get_tree().get_network_unique_id() == 1:
		player_seated()
	else:
		rpc_id(1, "player_seated")

# make sure all players have initialized their game tree with player nodes
var players_seated = []
# TODO: would this be "master"?
remote func player_seated():
	var who = get_tree().get_rpc_sender_id()
	assert(get_tree().is_network_server())
	#assert(who in players)
	assert(not who in players_seated)
	print("%d is ready" % who)
	players_seated.append(who)
	if players_seated.size() == players.size() - num_cpu_players:
		print("everyone's ready!")
		rpc("run_game")
		

func on_play_card(player_id, card_ref):
	var player = card_ref.get_parent()
	if player.is_a_real_boy:
		assert(player.name == str(player_id))
	player.remove_child(card_ref)
	player.arrange_hand()
	currentTrick.add_child(card_ref)
	currentTrick.accept_card(player, card_ref)
