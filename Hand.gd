extends Node2D

export(PackedScene) var Card

signal play_card(ref)


var cards = []
var max_click_idx = -1
var primed_card = null
var can_play = false

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
	print(max_click_idx)
	set_process(true)

func _process(_delta):
	var clicked_card = cards[max_click_idx]
	if is_legal(clicked_card):
		if primed_card == clicked_card:
			if can_play:
				cards.remove(max_click_idx)
				primed_card.disconnect("card_clicked", self, "on_card_clicked")
				can_play = false
				emit_signal("play_card", primed_card)
			else:
				print("cant play, not your turn")
		else:
			primed_card = clicked_card
		print("%d cards in hand" % cards.size())
		arrange_hand()
	max_click_idx = -1
	set_process(false)

func is_legal(card):
	var leadSuit = get_parent().currentTrick.leadSuit
	if leadSuit == null:
		print("mycardsuit:%d leadcardsuit:null"%[card.get_suit()])
	else:
		print("mycardsuit:%d leadcardsuit:%d"%[card.get_suit(),leadSuit])
	if card.get_suit() == leadSuit:
		return true
	for c in cards:
		if c.get_suit() == leadSuit:
			return false
	return true

func take_turn():
	can_play = true
