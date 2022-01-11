extends Node2D

export(PackedScene) var Card
export(PackedScene) var Hand
export(PackedScene) var Trick

const num_players = 4 # TODO: make adjustable

var players = []
var trumpCard
var currentTrick

func _init():
	randomize() # TODO: this may move elsewhere

# Called when the node enters the scene tree for the first time.
func _ready():
	yield(get_tree().create_timer(1),"timeout")
	set_table()
	deal_hand(5)
	for _cardnum in range(5):
		print("playing round %d"%_cardnum)
		yield(play_round(), "completed")

func set_table():
	var player = Hand.instance()
	add_child(player)
	players.append(player)
	player.position = $MainPlayerAnchor.position
	player.show_hand()
	player.connect("play_card", self, "on_play_card")
	for i in range(1,num_players):
		var otherPlayer = Hand.instance()
		add_child(otherPlayer)
		players.append(otherPlayer)
		otherPlayer.connect("play_card", self, "on_play_card")
		otherPlayer.position = Vector2(0,200).rotated(i*3.14/2) + Vector2(512,300)
	

func deal_hand(num_cards):
	var deck = range(0,52)
	deck.shuffle()
	for _cardnum in range(num_cards):
		for player in players:
			player.receive_card(deck.pop_back())
	for player in players:
		player.show_hand()
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
