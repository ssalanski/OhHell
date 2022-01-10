extends Node2D

export(PackedScene) var Hand

const num_players = 4 # TODO: make adjustable

var players = []

# Called when the node enters the scene tree for the first time.
func _ready():
	set_table()

func set_table():
	var player = Hand.instance()
	add_child(player)
	players.append(player)
	player.position = $MainPlayerAnchor.position
	player.receive_card(3)
	player.receive_card(5)
	player.receive_card(32)
	player.receive_card(46)
	player.receive_card(50)
	player.show_hand()
	for i in range(1,num_players):
		var otherPlayer = Hand.instance()
		add_child(otherPlayer)
		otherPlayer.position = Vector2(0,300).rotated(i*3.14/2) + Vector2(512,300)
		otherPlayer.receive_card(3)
		otherPlayer.receive_card(5)
		otherPlayer.receive_card(32)
		otherPlayer.receive_card(46)
		otherPlayer.receive_card(50)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
