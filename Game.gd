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
	pass


func run_game():
	# runs only on the master node
	if get_tree().is_network_server():
		pass
	# this function, run only by the master node, will progress the game hand by hand, trick by trick, turn by turn
	pass

# runs everywhere
remotesync func set_trump(card):
	# only the server can set trump
	if get_tree().get_rpc_sender_id() == 1:
		trumpCard = Card.instance()
		add_child(trumpCard)
		trumpCard.set_value(card)
		trumpCard.set_faceup(true)
		trumpCard.position = $TrumpCardAnchor.position


func seat_players(_player_list):
	print("my id is: " + str(get_tree().get_network_unique_id()))
	for player_info in _player_list:
		var player = Player.instance()
		player.set_name(str(player_info["id"]))
		player.set_network_master(player_info["id"])
		player.playername = player_info["name"]
		player.seat = player_info["seat"]
		players.append(player)
		add_child(player)
		print("added player: " + player.name)
		player.connect("play_card", self, "on_play_card")
		if player.name == str(get_tree().get_network_unique_id()):
			me = player
	# arrange players around table, with network master seated at the front, but maintaining order around the table
	for player in players:
		if player == me:
			player.position = $MainPlayerAnchor.position
		else:
			if players.size() == 2:
				player.position = Vector2(512,100)
			elif players.size() == 3:
				player.position= Vector2((((player.seat - me.seat) % 3) - 1.5) * 200 + 512, 250)
			else:
				var rotated_slot_num = (player.seat - me.seat) % players.size()
				var spread = PI/(players.size()-2)
				player.position = Vector2(0,200).rotated(rotated_slot_num*spread) + Vector2(512,300)


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
	rpc("send_trump", deck.pop_back())

func send_card(id, card):
	print("sending card %d to %d" % [card,id])
	rpc_id(id, "receive_card", card)

remote func receive_card(card):
	print("remote received card: %d" % card)
	me.receive_card(card)
	me.show_hand()

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
