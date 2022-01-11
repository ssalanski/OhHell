extends Node2D

export(PackedScene) var Hand

const num_players = 4 # TODO: make adjustable

var players = []

func _init():
	randomize() # TODO: this may move elsewhere

# Called when the node enters the scene tree for the first time.
func _ready():
	set_table()
	deal_hand(5)

func set_table():
	var player = Hand.instance()
	add_child(player)
	players.append(player)
	player.position = $MainPlayerAnchor.position
	player.show_hand()
	for i in range(1,num_players):
		var otherPlayer = Hand.instance()
		add_child(otherPlayer)
		players.append(otherPlayer)
		otherPlayer.position = Vector2(0,200).rotated(i*3.14/2) + Vector2(512,300)
	

func deal_hand(num_cards):
	var deck = range(0,52)
	deck.shuffle()
	for cardnum in range(num_cards):
		for player in players:
			player.receive_card(deck.pop_back())
	for player in players:
		player.show_hand()


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
