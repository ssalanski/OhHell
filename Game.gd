extends Node2D

export(PackedScene) var Hand

# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	set_table()

func set_table():
	var player = Hand.instance()
	add_child(player)
	player.position = $MainPlayerAnchor.position
	player.receive_card(3)
	player.receive_card(5)
	player.receive_card(12)
	player.receive_card(46)
	player.receive_card(50)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
