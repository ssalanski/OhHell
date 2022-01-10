extends Node2D

var value = null
var faceup = false

const FACEDOWN = 52

# Called when the node enters the scene tree for the first time.
func _ready():
	$CardFace.frame = 52

func set_faceup(f):
	faceup = f
	update_face()

func set_value(v):
	value = v
	update_face()

func update_face():
	if faceup:
		$CardFace.frame = value
	else:
		$CardFace.frame = FACEDOWN
