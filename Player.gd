extends Node2D

@export var CardScene : PackedScene

signal card_played(player_id, ref)

# player data (TODO: should separate player and hand constructs)
var playername = "": set = set_player_name
var seat
var next_player

var is_a_real_boy = true

# hand data
var cards = []
var max_click_idx = -1
var primed_card = null
var can_play = false

func set_player_name(pname):
	playername = pname
	$NameLabel.text = playername

# Called when the node enters the scene tree for the first time.
func _ready():
	set_process(false)


func receive_card(card):
	# place card instance in hand
	var cardInstance = CardScene.instantiate()
	cards.append(cardInstance)
	add_child(cardInstance)
	arrange_hand()
	if is_multiplayer_authority():
		# if we're the master node, set value and connect signal
		cardInstance.set_value(card)
		cardInstance.set_faceup(true)
		cardInstance.card_clicked.connect(on_card_clicked)
	else:
		# TODO: this is for debug purposes
		cardInstance.set_value(card)
		#cardInstance.set_faceup(true)

@rpc("any_peer","call_local", "reliable")
func play_card(card_idx):
	# anyone can play a card
	# signal the game, which player played what card
	var card_ref = cards[card_idx]
	cards.remove_at(card_idx)
	print("signal from %d that a %s was played (by them, or CPU)" % [multiplayer.get_remote_sender_id(), str(card_ref)])
	card_played.emit(multiplayer.get_remote_sender_id(), card_ref)


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
	var clicked_card = cards[max_click_idx]
	if is_legal(clicked_card):
		if primed_card == clicked_card:
			if can_play:
				primed_card.card_clicked.disconnect(on_card_clicked)
				play_card.rpc(max_click_idx)
				can_play = false
			else:
				print("cant play, not your turn")
		else:
			primed_card = clicked_card
		print("%d cards in hand" % cards.size())
		arrange_hand()
	max_click_idx = -1
	set_process(false)

func is_legal(card):
	var lead = get_parent().currentTrick.leadCard
	if lead == null:
		return true
	var leadSuit = lead.get_suit()
	if card.get_suit() == leadSuit:
		return true
	for c in cards:
		if c.get_suit() == leadSuit:
			return false
	return true

func take_turn():
	print(name + "'s turn")
	if is_multiplayer_authority():
		if is_a_real_boy:
			print("MY turn!")
			can_play = true
		else:
			print("local CPU's turn: " + playername)
			ai_take_turn()

func ai_take_turn():
	print(playername + " taking its turn")
	for c in cards:
		print(playername + " considering playing " + str(c))
		if is_legal(c):
			print(str(c) + " is legal!")
			# simulate thinking time
			await get_tree().create_timer(3).timeout
			rpc("play_card", cards.find(c))
			return
