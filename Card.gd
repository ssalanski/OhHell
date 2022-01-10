extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	$CardFace.frame = 52

func set_value(val):
	$CardFace.frame = val

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
