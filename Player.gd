extends Node2D

export(PackedScene) var Card

signal play_card(ref)

# player data (TODO: should separate player and hand constructs)
var playername
var seat

# hand data
var cards = []
var max_click_idx = -1
var primed_card = null
var can_play = false

# Called when the node enters the scene tree for the first time.
func _ready():
	set_process(false)


# runs everywhere, including caller
remotesync func receive_card(card):
	# only accept cards from server
	if get_tree().get_rpc_sender_id() == 1:
		# place card instance in hand
		var cardInstance = Card.instance()
		cards.append(cardInstance)
		add_child(cardInstance)
		arrange_hand()
		if is_network_master():
			# if we're the master node, set value and connect signal
			cardInstance.set_value(card)
			cardInstance.set_faceup(true)
			cardInstance.connect("card_clicked", self, "on_card_clicked")
		else:
			# TODO: this is for debug purposes
			cardInstance.set_value(card)
			cardInstance.set_faceup(true)

# runs everywhere, including caller
remotesync func play_card(card):
	# anyone can play a card
	# signal the game, which player played what card
	emit_signal("play_card", get_tree().get_rpc_sender_id(), card)

	
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