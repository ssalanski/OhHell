extends Node2D

export(PackedScene) var Card
export(PackedScene) var Hand
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

func play_hand(num_cards):
	deal_hand(num_cards)
	for trick_num in range(num_cards):
		print("playing round %d"%trick_num)
		yield(play_round(), "completed")

var _player_list
func set_players(player_list):
	_player_list = player_list

func seat_players():
	print("my id is: " + str(get_tree().get_network_unique_id()))
	for player_info in _player_list:
		var player = Hand.instance()
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
	

func deal_hand(num_cards):
	var deck = range(0,52)
	deck.shuffle()
	for _cardnum in range(num_cards):
		for player in players:
			player.receive_card(deck.pop_back())
	me.show_hand()
	trumpCard = Card.instance()
	add_child(trumpCard)
	trumpCard.set_value(deck.pop_back())
	trumpCard.set_faceup(true)
	trumpCard.position = $TrumpCardAnchor.position

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
