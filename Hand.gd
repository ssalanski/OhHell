extends Node2D

export(PackedScene) var Card

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var cards = []

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

func show_hand():
	for card in cards:
		card.set_faceup(true)

func receive_card(c):
	var new_card = Card.instance()
	cards.append(new_card)
	add_child(new_card)
	new_card.set_value(c)
	arrange_hand()
	
const CARD_GAP = 30
func arrange_hand():
	var h = -CARD_GAP/2 * (cards.size() - 1)
	for card in cards:
		card.position = Vector2(h,0)
		h = h + CARD_GAP

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
