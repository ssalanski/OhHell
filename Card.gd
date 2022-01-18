extends Node2D

signal card_clicked(ref)

var value = null
var faceup = false

const FACEDOWN = 52

# Called when the node enters the scene tree for the first time.
func _ready():
	$CardFace.frame = FACEDOWN

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

# numeric values for comparison purposes
# club=0,diamond=1,heart=2,spade=3 (since that's the order in cardsheet.png)
func get_suit():
	return int(value/13)
# 2=0,3=1,...Q=10,K=11,A=12
func get_denom():
	return value % 13

func beats(othercard, lead_suit, trump_suit):
	if get_suit() == trump_suit:
		if othercard.get_suit() == trump_suit:
			return get_denom() > othercard.get_denom()
		else:
			return true
	elif get_suit() == lead_suit:
		if othercard.get_suit() == lead_suit:
			return get_denom() > othercard.get_denom()
		else:
			return true
	else:
		return false


func _on_CardNode_input_event(_viewport, event, _shape_idx):
	if event is InputEventMouseButton:
		if event.pressed:
			emit_signal("card_clicked", self)
