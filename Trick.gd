extends Node2D


var cards = {}
var trumpSuit = null
var leadCard = null


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func accept_card(player, card):
	card.set_faceup(true)
	card.position = position.direction_to(player.position) * 50
	card.look_at(player.position)
	card.rotate(PI/2)
	cards[card] = player
	if leadCard == null:
		leadCard = card

func get_winner():
	var winning_card = leadCard
	for card in cards.keys():
		if card.beats(winning_card, leadCard.get_suit(), trumpSuit):
			winning_card = card
	return cards[winning_card]