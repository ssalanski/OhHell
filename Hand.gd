extends Node2D

export(PackedScene) var Card

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var cards = []
var max_click_idx = -1
var primed_card = null

# Called when the node enters the scene tree for the first time.
func _ready():
	set_process(false)

func show_hand():
	for card in cards:
		card.set_faceup(true)

func receive_card(c):
	var new_card = Card.instance()
	cards.append(new_card)
	add_child(new_card)
	new_card.set_value(c)
	arrange_hand()
	new_card.connect("card_clicked", self, "on_card_clicked")
	
const CARD_GAP = 30
func arrange_hand():
	var h = -CARD_GAP/2 * (cards.size() - 1)
	for card in cards:
		var v = 0
		if card == primed_card:
			v = -20
		card.position = Vector2(h,v)
		h = h + CARD_GAP


func on_card_clicked(ref):
	max_click_idx = max(cards.find(ref), max_click_idx)
	set_process(true)

func _process(_delta):
	primed_card = cards[max_click_idx]
	max_click_idx = -1
	arrange_hand()
	set_process(false)
