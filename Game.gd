extends Node2D

export(PackedScene) var Card
export(PackedScene) var Player
export(PackedScene) var Trick

var players = []
var me

var trumpCard
var currentTrick

func _init():
	randomize() # TODO: this may move elsewhere

# Called when the node enters the scene tree for the first time.
func _ready():
	seat_players()
	#set_table()
	play_hand(5)

var _player_list
func set_players(player_list):
	_player_list = player_list

func seat_players():
	print("my id is: " + str(get_tree().get_network_unique_id()))
	for player_info in _player_list:
		var player = Player.instance()
		player.id = player_info["id"]
		player.playername = player_info["name"]
		player.seat = player_info["seat"]
		players.append(player)
		add_child(player)
		player.connect("play_card", self, "on_play_card")
		if player.id == get_tree().get_network_unique_id():
			me = player
	for player in players:
		if player == me:
			player.position = $MainPlayerAnchor.position
			player.show_hand()
		else:
			var rotated_slot_num = (player.seat - me.seat) % players.size()
			player.position = Vector2(0,200).rotated(rotated_slot_num*PI/2) + Vector2(512,300)

func set_table():
	for player in players:
		add_child(player)
		player.connect("play_card", self, "on_play_card")
		if player == me:
			player.position = $MainPlayerAnchor.position
			player.show_hand()
		else:
			var rotated_slot_num = (player.seat - me.seat) % players.size()
			player.position = Vector2(0,200).rotated(rotated_slot_num*PI/2) + Vector2(512,300)


func play_hand(num_cards):
	if get_tree().is_network_server():
		deal_hand(num_cards)
	else:
		deal_fake_hands_to_others(num_cards)
	for trick_num in range(num_cards):
		print("playing round %d"%trick_num)
		yield(play_round(), "completed")

func deal_hand(num_cards):
	var deck = range(0,52)
	deck.shuffle()
	for _cardnum in range(num_cards):
		for player in players:
			var c = deck.pop_back()
			player.receive_card(c)
			if player != me:
				send_card(player.id,c)
	me.show_hand()
	# TODO: showing all hands on host for debug purposes
	for player in players:
		player.show_hand()
	set_trump(deck.pop_back())
	send_trump(trumpCard.value)

func send_card(id, card):
	print("sending card %d to %d" % [card,id])
	rpc_id(id, "receive_card", card)

remote func receive_card(card):
	print("remote received card: %d" % card)
	me.receive_card(card)
	me.show_hand()

remote func receive_trump(card):
	print("remote received trump: %d" % card)
	set_trump(card)
	
func send_trump(card):
	for player_id in players:
		rpc_id(player_id,"receive_trump",card)

func set_trump(trump_card_value):
	trumpCard = Card.instance()
	add_child(trumpCard)
	trumpCard.set_value(trump_card_value)
	trumpCard.set_faceup(true)
	trumpCard.position = $TrumpCardAnchor.position

func deal_fake_hands_to_others(num_cards):
	for _cardnum in range(num_cards):
		for player in players:
			if player != me:
				player.receive_card(52)

func play_round():
	currentTrick = Trick.instance()
	add_child(currentTrick)
	currentTrick.position = Vector2(512,300)
	for player in players:
		player.take_turn()
		yield(player, "play_card")
	yield(get_tree().create_timer(3), "timeout")
	currentTrick.queue_free()

func on_play_card(ref):
	var player = ref.get_parent()
	player.remove_child(ref)
	currentTrick.add_child(ref)
	currentTrick.accept_card(player, ref)
